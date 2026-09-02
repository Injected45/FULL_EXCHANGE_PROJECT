import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/glass.dart';
import 'send_layout.dart';
import '../home/home_repository.dart';

/// عناصر ملخّص الحوالة المشتركة بين شاشة التأكيد وشاشة النجاح.
///
/// وُضعت في ملفٍّ واحد عمداً: الشاشتان تعرضان المال نفسه، ونسختان من صفّ
/// مبلغٍ تفترقان يوماً ما — فتقرأ إحداهما رقماً بصيغةٍ والأخرى بأخرى.

/// رصيد الوكيل وحده — لا لقطة الرئيسية كاملة.
///
/// `homeSnapshotProvider` يجلب السقوف والحوالات والرصيد في ثلاثة نداءات؛
/// هاتان الشاشتان تحتاجان الرصيد فقط، ونداءٌ واحد أسرع في لحظةٍ الوكيل فيها
/// على وشك تحويل مال أو خارجٌ لتوّه منه.
final balanceProvider = FutureProvider.autoDispose<double>(
  (ref) => ref.watch(homeRepositoryProvider).balance(),
);

/// مبلغ برمز العملة على يساره دائماً — قرار المالك، مطبَّق في كل شاشة.
///
/// الصفّ كلّه ltr لأن الرقم مقطعٌ لاتيني؛ لولا ذلك أعادت الفقرة العربية
/// ترتيب الرمز والرقم. ولهذا يأتي رمز العملة **أولاً** في children.
class MoneyText extends StatelessWidget {
  const MoneyText(this.value,
      {super.key,
      required this.currency,
      this.size = 15,
      this.weight = FontWeight.w700,
      this.color});

  final num value;
  final String currency;
  final double size;
  final FontWeight weight;
  final Color? color;

  @override
  Widget build(BuildContext context) => Directionality(
        textDirection: TextDirection.ltr,
        child: Row(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.baseline,
          textBaseline: TextBaseline.alphabetic,
          children: [
            Text(currency,
                style: T.plex(size * .58 + 4, FontWeight.w400,
                    color: color ?? R.inkA(.5))),
            const SizedBox(width: 8),
            Text(Fmt.money(value), style: T.kufi(size, weight, color: color)),
          ],
        ),
      );
}

/// صفّ «تسمية — مبلغ» داخل الصندوق الكهرمانيّ.
class MoneyRow extends StatelessWidget {
  const MoneyRow(this.k, this.v,
      {super.key, required this.currency, this.strong = false});

  final String k;
  final num v;
  final String currency;
  final bool strong;

  @override
  Widget build(BuildContext context) => Row(
        children: [
          Expanded(
            child: Text(k,
                style: T.plex(strong ? 12.5 : 12,
                    strong ? FontWeight.w600 : FontWeight.w400,
                    color: R.warnInk.withValues(alpha: strong ? 1 : .8))),
          ),
          const SizedBox(width: 10),
          MoneyText(v,
              currency: currency,
              size: strong ? 18 : 14,
              color: R.warnInk),
        ],
      );
}

/// صندوق المبالغ — كهرمانيّ لأنه ما خُصم فعلاً، فهو محلّ نظر الوكيل.
class TotalsBox extends StatelessWidget {
  const TotalsBox({
    super.key,
    required this.amount,
    required this.commission,
    required this.currency,
    this.totalLabel = 'الإجمالي المخصوم من رصيدك',
  });

  final double amount;
  final double commission;
  final String currency;
  final String totalLabel;

  @override
  Widget build(BuildContext context) => Container(
        padding: kCardPad,
        decoration: BoxDecoration(
          color: R.warnBg,
          borderRadius: BorderRadius.circular(R.rCard),
          border: Border.all(color: R.warnBorder),
        ),
        child: Column(
          children: [
            MoneyRow('قيمة الحوالة', amount, currency: currency),
            const SizedBox(height: kGap),
            MoneyRow('عمولة التحويل', commission, currency: currency),
            const SizedBox(height: kGapRule),
            Divider(color: R.warnInk.withValues(alpha: .18), height: 1),
            const SizedBox(height: kGapRule),
            MoneyRow(totalLabel, amount + commission,
                currency: currency, strong: true),
          ],
        ),
      );
}

/// رصيد الوكيل، مع إخفاءٍ اختياري.
///
/// الإخفاء مقصود: الوكيل يفتح هذه الشاشة والزبون واقفٌ أمامه، ورصيد
/// الوكالة ليس من شأن الزبون.
class BalanceCard extends ConsumerStatefulWidget {
  const BalanceCard({super.key, required this.currency, this.label});

  final String currency;
  final String? label;

  @override
  ConsumerState<BalanceCard> createState() => _BalanceCardState();
}

class _BalanceCardState extends ConsumerState<BalanceCard> {
  bool _hidden = false;

  @override
  Widget build(BuildContext context) {
    final async = ref.watch(balanceProvider);

    return GlassCard(
      large: true,
      sheen: true,
      padding: const EdgeInsets.fromLTRB(22, 18, 14, 22),
      child: Column(
        children: [
          Row(
            children: [
              Text(widget.label ?? 'رصيدك الحالي', style: T.label),
              const Spacer(),
              IconButton(
                onPressed: () => setState(() => _hidden = !_hidden),
                iconSize: 19,
                constraints:
                    const BoxConstraints.tightFor(width: 44, height: 44),
                padding: EdgeInsets.zero,
                color: R.inkA(.45),
                tooltip: _hidden ? 'إظهار الرصيد' : 'إخفاء الرصيد',
                icon: Icon(_hidden
                    ? Icons.visibility_off_outlined
                    : Icons.visibility_outlined),
              ),
            ],
          ),
          const SizedBox(height: 6),
          async.when(
            loading: () => Container(
              width: 150,
              height: 32,
              decoration: BoxDecoration(
                color: R.inkA(.06),
                borderRadius: BorderRadius.circular(10),
              ),
            ),
            // تعذّر جلب الرصيد لا يمنع شيئاً — الخادم هو من يرفض عند عدم
            // الكفاية، لا هذه البطاقة.
            error: (_, _) => Text('تعذّر جلب الرصيد',
                style: T.plex(12.5, FontWeight.w500, color: R.inkA(.5))),
            data: (v) => _hidden
                ? Text('••••••', style: T.kufi(30, FontWeight.w800))
                : MoneyText(v,
                    currency: widget.currency,
                    size: 32,
                    weight: FontWeight.w800),
          ),
        ],
      ),
    );
  }
}

/// هاتف المستفيد — رقمٌ لاتينيّ داخل صفٍّ عربيّ، فيُلَفّ بـ ltr وحده.
class PhoneRow extends StatelessWidget {
  const PhoneRow(this.phone, {super.key, this.label = 'هاتف المستلم'});

  final String phone;
  final String label;

  @override
  Widget build(BuildContext context) => Row(
        children: [
          Text(label, style: T.plex(12, FontWeight.w400, color: R.inkA(.55))),
          const Spacer(),
          Directionality(
            textDirection: TextDirection.ltr,
            child: Text('+218 ${Fmt.phone(phone)}',
                style: T.kufi(14, FontWeight.w700)),
          ),
        ],
      );
}
