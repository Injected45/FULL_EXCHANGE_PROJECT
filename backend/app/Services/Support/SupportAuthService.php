<?php

namespace App\Services\Support;

use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Hash;
use Illuminate\Support\Facades\RateLimiter;
use Illuminate\Support\Str;

/**
 * دخول موظّفي الدعم — حسابٌ لكلّ شخص، بدل المفتاح المشترك.
 *
 * ما كان: مفتاحٌ واحد في `.env`، من عرفه دخل، ويكتب اسمه بيده عند الدخول.
 * فالسجلُّ يقول «ردّ محمد» لأن أحدَهم كتب «محمد» — لا لأنه محمد. ولا يمكن
 * منعُ شخصٍ بعينه إلا بتغيير المفتاح على الجميع.
 *
 * وما صار: اسمُ دخولٍ وكلمةُ مرورٍ ودورٌ لكلّ موظّف، وجلسةٌ تُلغى وحدها.
 *
 * ⚠ **جلسةُ الدعم ليست جلسةَ مستخدم.** لا `personal_access_tokens` ولا
 * `users`: موظّف الدعم ليس صفّاً هناك، وإبقاؤه خارجَه هو ما يمنع أن تصير
 * جلسةُ دعمٍ يوماً جلسةَ وكيلٍ بخطأ في وسيطٍ واحد. وهو القرارُ المتّخذ في
 * `employee_sessions` نفسُه، للسبب نفسه.
 */
class SupportAuthService
{
    /** مدّة الجلسة. يومٌ عملٍ طويل وشيءٌ فوقه، لا أسبوع. */
    public const SESSION_HOURS = 12;

    /** محاولاتُ دخولٍ فاشلة لاسمٍ واحد قبل الخنق. */
    private const MAX_ATTEMPTS = 8;
    private const LOCK_SECONDS = 300;

    /**
     * تسجيل دخول. يُرجع `[token, staff]` أو رسالةَ خطأ.
     *
     * الرسالةُ موحّدة عمداً: «الاسم أو كلمة المرور غير صحيحة» لا تقول
     * أيّهما، فلا تصلح لاكتشاف أسماء الدخول الموجودة. ويُستثنى من ذلك
     * الحسابُ المعطَّل — الموظّف يجب أن يعرف أن حسابه أُوقف، وإلا ظنّ
     * أنه نسي كلمته وأضاع وقتَه ووقتَ من يسأله.
     */
    public function login(string $username, string $password, ?string $ip, ?string $ua): array
    {
        $username = mb_strtolower(trim($username));

        if ($username === '' || $password === '') {
            return ['error' => 'اكتب اسم الدخول وكلمة المرور.'];
        }

        // خنقٌ باسم الدخول لا بالعنوان وحده: مكتبٌ كامل خلف عنوانٍ واحد،
        // وخنقُ العنوان يُقفل على الجميع بخطأ واحد منهم.
        $bucket = 'support-login:' . sha1($username . '|' . ($ip ?? ''));
        if (RateLimiter::tooManyAttempts($bucket, self::MAX_ATTEMPTS)) {
            $wait = RateLimiter::availableIn($bucket);
            return ['error' => "محاولات كثيرة. أعد المحاولة بعد {$wait} ثانية."];
        }

        $staff = DB::table('support_staff')
            ->whereRaw('LOWER(username) = ?', [$username])
            ->whereNull('deleted_at')
            ->first();

        if (!$staff || !Hash::check($password, $staff->password_hash)) {
            RateLimiter::hit($bucket, self::LOCK_SECONDS);
            return ['error' => 'اسم الدخول أو كلمة المرور غير صحيحة.'];
        }

        if (!$staff->is_active) {
            return ['error' => 'هذا الحساب موقوف. راجع مدير النظام.'];
        }

        RateLimiter::clear($bucket);

        $token = Str::random(64);

        DB::table('support_sessions')->insert([
            'staff_id'     => $staff->id,
            'token_hash'   => hash('sha256', $token),
            'expires_at'   => now()->addHours(self::SESSION_HOURS),
            'ip'           => $ip ? mb_substr($ip, 0, 45) : null,
            'user_agent'   => $ua ? mb_substr($ua, 0, 255) : null,
            'last_seen_at' => now(),
        ]);

        DB::table('support_staff')->where('id', $staff->id)
            ->update(['last_seen_at' => now()]);

        return ['token' => $token, 'staff' => $this->profile((int) $staff->id)];
    }

    /**
     * يحلّ الرمزَ إلى موظّف، أو `null`.
     *
     * ويُحدَّث `last_seen_at` هنا لأنه المكان الوحيد الذي يمرّ به كلُّ
     * نداء — «آخر ظهور» في شاشة المدير يجب أن يعني آخرَ نشاط، لا آخر
     * تسجيلِ دخول.
     */
    public function resolve(?string $token): ?object
    {
        if (!$token || strlen($token) < 32) {
            return null;
        }

        $row = DB::table('support_sessions as s')
            ->join('support_staff as st', 'st.id', '=', 's.staff_id')
            ->where('s.token_hash', hash('sha256', $token))
            ->whereNull('s.revoked_at')
            ->where('s.expires_at', '>', now())
            ->whereNull('st.deleted_at')
            ->where('st.is_active', 1)
            ->first([
                's.id as session_id', 'st.id', 'st.name', 'st.username',
                'st.role', 'st.must_change',
            ]);

        if (!$row) {
            return null;
        }

        DB::table('support_sessions')->where('id', $row->session_id)
            ->update(['last_seen_at' => now()]);

        $row->permissions = $this->permissionsOf((int) $row->id);

        return $row;
    }

    public function logout(?string $token): void
    {
        if (!$token) {
            return;
        }

        DB::table('support_sessions')
            ->where('token_hash', hash('sha256', $token))
            ->whereNull('revoked_at')
            ->update(['revoked_at' => now()]);
    }

    /** إلغاء جلسات موظّفٍ كلِّها — عند الإيقاف أو الحذف أو تغيير الكلمة. */
    public function revokeAll(int $staffId): void
    {
        DB::table('support_sessions')
            ->where('staff_id', $staffId)
            ->whereNull('revoked_at')
            ->update(['revoked_at' => now()]);
    }

    /**
     * تغيير كلمة المرور — ويُلغي كلَّ الجلسات بما فيها الحالية.
     *
     * إلغاءُ الجلسات هو نصفُ الغرض: من غيّر كلمتَه غالباً لأنه يخشى أن
     * أحداً يعرفها، وتغييرٌ يُبقي جلسةَ ذلك الأحد مفتوحةً لا يفعل شيئاً.
     */
    public function changePassword(int $staffId, string $current, string $new): ?string
    {
        $staff = DB::table('support_staff')->where('id', $staffId)
            ->whereNull('deleted_at')->first();

        if (!$staff) {
            return 'الحساب غير موجود.';
        }

        if (!Hash::check($current, $staff->password_hash)) {
            return 'كلمة المرور الحالية غير صحيحة.';
        }

        if (($msg = self::passwordProblem($new)) !== null) {
            return $msg;
        }

        if (Hash::check($new, $staff->password_hash)) {
            return 'الكلمة الجديدة هي نفسها الحالية.';
        }

        DB::table('support_staff')->where('id', $staffId)->update([
            'password_hash' => Hash::make($new),
            'must_change'   => 0,
        ]);

        $this->revokeAll($staffId);

        return null;
    }

    /**
     * شروطُ الكلمة — ثمانيةُ محارف وحرفٌ ورقم.
     *
     * ولا تعقيدَ فوق ذلك: قواعدُ الرموز الإجبارية تُنتج `Password1!` على
     * كلّ مكتب، وهي أسوأ من عبارةٍ طويلة يختارها صاحبها.
     */
    public static function passwordProblem(string $p): ?string
    {
        if (mb_strlen($p) < 8) {
            return 'كلمة المرور: ثمانية محارف على الأقل.';
        }
        if (!preg_match('/[A-Za-z\x{0600}-\x{06FF}]/u', $p) || !preg_match('/[0-9]/', $p)) {
            return 'كلمة المرور: حرفٌ ورقمٌ على الأقل.';
        }

        return null;
    }

    /** @return string[] */
    public function permissionsOf(int $staffId): array
    {
        return DB::table('support_permissions')
            ->where('staff_id', $staffId)
            ->pluck('permission')
            ->all();
    }

    public function profile(int $staffId): ?object
    {
        $s = DB::table('support_staff')->where('id', $staffId)
            ->whereNull('deleted_at')
            ->first(['id', 'name', 'username', 'role', 'must_change', 'is_active']);

        if ($s) {
            $s->permissions = $this->permissionsOf($staffId);
        }

        return $s;
    }
}
