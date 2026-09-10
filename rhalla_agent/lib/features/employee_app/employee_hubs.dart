import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../chat/chat_screen.dart';
import 'employee_header.dart';
import 'employee_session.dart';

/* ══════════════════════════════════════════════════════════════════════════
   الحوالاتُ المحلّية — إنشاء · حوالاتي · كشف حوالاتي
   ══════════════════════════════════════════════════════════════════════════

   ⚠ **لا شاشةَ جديدةً تحتها.** الثلاثةُ موجودةٌ كلُّها منذ قبل الأمر:
   الإنشاءُ شاشةُ الوكيل نفسُها (`/send/internal`)، و«حوالاتي» تبويبُ الوكيل
   نفسُه بمعامل (`TransfersScreen(asEmployee: true)`)، والكشفُ كشفُه. وهذه
   الشاشةُ ليست إلّا الفهرسَ الذي يجمعها كما نصّت الشجرة.
   ══════════════════════════════════════════════════════════════════════════ */

class EmployeeLocalHubScreen extends ConsumerWidget {
  const EmployeeLocalHubScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final p = ref.watch(employeeAuthProvider).profile;

    final canCreate   = p?.can('CREATE_TRANSFER') ?? false;
    final canOwn      = p?.can('VIEW_OWN_TRANSFERS') ?? false;
    final canIncoming = p?.can('VIEW_INCOMING_TRANSFERS') ?? false;
    final canDeliver  = p?.can('DELIVER_TRANSFER') ?? false;

    final tiles = <Widget>[
      /*
       * ⚠ يفتح **شاشة الوكيل نفسَها**: أمرُ المالك أن الموظف واجهةٌ من
       * وكيل لا كيانٌ ثانٍ، فنموذجُ الحوالة واحدٌ للاثنين. والمستودعُ وحده
       * يبدّل المسار إلى نظيره تحت `device/employee/`.
       */
      if (canCreate)
        EmployeeTile(
          icon: Icons.north_east_rounded,
          title: 'إنشاء حوالة',
          subtitle: 'حوالة محلية باسم الوكيل',
          onTap: () => context.push('/send/internal'),
        ),

      /*
       * ⚠ «حوالاتي» هي تبويبُ حوالات الوكيل بعين الموظف: صادرةٌ بشرائحها
       * (الكل · في الطريق) وواردةٌ بتبويباتها الثلاثة (غير مسلَّمة · تم
       * التسليم · ملغاة)، بالبحث والفلترة والفاتورة والتسليم — شيفرةٌ
       * واحدة تُبنى مرّتين.
       */
      if (canOwn || canIncoming || canDeliver)
        EmployeeTile(
          icon: Icons.receipt_long_outlined,
          title: 'حوالاتي',
          subtitle: canIncoming || canDeliver
              ? 'صادرة (الكل · في الطريق) · واردة (غير مسلَّمة · تم التسليم · ملغاة)'
              : 'صادرة — الكل · في الطريق',
          onTap: () => context.push('/employee/transfers'),
        ),

      // كشفُ حوالاته المحلّية — بفلاتره كاملةً كما نصّ الأمر.
      if (canOwn)
        EmployeeTile(
          icon: Icons.description_outlined,
          title: 'كشف حوالاتي',
          subtitle: 'الوارد والصادر · اليوم · أسبوع · شهر · الكل · من–إلى',
          onTap: () => context.push('/employee/statement/local'),
        ),
    ];

    return _Hub(
      title: 'حوالات محلية',
      tiles: tiles,
      emptyText: 'لم تُمنح صلاحيةً في الحوالات المحلية.\n\nراجع وكيلك.',
    );
  }
}

/* ══════════════════════════════════════════════════════════════════════════
   الحوالاتُ الخارجية — إنشاء · حوالاتي · كشف الحوالات
   ══════════════════════════════════════════════════════════════════════════

   ⚠ **ولا واردةَ لها**، ونصُّ الأمر صريح: «الخارجيةُ ليس لها واردة، فشاشةُ
   حوالاتي فيها صادرةٌ فقط، ولا يُعرض تبويبٌ فارغٌ باسم الواردة».

   ⚠ وشاشةُ الإنشاء **شاشةُ الوكيل نفسُها** (`/send/external`): جُلبت
   بالمعامل لا بالنسخ، ومسارُ الخادم هو مسارُ الوكيل نفسُه يُنادى بهويّته
   تحت حارس جلسة الموظف. لا منطقَ ماليَّ جديد.
   ══════════════════════════════════════════════════════════════════════════ */

class EmployeeExternalHubScreen extends ConsumerWidget {
  const EmployeeExternalHubScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final p = ref.watch(employeeAuthProvider).profile;

    final canCreate = p?.can('CREATE_EXTERNAL_TRANSFER') ?? false;
    final canOwn    = p?.can('VIEW_OWN_TRANSFERS') ?? false;

    final tiles = <Widget>[
      if (canCreate)
        EmployeeTile(
          icon: Icons.flight_takeoff_rounded,
          title: 'إنشاء حوالة',
          subtitle: 'حوالة خارجية باسم الوكيل',
          onTap: () => context.push('/send/external'),
        ),

      if (canOwn)
        EmployeeTile(
          icon: Icons.north_east_rounded,
          title: 'حوالاتي',
          subtitle: 'صادرة — ما أنشأتَه إلى الخارج',
          onTap: () => context.push('/employee/external/mine'),
        ),

      if (canOwn)
        EmployeeTile(
          icon: Icons.description_outlined,
          title: 'كشف الحوالات',
          subtitle: 'اليوم · أسبوع · شهر · الكل · من–إلى',
          onTap: () => context.push('/employee/statement/external'),
        ),
    ];

    return _Hub(
      title: 'حوالات خارجية',
      tiles: tiles,
      emptyText: 'لم تُمنح صلاحيةً في الحوالات الخارجية.\n\n'
          'صلاحيةُ «إنشاء حوالة خارجية» مستقلّةٌ عن المحلية — راجع وكيلك.',
    );
  }
}

/// فهرسُ قسمٍ — العنصرُ المشترك بين الحوالتين.
///
/// ⚠ واحدٌ لا اثنان: فهرسان بمظهرين يجعلان القسمين يبدوان من تطبيقين،
/// والأمرُ ينصّ على أن «سياقَ العرض بين المحلية والخارجية مطابقٌ تماماً».
class _Hub extends StatelessWidget {
  const _Hub({
    required this.title,
    required this.tiles,
    required this.emptyText,
  });

  final String title;
  final List<Widget> tiles;
  final String emptyText;

  @override
  Widget build(BuildContext context) => Screen(
        child: Column(
          children: [
            RhallaAppBar(title: title, onBack: () => context.pop()),
            Expanded(
              child: tiles.isEmpty
                  ? EmployeeEmpty(
                      icon: Icons.lock_outline_rounded, text: emptyText)
                  : ListView.separated(
                      padding: const EdgeInsets.fromLTRB(
                          R.padScreen, 16, R.padScreen, 30),
                      itemCount: tiles.length,
                      separatorBuilder: (_, _) =>
                          const SizedBox(height: R.gapRow),
                      itemBuilder: (_, i) => tiles[i],
                    ),
            ),
          ],
        ),
      );
}

/* ══════════════════════════════════════════════════════════════════════════
   «حوالاتي» الخارجية — قائمةُ ما أنشأه هذا الموظف إلى الخارج
   ══════════════════════════════════════════════════════════════════════════ */

/// صفٌّ من حوالات الموظف الخارجية.
///
/// ⚠ شكلُه شكلُ نظيرته المحلّية حرفاً بحرف — الخادمُ يعيد المفاتيح نفسَها —
/// كي تُعرض بالبطاقة نفسِها. وبطاقتان تفترقان عند أوّل تعديل.
class ExternalMovement {
  const ExternalMovement({
    required this.code,
    required this.amount,
    required this.commission,
    required this.beneficiary,
    required this.beneficiaryPhone,
    required this.destination,
    required this.date,
    required this.missingInCore,
  });

  final String code;
  final double? amount;
  final double? commission;
  final String? beneficiary;
  final String? beneficiaryPhone;

  /// الوجهة — دولةُ المستفيد. موضعُ «الفرع» في الحوالة المحلّية.
  final String? destination;
  final String date;
  final bool missingInCore;

  static ExternalMovement fromJson(Map<String, dynamic> j) => ExternalMovement(
        code: '${j['transfer_number'] ?? ''}',
        amount: j['amount'] == null ? null : Fmt.num_(j['amount']),
        commission:
            j['commission'] == null ? null : Fmt.num_(j['commission']),
        beneficiary: (j['beneficiary'] as String?)?.trim(),
        beneficiaryPhone: (j['beneficiary_phone'] as String?)?.trim(),
        destination: (j['branch'] as String?)?.trim(),
        date: '${j['date'] ?? ''}',
        missingInCore: j['missing_in_core'] == true,
      );
}

final employeeExternalMineProvider =
    FutureProvider.autoDispose<List<ExternalMovement>>((ref) async {
  final env = await ref
      .watch(apiClientProvider)
      .get('/device/employee/external/mine', query: {'limit': 200});
  final row = env.row ?? const {};
  return ((row['items'] as List?) ?? const [])
      .map((e) => (e as Map).cast<String, dynamic>())
      .map(ExternalMovement.fromJson)
      .toList();
});

class EmployeeExternalMineScreen extends ConsumerWidget {
  const EmployeeExternalMineScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final async = ref.watch(employeeExternalMineProvider);

    return Screen(
      child: Column(
        children: [
          // ⚠ «صادرة» وحدها في العنوان الفرعيّ، لا تبويباً فارغاً للواردة:
          // الخارجيةُ تخرج ولا تعود، ونصُّ الأمر عليه صريح.
          RhallaAppBar(
            title: 'حوالاتي الخارجية',
            subtitle: 'صادرة',
            onBack: () => context.pop(),
          ),
          Expanded(
            child: RefreshIndicator(
              onRefresh: () async =>
                  ref.invalidate(employeeExternalMineProvider),
              color: R.primary,
              backgroundColor: Colors.white,
              child: async.when(
                loading: () => const Center(child: CircularProgressIndicator()),
                error: (e, _) => ListView(children: [
                  EmployeeEmpty(
                      icon: Icons.wifi_off_rounded,
                      text: 'تعذّر تحميل القائمة.\n$e'),
                ]),
                data: (items) => items.isEmpty
                    ? ListView(
                        physics: const AlwaysScrollableScrollPhysics(),
                        children: const [
                          EmployeeEmpty(
                            icon: Icons.public_off_rounded,
                            text: 'لم تُنشئ حوالةً خارجيةً بعد.',
                          ),
                        ],
                      )
                    : ListView.separated(
                        padding: const EdgeInsets.fromLTRB(
                            R.padScreen, 14, R.padScreen, 30),
                        itemCount: items.length,
                        separatorBuilder: (_, _) => const SizedBox(height: 8),
                        itemBuilder: (_, i) => _ExternalRow(m: items[i]),
                      ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}

class _ExternalRow extends StatelessWidget {
  const _ExternalRow({required this.m});
  final ExternalMovement m;

  @override
  Widget build(BuildContext context) => GlassCard(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                IconTile(
                  size: 36,
                  background: R.primaryA(.12),
                  icon: Icon(Icons.flight_takeoff_rounded,
                      size: 18, color: R.primaryDark),
                ),
                const SizedBox(width: 10),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        m.beneficiary?.isNotEmpty == true
                            ? m.beneficiary!
                            : 'بلا اسم مستفيد',
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                        style: T.kufi(13.5, FontWeight.w700),
                      ),
                      const SizedBox(height: 2),
                      Directionality(
                        // رقمٌ لاتينيّ في فقرةٍ عربية — يُفرض اتجاهه، وإلّا
                        // قلبته الفقرةُ فقرأه الموظف معكوساً.
                        textDirection: TextDirection.ltr,
                        child: Text(m.code,
                            style: T.plex(11, FontWeight.w400,
                                color: R.inkA(.5))),
                      ),
                    ],
                  ),
                ),
                if (m.amount != null)
                  Directionality(
                    textDirection: TextDirection.ltr,
                    child: Row(
                      children: [
                        // العملةُ يسارَ المبلغ — قاعدةُ التطبيق كلِّه.
                        Text('د.ل',
                            style: T.kufi(11.5, FontWeight.w700,
                                color: R.primaryDark)),
                        const SizedBox(width: 4),
                        Text(Fmt.money(m.amount!),
                            style: T.kufi(14.5, FontWeight.w800)),
                      ],
                    ),
                  ),
              ],
            ),
            const SizedBox(height: 10),
            Divider(color: R.inkA(.07), height: 1),
            const SizedBox(height: 8),
            Row(
              children: [
                if (m.destination?.isNotEmpty == true) ...[
                  Icon(Icons.place_outlined, size: 13, color: R.inkA(.4)),
                  const SizedBox(width: 3),
                  Text(m.destination!,
                      style:
                          T.plex(11, FontWeight.w500, color: R.inkA(.6))),
                  const SizedBox(width: 12),
                ],
                if (m.commission != null) ...[
                  Text('عمولة',
                      style:
                          T.plex(10.5, FontWeight.w400, color: R.inkA(.45))),
                  const SizedBox(width: 4),
                  Directionality(
                    textDirection: TextDirection.ltr,
                    child: Text(Fmt.money(m.commission!),
                        style: T.plex(11, FontWeight.w600,
                            color: R.inkA(.65))),
                  ),
                ],
                const Spacer(),
                Text(Fmt.stampShort(m.date),
                    style:
                        T.plex(10.5, FontWeight.w400, color: R.inkA(.45))),
              ],
            ),

            /*
             * ⚠ صفٌّ لا أصلَ له في المنظومة يُعرض ويُعلَّم، ولا يُخفى.
             *
             * إخفاؤه يجعل الموظف يقرأ كشفاً ينقصه صفّ، فلا يطابق الدرج
             * ولا يعرف لماذا. وقولُ الحقيقة أنفعُ من صمتٍ مرتّب.
             */
            if (m.missingInCore) ...[
              const SizedBox(height: 8),
              Row(
                children: [
                  Icon(Icons.report_problem_outlined, size: 13, color: R.warnIcon),
                  const SizedBox(width: 4),
                  Expanded(
                    child: Text(
                      'لم يُعثر على أصل هذه الحوالة في المنظومة — راجع وكيلك.',
                      style: T.plex(10.5, FontWeight.w500, color: R.warnIcon),
                    ),
                  ),
                ],
              ),
            ],
          ],
        ),
      );
}

/* ══════════════════════════════════════════════════════════════════════════
   التبويبُ الرابع — مراسلة الوكيل
   ══════════════════════════════════════════════════════════════════════════ */

/// محادثةُ الموظف مع وكيله — **شاشةُ المحادثة نفسُها** لا نسخةٌ عنها.
///
/// ⚠ وهي محادثةٌ واحدة لا قائمةُ محادثات: الموظف يراسل وكيلَه وحدَه، فقائمةٌ
/// بعنصرٍ واحد نقرةٌ زائدة قبل كل رسالة.
///
/// ⚠ وحشوةٌ سفليّة بمقدار الشريط: مربّعُ الكتابة يقع في أسفل الشاشة،
/// والشريطُ الزجاجيّ يعلو المحتوى — فبدونها يكتب الموظف تحت الشريط ولا يرى
/// ما يكتب.
class EmployeeChatTab extends ConsumerWidget {
  const EmployeeChatTab({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final p = ref.watch(employeeAuthProvider).profile;

    // ⚠ تبويبٌ بلا صلاحية يقول ما ينقصه ولا يختفي، ولا يفتح ثمّ يردّ 403.
    if (!(p?.can('CHAT_WITH_AGENT') ?? false)) {
      return Screen(
        child: Column(
          children: [
            if (p != null) EmployeeHeader(profile: p),
            const Expanded(
              child: EmployeeEmpty(
                icon: Icons.lock_outline_rounded,
                text: 'تحتاج صلاحية «مراسلة الوكيل» لفتح المحادثة.\n\n'
                    'راجع وكيلك.',
              ),
            ),
          ],
        ),
      );
    }

    return const Padding(
      padding: EdgeInsets.only(bottom: 92),
      child: ChatScreen(title: 'الوكيل', asEmployee: true, asTab: true),
    );
  }
}
