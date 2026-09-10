import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:url_launcher/url_launcher.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../transfers/receipt.dart';
import 'employee_header.dart';

/// فاتورةُ حوالةٍ **خارجيةٍ صادرة**.
///
/// ══════════════════════════════════════════════════════════════════════════
///  أمرُ المالك (11 سبتمبر 2026)
/// ══════════════════════════════════════════════════════════════════════════
///
/// «عند الضغط على حوالة خارجية صادرة أريد أن تظهر فاتورةٌ بعرض بيانات الحوالة
///  **بنفس آليّة وطريقة عرض الحوالة الداخلية بالضبط**، مع اختلاف عرض البيانات».
///
/// ⚠ ولذلك لم تُبنَ ورقةٌ جديدة: هذه هي أجزاءُ الفاتورة الداخلية نفسُها —
/// [ReceiptHeader] و[ReceiptRow] و[BoxedField] و[MiniButton]، والطباعةُ
/// والمشاركةُ بـ[ReceiptTools] بتصويرِ `RepaintBoundary` كما هي هناك. فما
/// يتغيّر **البياناتُ المعروضة وحدَها**، وهو نصُّ الأمر حرفاً.
///
/// ── وما الذي يختلف في البيانات، ولماذا ──────────────────────────────────
///
/// الحوالةُ المحلّية مبلغٌ واحدٌ بعملةٍ واحدة يُسلَّم في فرع. والخارجيةُ تعبر
/// عملتين: يُقبض بالدينار، ويُسلَّم بعملة الوجهة بسعرٍ يحسبه المحفّز. فتُعرض
/// هنا **ثلاثةُ أرقامٍ لا رقم**: المقبوض · السعر · ما يُستلم هناك.
///
/// ⚠ وكلُّها **من الخادم كما كتبها المحفّزُ بعد الإدراج** — لا يُضرب رقمٌ في
/// رقمٍ في الهاتف. حسابٌ ثانٍ هنا يفترق عن الأوّل عند أوّل كسر، ثمّ يحمل
/// الزبونُ ورقةً تقول غيرَ ما في الدفتر.
///
/// ⚠ **ولا حالةَ تُختلَق.** `ExternalEx` لا جدولَ حالاتٍ لها نظيرَ
/// `InternalEx_Stautes`، فتُقرأ حالتُها من أعلامها الثلاثة (ملغاة · مسلَّمة ·
/// معتمدة) ولا يُكتب وصفٌ لا تقوله القاعدة: الموظف يبني على هذا السطر كلامَه
/// للزبون.
class EmployeeExternalReceiptScreen extends ConsumerStatefulWidget {
  const EmployeeExternalReceiptScreen({super.key, required this.code});

  /// رقمُ الحوالة كما تعرفه المنظومة — `13152-55-6`.
  final String code;

  @override
  ConsumerState<EmployeeExternalReceiptScreen> createState() =>
      _EmployeeExternalReceiptScreenState();
}

class _EmployeeExternalReceiptScreenState
    extends ConsumerState<EmployeeExternalReceiptScreen>
    with ReceiptTools<EmployeeExternalReceiptScreen> {
  Future<void> _print() => printReceipt(name: widget.code);

  Future<void> _share(ExternalReceipt r) => shareReceipt(
        name: widget.code,
        text: 'حوالة خارجية ${r.code} — ${r.beneficiaryName}',
      );

  Future<void> _call(String phone) async {
    final p = phone.replaceAll(RegExp(r'\D'), '');
    if (p.isEmpty) return;
    /*
     * ⚠ بلا بادئة `+218`: المستفيدُ **خارج ليبيا**، ورقمُه برمز بلده. وإلصاقُ
     * رمز ليبيا برقمٍ مصريّ يُنتج رقماً لا وجودَ له — وهي البادئةُ التي
     * تضعها فاتورةُ الحوالة المحلّية بحقّ، لأنّ مستلمَها ليبيّ.
     */
    if (!await launchUrl(Uri.parse('tel:$p'))) {
      if (mounted) receiptToast('تعذّر فتح تطبيق الاتصال.');
    }
  }

  void _copy(String phone) {
    Clipboard.setData(ClipboardData(text: phone));
    receiptToast('نُسخ رقم المستفيد');
  }

  @override
  Widget build(BuildContext context) {
    final async = ref.watch(externalReceiptProvider(widget.code));

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'فاتورة حوالة خارجية صادرة',
            onBack: () => Navigator.of(context).pop(),
            trailing: async.valueOrNull == null
                ? null
                : IconButton(
                    tooltip: 'طباعة',
                    onPressed: receiptBusy ? null : _print,
                    icon: Icon(Icons.print_outlined,
                        size: 22, color: R.primaryDark),
                    constraints:
                        const BoxConstraints(minWidth: 44, minHeight: 44),
                  ),
          ),
          Expanded(
            child: async.when(
              loading: () =>
                  Center(child: CircularProgressIndicator(color: R.primary)),
              error: (e, _) => EmployeeEmpty(
                icon: Icons.wifi_off_rounded,
                text: 'تعذّر تحميل الفاتورة.\n$e',
              ),
              data: (r) => ListView(
                padding:
                    const EdgeInsets.fromLTRB(R.padScreen, 18, R.padScreen, 30),
                children: [
                  RepaintBoundary(
                    key: receiptKey,
                    child: _ExternalInvoice(
                      r: r,
                      onCall: () => _call(r.beneficiaryPhone),
                      onCopy: () => _copy(r.beneficiaryPhone),
                    ),
                  ),
                  const SizedBox(height: 20),
                  PrimaryButton(
                    label: 'مشاركة الفاتورة',
                    loading: receiptBusy,
                    icon: const Icon(Icons.share_rounded,
                        size: 18, color: Colors.white),
                    onPressed: receiptBusy ? null : () => _share(r),
                  ),
                  const SizedBox(height: 10),
                  GlassButton(
                    label: 'طباعة',
                    onPressed: receiptBusy ? null : _print,
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }
}

/* ───────────────── الورقة ───────────────── */

/// ورقةُ الفاتورة — نفسُ حاوية الفاتورة الداخلية وأجزائها.
class _ExternalInvoice extends StatelessWidget {
  const _ExternalInvoice({
    required this.r,
    required this.onCall,
    required this.onCopy,
  });

  final ExternalReceipt r;
  final VoidCallback onCall;
  final VoidCallback onCopy;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(18, 20, 18, 20),
        decoration: BoxDecoration(
          // ⚠ مصمَتةٌ لا شفّافة: الورقةُ **تُصوَّر** للطباعة والمشاركة،
          // وشفافيةٌ هنا تلتقط ما خلفها فتخرج على الورق بخلفيةٍ متّسخة.
          // القاعدةُ نفسُها في الفاتورة الداخلية.
          color: r.isCancelled ? null : Colors.white,
          gradient: r.isCancelled
              ? const LinearGradient(
                  begin: Alignment.topRight,
                  end: Alignment.bottomLeft,
                  colors: [
                    Color(0xFFFFF7F7),
                    Color(0xFFFDEEEE),
                    Color(0xFFFFF9F9),
                  ],
                  stops: [0, .55, 1],
                )
              : null,
          borderRadius: BorderRadius.circular(R.rCardXl),
          border: Border.all(
            color: r.isCancelled ? R.error.withValues(alpha: .16) : R.inkA(.06),
          ),
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            // ترويسةُ الشركة نفسُها — كلُّ ورقةٍ يخرج بها الموظف باسم شركته.
            const ReceiptHeader(),
            const SizedBox(height: 20),

            ReceiptRow('تاريخ ووقت التحويل',
                Fmt.stamp(r.at, separator: '  '), ltr: true),
            ReceiptRow('رقم الكود', r.code, ltr: true, strong: true),
            if (r.senderName.isNotEmpty)
              ReceiptRow('اسم المرسل', r.senderName, strong: true),
            if (r.country.isNotEmpty) ReceiptRow('الدولة', r.country),
            if (r.city.isNotEmpty) ReceiptRow('المدينة', r.city),
            // ⚠ نوعُ الخدمة سطرٌ في الخارجية وحدَها: هو الذي يحدّد **كيف**
            // يستلم المستفيد — بريدٌ أو بنكٌ أو نقداً — والزبون يُسأل عنه.
            if (r.service.isNotEmpty) ReceiptRow('نوع الخدمة', r.service),
            if (r.branch.isNotEmpty) ReceiptRow('فرع المرسل', r.branch),

            const SizedBox(height: 12),
            BoxedField(
              icon: Icons.person_outline_rounded,
              label: 'اسم المستفيد',
              child: Text(
                r.beneficiaryName.isEmpty ? '—' : r.beneficiaryName,
                textAlign: TextAlign.start,
                style: T.kufi(17, FontWeight.w700),
              ),
            ),
            const SizedBox(height: 10),
            BoxedField(
              icon: Icons.phone_outlined,
              label: 'هاتف المستفيد',
              child: Row(
                children: [
                  Directionality(
                    // رقمٌ لاتينيّ في فقرةٍ عربية — يُفرض اتجاهه، وإلّا قلبته
                    // الفقرةُ فقرأه الموظف معكوساً واتّصل بغير المستفيد.
                    textDirection: TextDirection.ltr,
                    child: Text(
                      r.beneficiaryPhone.isEmpty ? '—' : r.beneficiaryPhone,
                      style: T.kufi(16, FontWeight.w700),
                    ),
                  ),
                  const Spacer(),
                  if (r.beneficiaryPhone.isNotEmpty) ...[
                    MiniButton(
                        label: 'نسخ',
                        icon: Icons.copy_rounded,
                        onTap: onCopy),
                    const SizedBox(width: 8),
                    MiniButton(
                        label: 'اتصال',
                        icon: Icons.call_rounded,
                        filled: true,
                        onTap: onCall),
                  ],
                ],
              ),
            ),

            const SizedBox(height: 14),
            Divider(color: R.inkA(.07), height: 1),
            const SizedBox(height: 14),

            /*
             * ⚠ ثلاثةُ أرقامٍ لا رقمٌ واحد — وهو **اختلافُ عرض البيانات**
             * الذي نصّ عليه الأمر:
             *
             *   • المقبوض بالدينار — ما دفعه المرسل هنا.
             *   • سعرُ الصرف — كما كتبه المحفّز بعد الإدراج.
             *   • ما يُستلم بعملة الوجهة — وهو الرقمُ الذي يسأل عنه المستفيد.
             *
             * ولا يُضرب رقمٌ في رقمٍ هنا: الثلاثةُ من الخادم، فورقةُ الزبون
             * لا تخالف الدفتر بكسرٍ واحد.
             */
            if (r.amount != null)
              ReceiptRow('المبلغ المقبوض', Fmt.money(r.amount!),
                  ltr: true, strong: true, currency: 'د.ل'),
            if (r.rate != null)
              ReceiptRow('سعر الصرف', Fmt.rate(r.rate!), ltr: true),
            if (r.netTotal != null)
              ReceiptRow('المبلغ المستلَم', Fmt.money(r.netTotal!),
                  ltr: true,
                  strong: true,
                  currency: r.currencyCode.isEmpty ? null : r.currencyCode),

            const SizedBox(height: 10),
            Row(
              children: [
                Text('حالة الحوالة',
                    style: T.plex(12, FontWeight.w400, color: R.inkA(.55))),
                const SizedBox(width: 10),
                Expanded(
                  child: Align(
                    alignment: AlignmentDirectional.centerEnd,
                    child: Container(
                      padding: const EdgeInsets.symmetric(
                          horizontal: 12, vertical: 6),
                      decoration: BoxDecoration(
                        color: r.statusColor.withValues(alpha: .10),
                        border: Border.all(
                            color: r.statusColor.withValues(alpha: .32)),
                        borderRadius: BorderRadius.circular(99),
                      ),
                      child: Text(
                        r.statusLabel,
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                        style: T.kufi(12.5, FontWeight.w700,
                            color: r.statusColor),
                      ),
                    ),
                  ),
                ),
              ],
            ),

            // ملاحظةٌ كُتبت على الحوالة — وإن غابت فلا يظهر شيء: أغلبُ
            // الحوالات بلا ملاحظة، وحقلٌ فارغ في كلّ فاتورة يوحي بنصٍّ لم يصل.
            if (r.notes.isNotEmpty) ...[
              const SizedBox(height: 14),
              BoxedField(
                icon: Icons.sticky_note_2_outlined,
                label: 'ملاحظة',
                child: Text(r.notes,
                    textAlign: TextAlign.start,
                    style: T.kufi(14, FontWeight.w500, height: 1.55)),
              ),
            ],
          ],
        ),
      );
}

/* ───────────────── الطراز والمزوّد ───────────────── */

class ExternalReceipt {
  const ExternalReceipt({
    required this.code,
    required this.senderName,
    required this.beneficiaryName,
    required this.beneficiaryPhone,
    required this.country,
    required this.city,
    required this.service,
    required this.branch,
    required this.amount,
    required this.commission,
    required this.rate,
    required this.netTotal,
    required this.currencyCode,
    required this.notes,
    required this.isCancelled,
    required this.isDelivered,
    required this.confirmedType,
    required this.at,
  });

  final String code;
  final String senderName;
  final String beneficiaryName;
  final String beneficiaryPhone;
  final String country;
  final String city;
  final String service;
  final String branch;

  /// المقبوضُ بالدينار.
  final double? amount;
  final double? commission;

  /// سعرُ الصرف كما كتبه المحفّز.
  final double? rate;

  /// ما يُستلم بعملة الوجهة.
  final double? netTotal;
  final String currencyCode;
  final String notes;

  final bool isCancelled;
  final bool isDelivered;
  final int? confirmedType;
  final String at;

  /*
   * ⚠ الحالةُ تُشتقّ من أعلام القاعدة الثلاثة، ولا تُختلَق.
   *
   * `ExternalEx` لا جدولَ حالاتٍ لها نظيرَ `InternalEx_Stautes` — والصفُّ
   * يحمل `IsCanceled` و`IsDelivered` و`ConfirmedType` وحدَها. فيُقال ما تقوله
   * هذه الثلاثة لا أكثر: «قيد التنفيذ» عن حوالةٍ لا تُعرف حالتُها ادّعاءٌ
   * يبني عليه الموظف كلامَه للزبون.
   */
  String get statusLabel {
    if (isCancelled) return 'ملغاة';
    if (isDelivered) return 'مسلَّمة';
    return 'قيد المعالجة';
  }

  Color get statusColor {
    if (isCancelled) return R.error;
    if (isDelivered) return R.primaryGradEnd;
    return R.primaryDark;
  }

  static String _s(dynamic v) => '${v ?? ''}'.trim();

  static ExternalReceipt fromJson(Map<String, dynamic> j) => ExternalReceipt(
        code: _s(j['code']),
        senderName: _s(j['sender_name']),
        beneficiaryName: _s(j['beneficiary_name']),
        beneficiaryPhone: _s(j['beneficiary_phone']),
        country: _s(j['country']),
        city: _s(j['city']),
        service: _s(j['service']),
        branch: _s(j['branch']),
        amount: j['amount'] == null ? null : Fmt.num_(j['amount']),
        commission: j['commission'] == null ? null : Fmt.num_(j['commission']),
        rate: j['rate'] == null ? null : Fmt.num_(j['rate']),
        netTotal: j['net_total'] == null ? null : Fmt.num_(j['net_total']),
        currencyCode: _s(j['currency_code']),
        notes: _s(j['notes']),
        isCancelled: '${j['is_canceled'] ?? 0}' != '0',
        isDelivered: '${j['is_delivered'] ?? 0}' != '0',
        confirmedType: int.tryParse('${j['confirmed_type'] ?? ''}'),
        at: _s(j['at']),
      );
}

/// فاتورةُ حوالةٍ خارجيةٍ بالرقم — والخادمُ يحرس ملكيّتَها.
final externalReceiptProvider =
    FutureProvider.autoDispose.family<ExternalReceipt, String>(
        (ref, code) async {
  final env = await ref
      .watch(apiClientProvider)
      .get('/device/employee/external/mine/$code');
  return ExternalReceipt.fromJson(env.row ?? const {});
});
