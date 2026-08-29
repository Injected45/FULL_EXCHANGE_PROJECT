import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/ambient.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../auth/auth_controller.dart';
import 'send_repository.dart';

/// شاشة النجاح — الرمز هو المنتج الحقيقي للعملية، فهو أكبر عنصر فيها.
class TransferDoneScreen extends ConsumerWidget {
  const TransferDoneScreen({super.key, required this.transfer});

  final CreatedTransfer transfer;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final currency =
        ref.watch(authControllerProvider).user?.currencyCode ?? 'د.ل';

    return PopScope(
      // العملية تمّت — لا رجوع إلى المراجعة.
      canPop: false,
      child: Screen(
        child: ListView(
          padding: const EdgeInsets.fromLTRB(26, 0, 26, 30),
          children: [
            const SizedBox(height: 60),
            Center(
              child: SizedBox(
                width: 132,
                height: 132,
                child: Stack(
                  alignment: Alignment.center,
                  children: [
                    const Positioned.fill(child: PulseRing(seconds: 2.4)),
                    Container(
                      width: 96,
                      height: 96,
                      decoration: BoxDecoration(
                        shape: BoxShape.circle,
                        gradient: R.primaryGradient,
                        boxShadow: [
                          BoxShadow(
                            color: R.primaryA(.36),
                            blurRadius: 44,
                            offset: const Offset(0, 22),
                          )
                        ],
                      ),
                      child: const Icon(Icons.check_rounded,
                          size: 44, color: Colors.white),
                    ),
                  ],
                ),
              ),
            ),
            const SizedBox(height: 28),
            RiseIn(
              delay: const Duration(milliseconds: 150),
              child: Column(
                children: [
                  Text('تمّت الحوالة',
                      textAlign: TextAlign.center, style: T.titleSm),
                  const SizedBox(height: 10),
                  Text('أرسل الرمز للمستفيد ليستلمها من أي فرع.',
                      textAlign: TextAlign.center,
                      style: T.plex(13, FontWeight.w400,
                          color: R.inkA(.58), height: 1.8)),
                ],
              ),
            ),
            const SizedBox(height: 26),
            RiseIn(
              delay: const Duration(milliseconds: 250),
              child: GlassCard(
                large: true,
                sheen: true,
                padding: const EdgeInsets.symmetric(horizontal: 22, vertical: 22),
                child: Column(
                  children: [
                    Text('رمز الحوالة', style: T.label),
                    const SizedBox(height: 14),
                    Directionality(
                      textDirection: TextDirection.ltr,
                      child: SelectableText(
                        transfer.shareCode,
                        textAlign: TextAlign.center,
                        style: T.kufi(34, FontWeight.w800, spacing: 3.5),
                      ),
                    ),
                    const SizedBox(height: 12),
                    _CopyButton(code: transfer.shareCode),
                    const SizedBox(height: 18),
                    Divider(color: R.inkA(.07), height: 1),
                    const SizedBox(height: 14),
                    _Kv('المستفيد', transfer.receiverName),
                    const SizedBox(height: 12),
                    _Kv('المبلغ', '${Fmt.money(transfer.amount)} $currency',
                        numeric: true),
                    if (transfer.commission > 0) ...[
                      const SizedBox(height: 12),
                      _Kv('العمولة',
                          '${Fmt.money(transfer.commission)} $currency',
                          numeric: true),
                    ],
                  ],
                ),
              ),
            ),
            const SizedBox(height: 22),
            RiseIn(
              delay: const Duration(milliseconds: 350),
              child: Column(
                children: [
                  PrimaryButton(
                    label: 'حوالة جديدة',
                    onPressed: () =>
                        context.pushReplacement('/send/internal'),
                  ),
                  const SizedBox(height: 10),
                  GlassButton(
                    label: 'العودة إلى الرئيسية',
                    onPressed: () => context.go('/'),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}

class _CopyButton extends StatefulWidget {
  const _CopyButton({required this.code});

  final String code;

  @override
  State<_CopyButton> createState() => _CopyButtonState();
}

class _CopyButtonState extends State<_CopyButton> {
  bool _copied = false;

  @override
  Widget build(BuildContext context) => TextButton.icon(
        onPressed: () async {
          await Clipboard.setData(ClipboardData(text: widget.code));
          if (!mounted) return;
          setState(() => _copied = true);
          await Future.delayed(const Duration(seconds: 2));
          if (mounted) setState(() => _copied = false);
        },
        style: TextButton.styleFrom(minimumSize: const Size(44, 44)),
        icon: Icon(_copied ? Icons.check_rounded : Icons.copy_rounded,
            size: 16, color: R.primaryGradEnd),
        label: Text(_copied ? 'نُسخ' : 'نسخ الرمز',
            style: T.plex(12.5, FontWeight.w600, color: R.primaryGradEnd)),
      );
}

class _Kv extends StatelessWidget {
  const _Kv(this.k, this.v, {this.numeric = false});

  final String k;
  final String v;
  final bool numeric;

  @override
  Widget build(BuildContext context) => Row(
        children: [
          Text(k, style: T.plex(12, FontWeight.w400, color: R.inkA(.55))),
          const Spacer(),
          numeric
              ? Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text(v, style: T.kufi(14, FontWeight.w700)),
                )
              : Text(v, style: T.plex(13.5, FontWeight.w600)),
        ],
      );
}
