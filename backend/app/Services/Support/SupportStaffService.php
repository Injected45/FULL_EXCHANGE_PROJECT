<?php

namespace App\Services\Support;

use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Hash;
use Illuminate\Support\Str;

/**
 * إدارة حسابات الدعم وصلاحياتها.
 *
 * ⚠ **Default Deny** في طبقتين: سقفُ الدور
 * ({@see SupportPermissions::ROLE_CEILING}) يقول ما يجوز، وصفوفُ
 * `support_permissions` تقول ما مُنح. ومنعُ الأولى لا يُتجاوَز بالثانية.
 *
 * ولا شيء هنا يمسّ المال: حساباتُ موظّفي دعمٍ وصلاحياتُ محادثات.
 */
class SupportStaffService
{
    public function __construct(private SupportAuthService $auth)
    {
    }

    public function all(bool $withDeleted = false): array
    {
        $q = DB::table('support_staff')->orderBy('name');

        if (!$withDeleted) {
            $q->whereNull('deleted_at');
        }

        $rows = $q->get(['id', 'name', 'username', 'role', 'is_active',
                         'must_change', 'last_seen_at', 'created_at', 'deleted_at']);

        $ids = $rows->pluck('id')->map(fn ($v) => (int) $v)->all();

        // الصلاحياتُ لكلّ الموظّفين في استعلامٍ واحد، لا واحدٍ لكلّ صفّ.
        $perms = [];
        if ($ids !== []) {
            foreach (DB::table('support_permissions')->whereIn('staff_id', $ids)
                        ->get(['staff_id', 'permission']) as $p) {
                $perms[(int) $p->staff_id][] = $p->permission;
            }
        }

        return $rows->map(fn ($r) => [
            'id'           => (int) $r->id,
            'name'         => $r->name,
            'username'     => $r->username,
            'role'         => $r->role,
            'role_label'   => SupportPermissions::ROLES[$r->role] ?? $r->role,
            'is_active'    => (bool) $r->is_active,
            'must_change'  => (bool) $r->must_change,
            'last_seen_at' => $r->last_seen_at ? (string) $r->last_seen_at : null,
            'created_at'   => (string) $r->created_at,
            'deleted'      => $r->deleted_at !== null,
            'permissions'  => $perms[(int) $r->id] ?? [],
        ])->all();
    }

    /**
     * إنشاء حساب. يُرجع `['id'=>…, 'password'=>…]` أو `['error'=>…]`.
     *
     * الكلمةُ الأولى تُولَّد هنا وتُعرض مرّةً واحدة، ولا تُخزَّن نصّاً ولا
     * تُرسَل: من أنشأ الحساب يُسلّمها لصاحبها، و `must_change` تُجبره على
     * تغييرها في أوّل دخول. حسابٌ يبقى على كلمةِ من أنشأه ليس حساباً
     * شخصياً، والسجلُّ الذي يحمل اسمَه يصير شهادةَ زور.
     */
    public function create(string $name, string $username, string $role, object $by): array
    {
        $name     = trim($name);
        $username = mb_strtolower(trim($username));

        if (mb_strlen($name) < 2) {
            return ['error' => 'اكتب اسم الموظّف.'];
        }

        if (!preg_match('/^[a-z0-9._-]{3,60}$/', $username)) {
            return ['error' => 'اسم الدخول: حروفٌ لاتينية صغيرة وأرقام و . _ - فقط، 3 محارف فأكثر.'];
        }

        if (!SupportPermissions::roleExists($role)) {
            return ['error' => 'الدور غير معروف.'];
        }

        $taken = DB::table('support_staff')
            ->whereRaw('LOWER(username) = ?', [$username])
            ->whereNull('deleted_at')->exists();

        if ($taken) {
            return ['error' => 'اسم الدخول مستعمَل.'];
        }

        // كلمةٌ مولَّدة تستوفي الشرط بالبناء: حروفٌ وأرقام واثنا عشر محرفاً.
        $password = Str::lower(Str::random(8)) . random_int(1000, 9999);

        $id = DB::transaction(function () use ($name, $username, $role, $password, $by) {
            $id = (int) DB::table('support_staff')->insertGetId([
                'name'          => mb_substr($name, 0, 120),
                'username'      => $username,
                'password_hash' => Hash::make($password),
                'role'          => $role,
                'is_active'     => 1,
                'must_change'   => 1,
                'created_by'    => $by->id,
            ]);

            // صفوفُ البداية تُكتب فعلاً — لا استثناءَ في الفحص. انظر
            // `ROLE_DEFAULTS`.
            $rows = [];
            foreach (SupportPermissions::ROLE_DEFAULTS[$role] as $p) {
                $rows[] = [
                    'staff_id'   => $id,
                    'permission' => $p,
                    'granted_by' => $by->id,
                    'granted_at' => now(),
                ];
            }
            if ($rows !== []) {
                DB::table('support_permissions')->insert($rows);
            }

            return $id;
        });

        return ['id' => $id, 'password' => $password];
    }

    public function update(int $id, array $changes, object $by): array
    {
        $staff = DB::table('support_staff')->where('id', $id)->whereNull('deleted_at')->first();
        if (!$staff) {
            return ['error' => 'الحساب غير موجود.'];
        }

        $update = [];

        if (isset($changes['name'])) {
            $name = trim((string) $changes['name']);
            if (mb_strlen($name) < 2) {
                return ['error' => 'اكتب اسم الموظّف.'];
            }
            $update['name'] = mb_substr($name, 0, 120);
        }

        if (isset($changes['role'])) {
            $role = (string) $changes['role'];
            if (!SupportPermissions::roleExists($role)) {
                return ['error' => 'الدور غير معروف.'];
            }

            // خفضُ الدور يسحب ما صار فوق السقف — وإلا بقي موظّفُ دعمٍ يحمل
            // صلاحياتِ مشرفٍ لأنه كان مشرفاً بالأمس. والفحصُ عند كل نداء
            // يعتمد على الصفوف، فبقاؤها بقاءُ الصلاحية نفسها.
            if ($role !== $staff->role) {
                $update['role'] = $role;

                foreach ($this->auth->permissionsOf($id) as $p) {
                    if (!SupportPermissions::allowedForRole($role, $p)) {
                        DB::table('support_permissions')
                            ->where('staff_id', $id)->where('permission', $p)->delete();
                    }
                }
            }
        }

        if (array_key_exists('is_active', $changes)) {
            $active = (bool) $changes['is_active'];
            $update['is_active'] = $active ? 1 : 0;

            // إيقافٌ يُنهي الجلسات فوراً: حسابٌ موقوف وجلستُه مفتوحة إلى
            // الغد ليس موقوفاً.
            if (!$active) {
                $this->auth->revokeAll($id);
            }
        }

        if ($update !== []) {
            DB::table('support_staff')->where('id', $id)->update($update);
        }

        return ['ok' => true];
    }

    /** حذفٌ ناعم: الصفّ يبقى ليبقى لسجلّ النشاط معنى. */
    public function delete(int $id): array
    {
        $staff = DB::table('support_staff')->where('id', $id)->whereNull('deleted_at')->first();
        if (!$staff) {
            return ['error' => 'الحساب غير موجود.'];
        }

        DB::table('support_staff')->where('id', $id)->update([
            'deleted_at' => now(),
            'is_active'  => 0,
        ]);

        $this->auth->revokeAll($id);

        // الإسنادُ يُنزع لا يُترك: محادثةٌ مُسنَدة إلى محذوفٍ لا يراها أحد.
        DB::table('support_thread_state')->where('assigned_to', $id)->update([
            'assigned_to' => null,
            'assigned_at' => null,
            'status'      => DB::raw("CASE WHEN status = 'OPEN' THEN 'NEW' ELSE status END"),
            'updated_at'  => now(),
        ]);

        return ['ok' => true];
    }

    /** تعيين كلمةٍ جديدة من المدير — تُعرض مرّةً وتُجبر على التغيير. */
    public function resetPassword(int $id): array
    {
        $staff = DB::table('support_staff')->where('id', $id)->whereNull('deleted_at')->first();
        if (!$staff) {
            return ['error' => 'الحساب غير موجود.'];
        }

        $password = Str::lower(Str::random(8)) . random_int(1000, 9999);

        DB::table('support_staff')->where('id', $id)->update([
            'password_hash' => Hash::make($password),
            'must_change'   => 1,
        ]);

        $this->auth->revokeAll($id);

        return ['password' => $password];
    }

    /**
     * ضبطُ صلاحيات موظّفٍ دفعةً واحدة.
     *
     * دفعةً لا واحدةً واحدة: شاشةُ المنح مربّعاتُ اختيارٍ تُحفظ مرّة، وإرسالُ
     * فرقٍ لكلّ مربّع يترك الحالةَ نصفَ محفوظة إن انقطعت الشبكة في المنتصف.
     *
     * @param string[] $keys
     */
    public function setPermissions(int $id, array $keys, object $by): array
    {
        $staff = DB::table('support_staff')->where('id', $id)->whereNull('deleted_at')->first();
        if (!$staff) {
            return ['error' => 'الحساب غير موجود.'];
        }

        $wanted = [];
        foreach (array_unique($keys) as $k) {
            $k = (string) $k;
            if (!SupportPermissions::exists($k)) {
                return ['error' => "صلاحية غير معروفة: {$k}"];
            }
            // السقفُ يُفحص في الخادم لا في الشاشة وحدها.
            if (!SupportPermissions::allowedForRole($staff->role, $k)) {
                return ['error' => 'هذه الصلاحية لا تُمنح لهذا الدور: '
                    . (SupportPermissions::CATALOG[$k][1] ?? $k)];
            }
            $wanted[] = $k;
        }

        $current = $this->auth->permissionsOf($id);
        $add     = array_diff($wanted, $current);
        $remove  = array_diff($current, $wanted);

        DB::transaction(function () use ($id, $add, $remove, $by) {
            if ($remove !== []) {
                DB::table('support_permissions')->where('staff_id', $id)
                    ->whereIn('permission', array_values($remove))->delete();
            }

            $rows = [];
            foreach ($add as $p) {
                $rows[] = [
                    'staff_id'   => $id,
                    'permission' => $p,
                    'granted_by' => $by->id,
                    'granted_at' => now(),
                ];
            }
            if ($rows !== []) {
                DB::table('support_permissions')->insert($rows);
            }
        });

        return [
            'ok'      => true,
            'added'   => array_values($add),
            'removed' => array_values($remove),
        ];
    }

    /** الموظّفون الذين يجوز الإسنادُ إليهم. */
    public function assignees(): array
    {
        return DB::table('support_staff as s')
            ->join('support_permissions as p', function ($j) {
                $j->on('p.staff_id', '=', 's.id')->where('p.permission', 'REPLY');
            })
            ->whereNull('s.deleted_at')->where('s.is_active', 1)
            ->orderBy('s.name')
            ->get(['s.id', 's.name', 's.role'])
            ->map(fn ($r) => [
                'id'   => (int) $r->id,
                'name' => $r->name,
                'role' => SupportPermissions::ROLES[$r->role] ?? $r->role,
            ])->all();
    }
}
