import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import 'employee_header.dart';
import 'employee_session.dart';

/// تقاريرُ الموظف — كلُّ تقريرٍ خلف صلاحيته وحدَه.
///
/// ══════════════════════════════════════════════════════════════════════════
///  شاشةٌ واحدة، وتقاريرُ تظهر بحسب ما مُنح
/// ══════════════════════════════════════════════════════════════════════════
///
/// ⚠ **ولا يُعرض تقريرٌ لم يُمنح صاحبُه إيّاه.** والإخفاءُ تجميلٌ لا حماية:
/// الخادمُ يردّ 403 على كلّ نداء، والحارسُ هناك. لكنّ بلاطةً تُفتح فتُخفق
/// تُعلّم الموظف أن التطبيق معطوب، فلا تُعرض أصلاً.
///
/// ⚠ وكلُّها **قراءةٌ خالصة**: تعرض ما وقع ولا تغيّر منه شيئاً.
///
/// ⚠ وباسمٍ صريح: `EmployeeReportsScreen` مأخوذٌ لتقارير **الوكيل** عن
/// موظفيه في `features/employees/`. وهذه تقاريرُ الموظف عن نفسِه — شاشتان
/// مختلفتان لجمهورين مختلفين، واسمٌ واحد لهما يخلط بينهما عند أوّل استيراد.
/// ⚠ وبعد إعادة الهيكلة (10 سبتمبر 2026) صارت **تبويباً** في الشريط السفليّ
/// لا شاشةً تُدفع، ورأسُها «حوالات اليوم (الكل)» كما نصّ الأمر: محلّيةٌ
/// وخارجيةٌ معاً، مسلَّمةٌ وغير مسلَّمة، تُعرض كشفاً ويُصدَر منها كشف،
/// بالفلتر المعروف كاملاً.
///
/// ⚠ ولم يسقط منها شيء: التقاريرُ الستّةُ التي كانت هنا باقيةٌ تحته،
/// و«الأرصدة» انتقلت إليه من الشاشة الرئيسية — لأنها قراءةٌ لا عمل، وهذا
/// موضعُ القراءات.
class EmployeeOwnReportsScreen extends ConsumerWidget {
  const EmployeeOwnReportsScreen({super.key, this.asTab = false});

  /// تبويبٌ في الشريط السفليّ — فبلا زرّ رجوع، وبترويسة الموظف فوقه.
  final bool asTab;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final p = ref.watch(employeeAuthProvider).profile;

    final items = <ReportDef>[
      const ReportDef(
        key: 'REPORT_DAILY_TRANSFERS',
        title: 'حوالات اليوم',
        subtitle: 'ما أنشأتَه اليوم',
        icon: Icons.today_rounded,
        path: '/device/employee/reports/daily',
      ),
      const ReportDef(
        key: 'REPORT_DELIVERED_TRANSFERS',
        title: 'الحوالات المسلَّمة',
        subtitle: 'ما سلَّمتَه خلال شهر',
        icon: Icons.done_all_rounded,
        path: '/device/employee/reports/delivered',
      ),
      const ReportDef(
        key: 'REPORT_PENDING_TRANSFERS',
        title: 'غير مسلَّمة',
        subtitle: 'واردٌ لم يُسلَّم بعد',
        icon: Icons.pending_actions_rounded,
        path: '/device/employee/reports/pending',
      ),
      /*
       * ⚠ **مفتاحٌ متقاعدٌ كان يحرس تقريراً حيّاً — فأغلقه على الجميع.**
       *
       * كان الشرطُ هنا `REPORT_EMPLOYEE_CASHBOX`، وهو من المفاتيح الخمسة
       * التي خرجت من الكتالوج يوم أُلغيت العهدةُ والوردية (10 سبتمبر 2026).
       * ومفتاحٌ خارج الكتالوج **لا يُمنح لأحد** — فالبلاطةُ كانت محجوبةً عن
       * كلّ موظفٍ مهما مُنح.
       *
       * والمسارُ في الخادم يحرسه `employee:REPORT_POINT_OF_SALE` وحدَه، وهو
       * حيٌّ في الكتالوج. فوكيلٌ يمنح «تقرير نقطة البيع» كان يفتح البابَ في
       * الخادم ولا يظهر في التطبيق شيء — عيبٌ صامتٌ لا يُرى في `analyze`
       * ولا في تشغيل، ويُقرأ عند الوكيل «المنحُ لا يعمل».
       *
       * كشفه `employee_home_permissions_test` بعد أن صار يسأل عن **كلّ**
       * مفتاحٍ: هل له بابٌ في تبويبه؟
       */
      const ReportDef(
        key: 'REPORT_POINT_OF_SALE',
        title: 'تقرير نقطة البيع',
        subtitle: 'عملي على نقطة بيعي',
        icon: Icons.storefront_outlined,
        path: '/device/employee/reports/point-of-sale',
      ),
      const ReportDef(
        key: 'REPORT_AUDIT',
        title: 'سجل نشاطي',
        subtitle: 'ما قمتُ به خلال أسبوعين',
        icon: Icons.history_rounded,
        path: '/device/employee/reports/audit',
      ),
      const ReportDef(
        key: 'REPORT_AGENT_BALANCE',
        title: 'رصيد الوكيل',
        subtitle: 'الرصيد الحالي',
        icon: Icons.account_balance_wallet_outlined,
        path: '/device/employee/reports/agent-balance',
        isBalance: true,
      ),
    ];

    final allowed = items.where((r) => p?.can(r.key) ?? false).toList();

    /*
     * ══════════════════════════════════════════════════════════════════
     *  رأسُ التبويب — «حوالات اليوم (الكل)»
     * ══════════════════════════════════════════════════════════════════
     *
     * ⚠ وهو **الكشفُ نفسُه** الذي يُفتح تحت المحلّية والخارجية، بنطاق
     * «الكل». لا شاشةَ ثالثة: ثلاثُ شاشاتٍ متشابهة تفترق عند أوّل تعديل،
     * ثمّ يختلف مجموعُ «الكل» عن مجموع شطريه.
     *
     * ⚠ وبـ`VIEW_OWN_TRANSFERS` لا بمفتاح تقرير: هذه حوالاتُه هو، ومفتاحٌ
     * ثانٍ لبيانٍ يراه أصلاً يجعل الوكيل يمنح أحدهما ظانّاً أنه منع الآخر.
     */
    final head = <Widget>[
      if (p?.can('VIEW_OWN_TRANSFERS') ?? false)
        EmployeeTile(
          icon: Icons.today_rounded,
          title: 'حوالات اليوم — الكل',
          subtitle: 'محلية وخارجية · مسلَّمة وغير مسلَّمة · كشفٌ يُصدَر',
          onTap: () => context.push('/employee/statement/all'),
        ),

      /*
       * ⚠ «الأرصدة» نزلت إلى هنا من الشاشة الرئيسية ولم تُحذف — شرطُ الأمر:
       * «لا يُحذف حقلٌ ولا كلمة». وهي قراءةٌ لا عمل، وهذا موضعُ القراءات.
       */
      if ((p?.can('VIEW_AGENT_TOTAL_BALANCE') ?? false) ||
          (p?.can('VIEW_FINANCIAL_SUMMARY') ?? false))
        EmployeeTile(
          icon: Icons.account_balance_wallet_outlined,
          title: (p?.can('VIEW_AGENT_TOTAL_BALANCE') ?? false)
              ? 'الأرصدة'
              : 'الملخّص المالي',
          subtitle: (p?.can('VIEW_AGENT_TOTAL_BALANCE') ?? false)
              ? ((p?.can('VIEW_FINANCIAL_SUMMARY') ?? false)
                  ? 'رصيد الوكيل والملخّص اليومي'
                  : 'رصيد الوكيل')
              : 'ملخّص يومك',
          onTap: () => context.push('/employee/balances'),
        ),
    ];

    final tiles = <Widget>[
      ...head,
      for (final r in allowed)
        _Tile(
          def: r,
          onTap: () => Navigator.of(context, rootNavigator: true)
              .push(MaterialPageRoute(
            builder: (_) => EmployeeReportView(def: r),
          )),
        ),
    ];

    // ⚠ `_Msg` قائمةٌ بذاتها لا عنصرٌ داخل قائمة: لفُّها في `ListView` ثانية
    // يضع مِنظاراً رأسياً داخل مِنظارٍ رأسيّ بلا ارتفاعٍ محدّد، فتسقط
    // الشاشةُ بـ«Vertical viewport was given unbounded height».
    final body = tiles.isEmpty
        ? const _Msg(
            icon: Icons.lock_outline_rounded,
            text: 'لا تقارير ممنوحة لك.\n\n'
                'يمنحها الوكيل من شاشة الصلاحيات.',
          )
        : ListView.separated(
            // ⚠ 110 أسفلَ القائمة حين تكون تبويباً: الشريطُ السفليّ يعلو
            // المحتوى، وبدونها يبقى آخرُ تقريرٍ تحته فلا يُنقر.
            padding: EdgeInsets.fromLTRB(
                R.padScreen, 14, R.padScreen, asTab ? 110 : 30),
            itemCount: tiles.length,
            separatorBuilder: (_, _) => const SizedBox(height: R.gapRow),
            itemBuilder: (_, i) => tiles[i],
          );

    return Screen(
      child: Column(
        children: [
          if (asTab && p != null)
            EmployeeHeader(profile: p)
          else
            RhallaAppBar(title: 'التقارير', onBack: () => context.pop()),
          if (asTab) const SizedBox(height: 6),
          Expanded(child: body),
        ],
      ),
    );
  }
}

class ReportDef {
  const ReportDef({
    required this.key,
    required this.title,
    required this.subtitle,
    required this.icon,
    required this.path,
    this.isBalance = false,
  });

  final String key;
  final String title;
  final String subtitle;
  final IconData icon;
  final String path;

  /// تقريرُ الرصيد يعود بشكلٍ آخر — قائمةُ محافظ لا مجاميع وسطور.
  final bool isBalance;
}

/// عرضُ تقريرٍ واحد.
///
/// ⚠ **عرضٌ عامّ لا سبعُ شاشات.** التقاريرُ تشترك في شكلها: مجاميعُ فوق
/// وسطورٌ تحتها. وسبعُ شاشاتٍ متشابهة تفترق عن بعضها عند أوّل تعديل، ثمّ
/// يصير لكلّ تقريرٍ مظهرٌ مختلف بلا سبب.
class EmployeeReportView extends ConsumerWidget {
  const EmployeeReportView({super.key, required this.def});

  final ReportDef def;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final async = ref.watch(_reportProvider(def.path));

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(title: def.title, onBack: () => context.pop()),
          Expanded(
            child: RefreshIndicator(
              onRefresh: () async => ref.invalidate(_reportProvider(def.path)),
              color: R.primary,
              backgroundColor: Colors.white,
              child: async.when(
                loading: () => const Center(child: CircularProgressIndicator()),
                error: (e, _) => _Msg(
                  icon: Icons.wifi_off_rounded,
                  text: 'تعذّر تحميل التقرير.\n$e',
                ),
                data: (d) => _body(d),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _body(Map<String, dynamic> d) {
    // تقريرُ الرصيد: قائمةُ محافظ.
    if (def.isBalance) {
      final rows = (d['_rows'] as List?) ?? const [];
      return ListView(
        padding: const EdgeInsets.fromLTRB(R.padScreen, 16, R.padScreen, 30),
        children: [
          if (rows.isEmpty)
            const _Msg(icon: Icons.wallet_outlined, text: 'لا رصيد.')
          else
            ...rows.map((r) {
              final m = (r as Map).cast<String, dynamic>();
              return GlassCard(
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Text('الرصيد الحالي', style: T.kufi(14, FontWeight.w700)),
                    Directionality(
                      textDirection: TextDirection.ltr,
                      child: Row(
                        children: [
                          Text('د.ل',
                              style: T.kufi(13, FontWeight.w700,
                                  color: R.primaryDark)),
                          const SizedBox(width: 6),
                          Text(Fmt.money(Fmt.num_(m['Walet'])),
                              style: T.kufi(20, FontWeight.w800)),
                        ],
                      ),
                    ),
                  ],
                ),
              );
            }),
        ],
      );
    }

    final items = (d['items'] as List?) ?? const [];

    return ListView(
      padding: const EdgeInsets.fromLTRB(R.padScreen, 16, R.padScreen, 30),
      children: [
        _Summary(d: d),
        const SizedBox(height: R.gapCard),
        if (items.isEmpty)
          const _Msg(icon: Icons.inbox_rounded, text: 'لا حركات في هذه المدة.')
        else
          ...items.map((e) => Padding(
                padding: const EdgeInsets.only(bottom: 8),
                child: _ItemRow(m: (e as Map).cast<String, dynamic>()),
              )),
      ],
    );
  }
}

/// ⚠ `family` على المسار لا مزوّدٌ لكل تقرير: سبعةُ مزوّداتٍ متطابقة تختلف
/// في سطرٍ واحد هي سبعةُ مواضع تُنسى إحداها عند التعديل.
final _reportProvider =
    FutureProvider.autoDispose.family<Map<String, dynamic>, String>(
        (ref, path) async {
  final env = await ref.watch(apiClientProvider).get(path);

  final row = env.row;
  if (row != null) return row;

  // ⚠ تقريرُ الرصيد يعود قائمةً لا كائناً — يُغلَّف ليمرّ بالشكل نفسِه.
  return {'_rows': env.rows};
});

class _Summary extends StatelessWidget {
  const _Summary({required this.d});
  final Map<String, dynamic> d;

  @override
  Widget build(BuildContext context) {
    final cells = <(String, String)>[];

    if (d.containsKey('count')) {
      cells.add(('العدد', Fmt.count(int.tryParse('${d['count']}') ?? 0)));
    }
    if (d.containsKey('total')) {
      cells.add(('الإجمالي', Fmt.money(Fmt.num_(d['total']))));
    }
    if (d.containsKey('in')) {
      cells.add(('الداخل', Fmt.money(Fmt.num_(d['in']))));
    }
    if (d.containsKey('out')) {
      cells.add(('الخارج', Fmt.money(Fmt.num_(d['out']))));
    }
    if (d.containsKey('net')) {
      cells.add(('الصافي', Fmt.money(Fmt.num_(d['net']))));
    }

    if (cells.isEmpty) return const SizedBox.shrink();

    return GlassCard(
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceAround,
        children: [
          for (final (label, value) in cells)
            Column(
              children: [
                Text(label,
                    style: T.plex(11.5, FontWeight.w600, color: R.inkA(.55))),
                const SizedBox(height: 4),
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text(value,
                      style: T.kufi(15, FontWeight.w800, color: R.ink)),
                ),
              ],
            ),
        ],
      ),
    );
  }
}

class _ItemRow extends StatelessWidget {
  const _ItemRow({required this.m});
  final Map<String, dynamic> m;

  @override
  Widget build(BuildContext context) {
    final code = '${m['transfer_number'] ?? ''}';
    final name = '${m['receiver_name'] ?? m['employee_name'] ?? ''}'.trim();
    final action = '${m['action'] ?? ''}';
    final auditAction = '${m['action'] ?? ''}';
    final amount = m.containsKey('amount') ? Fmt.num_(m['amount']) : null;

    // سجلُّ النشاط لا مبلغَ فيه ولا رقمَ حوالة — يُعرض بحدثه.
    final isAudit = m.containsKey('entity_type');

    return Container(
      padding: const EdgeInsets.all(12),
      decoration: BoxDecoration(
        color: R.whiteA(.55),
        border: Border.all(color: R.inkA(.08)),
        borderRadius: BorderRadius.circular(R.rCard),
      ),
      child: Row(
        children: [
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  isAudit
                      ? _auditLabel(auditAction)
                      : (name.isNotEmpty ? name : 'حوالة'),
                  style: T.kufi(13, FontWeight.w700),
                ),
                const SizedBox(height: 3),
                if (code.isNotEmpty && code != 'null')
                  Directionality(
                    textDirection: TextDirection.ltr,
                    child: Text(code,
                        style: T.plex(11, FontWeight.w400, color: R.inkA(.5))),
                  ),
                if (m['note'] != null && '${m['note']}'.trim().isNotEmpty)
                  Text('${m['note']}',
                      style:
                          T.plex(11, FontWeight.w400, color: R.inkA(.5))),
                if (action == 'DELIVERED')
                  Text('تسليم',
                      style: T.plex(11, FontWeight.w600, color: R.error)),
              ],
            ),
          ),
          Column(
            crossAxisAlignment: CrossAxisAlignment.end,
            children: [
              if (amount != null && amount != 0)
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text(Fmt.money(amount),
                      style: T.kufi(14, FontWeight.w800, color: R.ink)),
                ),
              const SizedBox(height: 2),
              Text(Fmt.stampShort('${m['at'] ?? ''}'),
                  style: T.plex(10.5, FontWeight.w400, color: R.inkA(.45))),
            ],
          ),
        ],
      ),
    );
  }

  /// أحداثُ التدقيق بأسماء يفهمها الموظف لا بمفاتيحها.
  static String _auditLabel(String a) => switch (a) {
        'EMPLOYEE_CREATED_TRANSFER' => 'أنشأتُ حوالة',
        'EMPLOYEE_DELIVERED_TRANSFER' => 'سجّلتُ تسليم حوالة',
        'SHIFT_STARTED' => 'بدأتُ وردية',
        'SHIFT_CLOSED' => 'أقفلتُ وردية',
        'CASHBOX_ENTRY' => 'سجّلتُ حركة خزينة',
        'EMPLOYEE_ACTIVATED' => 'فعّلتُ الجهاز',
        'EMPLOYEE_LOGOUT' => 'خرجتُ من التطبيق',
        'EMPLOYEE_APPROVAL_REQUESTED' => 'طلبتُ موافقة الوكيل',
        'EMPLOYEE_APPROVAL_CANCELLED' => 'سحبتُ طلب موافقة',
        _ => a,
      };
}

class _Tile extends StatelessWidget {
  const _Tile({required this.def, required this.onTap});

  final ReportDef def;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => Material(
        color: Colors.transparent,
        child: InkWell(
          onTap: onTap,
          borderRadius: BorderRadius.circular(R.rCard),
          child: GlassCard(
            child: Row(
              children: [
                Container(
                  width: 40,
                  height: 40,
                  decoration: BoxDecoration(
                    color: R.primaryA(.10),
                    borderRadius: BorderRadius.circular(12),
                  ),
                  child: Icon(def.icon, size: 20, color: R.primaryDark),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(def.title, style: T.kufi(14, FontWeight.w700)),
                      const SizedBox(height: 2),
                      Text(def.subtitle,
                          style: T.plex(11.5, FontWeight.w400,
                              color: R.inkA(.55))),
                    ],
                  ),
                ),
                Icon(Icons.chevron_left_rounded, size: 22, color: R.inkA(.35)),
              ],
            ),
          ),
        ),
      );
}

class _Msg extends StatelessWidget {
  const _Msg({required this.icon, required this.text});
  final IconData icon;
  final String text;

  @override
  Widget build(BuildContext context) => ListView(
        padding: const EdgeInsets.fromLTRB(30, 70, 30, 30),
        children: [
          Icon(icon, size: 42, color: R.inkA(.24)),
          const SizedBox(height: 14),
          Text(text,
              textAlign: TextAlign.center,
              style: T.plex(13, FontWeight.w500,
                  color: R.inkA(.5), height: 1.8)),
        ],
      );
}
