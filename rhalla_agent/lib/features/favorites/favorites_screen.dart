import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/ambient.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../auth/auth_controller.dart';
import 'favorites_repository.dart';

/// رسالة نجاح/تنبيه بنفس شكل بقية التطبيق.
void showFavoriteToast(BuildContext context, String text,
    {SnackBarAction? action}) {
  ScaffoldMessenger.of(context)
    ..hideCurrentSnackBar()
    ..showSnackBar(SnackBar(
      content:
          Text(text, style: T.plex(13, FontWeight.w500, color: Colors.white)),
      backgroundColor: R.primaryGradEnd,
      behavior: SnackBarBehavior.floating,
      margin: const EdgeInsets.fromLTRB(16, 0, 16, 100),
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
      action: action,
    ));
}

/// زرّ اختياري في شاشة النجاح: يحفظ المستفيد في المفضّلة.
///
/// يختفي إن نقص الرمز أو الهاتف، لأن الخادم يشترط الاثنين ويرفض الطلب
/// بدونهما — وزرٌّ يفشل دائماً أسوأ من زرٍّ غائب.
class AddToFavoritesButton extends ConsumerStatefulWidget {
  const AddToFavoritesButton({
    super.key,
    required this.code,
    required this.kind,
    required this.name,
    required this.phone,
  });

  /// رمز الحوالة كما في عمود `Code` — لا رمز الموبايل.
  final String code;
  final FavoriteKind kind;
  final String name;
  final String phone;

  @override
  ConsumerState<AddToFavoritesButton> createState() =>
      _AddToFavoritesButtonState();
}

class _AddToFavoritesButtonState extends ConsumerState<AddToFavoritesButton> {
  bool _busy = false;
  bool _done = false;

  Future<void> _add() async {
    setState(() => _busy = true);
    try {
      await ref.read(favoritesRepositoryProvider).add(
            code: widget.code,
            kind: widget.kind,
            phone: widget.phone,
          );
      invalidateFavorites(ref);
      if (!mounted) return;
      setState(() {
        _busy = false;
        _done = true;
      });
      // 409 يعني أنه مضاف سلفاً — والنتيجة التي أرادها الوكيل متحقّقة،
      // فلا تُعرض كفشل.
      showFavoriteToast(context, 'أُضيف ${widget.name} إلى المفضّلة');
    } catch (e) {
      if (!mounted) return;
      setState(() => _busy = false);
      showFavoriteToast(context, '$e');
    }
  }

  @override
  Widget build(BuildContext context) {
    if (widget.code.isEmpty || widget.phone.isEmpty) {
      return const SizedBox.shrink();
    }

    return Center(
      child: InkWell(
        onTap: _busy || _done ? null : _add,
        borderRadius: BorderRadius.circular(R.rPill),
        child: Container(
          padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 11),
          decoration: BoxDecoration(
            color: _done ? R.primaryA(.12) : R.whiteA(.7),
            borderRadius: BorderRadius.circular(R.rPill),
            border: Border.all(
                color: _done ? R.primaryA(.35) : R.inkA(.10)),
          ),
          child: Row(
            mainAxisSize: MainAxisSize.min,
            children: [
              if (_busy)
                const SizedBox(
                  width: 16,
                  height: 16,
                  child: CircularProgressIndicator(
                      strokeWidth: 2, color: R.primary),
                )
              else
                Icon(_done ? Icons.star_rounded : Icons.star_outline_rounded,
                    size: 18, color: R.star),
              const SizedBox(width: 8),
              Text(
                _done ? 'في المفضّلة' : 'أضف المستفيد إلى المفضّلة',
                style: T.plex(12.5, FontWeight.w600, color: R.primaryDark),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

/// زرّ «المفضّلة» بجوار حقل المستفيد — يفتح ورقة الاختيار.
class FavoriteFieldButton extends StatelessWidget {
  const FavoriteFieldButton({super.key, required this.onTap});

  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(R.rPill),
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 6),
          child: Row(
            mainAxisSize: MainAxisSize.min,
            children: [
              Icon(Icons.star_rounded, size: 18, color: R.star),
              const SizedBox(width: 5),
              Text('المفضّلة',
                  style: T.plex(11.5, FontWeight.w600, color: R.primaryDark)),
            ],
          ),
        ),
      );
}

class FavoritesScreen extends ConsumerStatefulWidget {
  const FavoritesScreen({super.key});

  @override
  ConsumerState<FavoritesScreen> createState() => _FavoritesScreenState();
}

class _FavoritesScreenState extends ConsumerState<FavoritesScreen> {
  String _q = '';

  Future<void> _remove(FavoriteCustomer c) async {
    final repo = ref.read(favoritesRepositoryProvider);
    try {
      await repo.removeCustomer(c);
      invalidateFavorites(ref);
      if (!mounted) return;
      showFavoriteToast(
        context,
        'حُذف ${c.name} من المفضّلة',
        action: SnackBarAction(
          label: 'تراجع',
          textColor: Colors.white,
          onPressed: () async {
            // الرمز والنوع والهاتف كلها معنا، فالتراجع إعادةُ إضافة لا أكثر.
            for (final f in c.entries) {
              await repo.add(code: f.code, kind: f.kind, phone: f.phone);
            }
            invalidateFavorites(ref);
          },
        ),
      );
    } catch (e) {
      if (!mounted) return;
      showFavoriteToast(context, '$e');
    }
  }

  void _sendTo(FavoriteCustomer c) {
    // نفتح النموذج الذي جاءت منه: من فضّل حوالة خارجية يريدها خارجية.
    final route = switch (c.kind) {
      FavoriteKind.external => '/send/external',
      FavoriteKind.accounts => '/send/accounts',
      FavoriteKind.internal => '/send/internal',
    };
    context.push(route, extra: c);
  }

  @override
  Widget build(BuildContext context) {
    final async = ref.watch(favoritesProvider);
    final currency =
        ref.watch(authControllerProvider).user?.currencyCode ?? 'د.ل';

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'المفضّلة',
            subtitle: 'عملاء سبق أن حوّلتَ لهم',
            onBack: () => context.pop(),
          ),
          Expanded(
            child: async.when(
              loading: () =>
                  const Center(child: CircularProgressIndicator(color: R.primary)),
              error: (e, _) => _Failed(
                message: '$e',
                onRetry: () => ref.invalidate(favoritesProvider),
              ),
              data: (all) {
                if (all.isEmpty) return const _Empty();
                final rows = all.where((c) => c.matches(_q)).toList();

                return Column(
                  children: [
                    Padding(
                      padding: const EdgeInsets.fromLTRB(
                          R.padScreen, 16, R.padScreen, 6),
                      child: TextField(
                        onChanged: (v) => setState(() => _q = v),
                        style: T.value,
                        decoration: InputDecoration(
                          isDense: true,
                          filled: true,
                          fillColor: R.inkA(.05),
                          prefixIcon: Icon(Icons.search_rounded,
                              size: 20, color: R.inkA(.5)),
                          hintText: 'ابحث بالاسم أو الرقم',
                          hintStyle: T.plex(13.5, FontWeight.w400,
                              color: R.inkA(.42)),
                          border: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(R.rRow),
                            borderSide: BorderSide.none,
                          ),
                        ),
                      ),
                    ),
                    Expanded(
                      child: rows.isEmpty
                          ? Center(
                              child: Text('لا نتائج',
                                  style: T.plex(13, FontWeight.w400,
                                      color: R.inkA(.5))),
                            )
                          : ListView.separated(
                              padding: const EdgeInsets.fromLTRB(
                                  R.padScreen, 12, R.padScreen, 40),
                              itemCount: rows.length,
                              separatorBuilder: (_, _) =>
                                  const SizedBox(height: 10),
                              itemBuilder: (_, i) => RiseIn.small(
                                child: _CustomerCard(
                                  customer: rows[i],
                                  currency: currency,
                                  onSend: () => _sendTo(rows[i]),
                                  onRemove: () => _remove(rows[i]),
                                ),
                              ),
                            ),
                    ),
                  ],
                );
              },
            ),
          ),
        ],
      ),
    );
  }
}

class _CustomerCard extends StatelessWidget {
  const _CustomerCard({
    required this.customer,
    required this.currency,
    required this.onSend,
    required this.onRemove,
  });

  final FavoriteCustomer customer;
  final String currency;
  final VoidCallback onSend;
  final VoidCallback onRemove;

  @override
  Widget build(BuildContext context) => GlassCard(
        padding: EdgeInsets.zero,
        child: InkWell(
          // البطاقة كلها تفتح النموذج — والنجمة وحدها تحذف.
          onTap: onSend,
          borderRadius: BorderRadius.circular(R.rCard),
          child: Padding(
            padding: const EdgeInsets.fromLTRB(16, 14, 6, 14),
            child: _body(),
          ),
        ),
      );

  Widget _body() => Row(
          children: [
            _Avatar(letter: customer.initial),
            const SizedBox(width: 12),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    children: [
                      Flexible(
                        child: Text(customer.name,
                            maxLines: 1,
                            overflow: TextOverflow.ellipsis,
                            style: T.kufi(14.5, FontWeight.w700)),
                      ),
                      const SizedBox(width: 8),
                      // النوع مكتوب: القائمتان منفصلتان في النماذج، فقد
                      // يظهر الاسم نفسه مرّتين هنا بنوعين مختلفين.
                      _KindChip(kind: customer.kind),
                    ],
                  ),
                  const SizedBox(height: 5),
                  Directionality(
                    textDirection: TextDirection.ltr,
                    child: Text(Fmt.phone(customer.phone),
                        style: T.plex(12, FontWeight.w400, color: R.inkA(.55))),
                  ),
                  const SizedBox(height: 5),
                  Directionality(
                    textDirection: TextDirection.ltr,
                    child: Row(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        Text(currency,
                            style: T.plex(10.5, FontWeight.w400,
                                color: R.inkA(.45))),
                        const SizedBox(width: 5),
                        Text(Fmt.money(customer.lastAmount),
                            style: T.kufi(12, FontWeight.w600)),
                      ],
                    ),
                  ),
                ],
              ),
            ),
            const SizedBox(width: 6),
            Text('حوِّل له',
                style: T.plex(11.5, FontWeight.w600, color: R.primaryDark)),
            IconButton(
              tooltip: 'حذف من المفضّلة',
              onPressed: onRemove,
              icon: Icon(Icons.star_rounded, size: 22, color: R.primary),
              constraints: const BoxConstraints(minWidth: 44, minHeight: 44),
            ),
          ],
        );
}

class _KindChip extends StatelessWidget {
  const _KindChip({required this.kind});

  final FavoriteKind kind;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 3),
        decoration: BoxDecoration(
          color: R.primaryA(.10),
          borderRadius: BorderRadius.circular(R.rPill),
        ),
        child: Text(kind.label,
            style: T.plex(9.5, FontWeight.w600, color: R.primaryDark)),
      );
}

class _Avatar extends StatelessWidget {
  const _Avatar({required this.letter});

  final String letter;

  @override
  Widget build(BuildContext context) => Container(
        width: 42,
        height: 42,
        alignment: Alignment.center,
        decoration: BoxDecoration(
          shape: BoxShape.circle,
          color: R.primaryA(.12),
        ),
        child: Text(letter, style: T.kufi(16, FontWeight.w700, color: R.primaryDark)),
      );
}

class _Empty extends StatelessWidget {
  const _Empty();

  @override
  Widget build(BuildContext context) => Center(
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 40),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Icon(Icons.star_outline_rounded, size: 46, color: R.primaryA(.35)),
              const SizedBox(height: 16),
              Text('لا مفضّلة بعد',
                  style: T.kufi(15, FontWeight.w700)),
              const SizedBox(height: 8),
              Text(
                'بعد إتمام أي حوالة، اضغط «أضف إلى المفضّلة» '
                'ليظهر المستفيد هنا وتحوّل له لاحقاً بضغطة.',
                textAlign: TextAlign.center,
                style: T.plex(12.5, FontWeight.w400, color: R.inkA(.55), height: 1.6),
              ),
            ],
          ),
        ),
      );
}

class _Failed extends StatelessWidget {
  const _Failed({required this.message, required this.onRetry});

  final String message;
  final VoidCallback onRetry;

  @override
  Widget build(BuildContext context) => Center(
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 32),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Text(message,
                  textAlign: TextAlign.center,
                  style: T.plex(12.5, FontWeight.w500,
                      color: R.errorText, height: 1.6)),
              const SizedBox(height: 14),
              GlassButton(label: 'إعادة المحاولة', onPressed: onRetry),
            ],
          ),
        ),
      );
}

/// ورقة اختيار من المفضّلة — تُفتح من حقل المستفيد في نماذج الحوالة.
class FavoritePickerSheet extends ConsumerStatefulWidget {
  const FavoritePickerSheet({super.key, required this.kind});

  /// **نوع الحوالة التي فُتحت منها الورقة.**
  ///
  /// قرار المالك: مستفيدو الداخلية لا يظهرون في الخارجية ولا العكس —
  /// العملاء مختلفون، وخلطهم يُكدّس أسماءً لا تصلح للوجهة.
  final FavoriteKind kind;

  /// يعيد العميل المختار، أو `null` إن أُغلقت الورقة.
  static Future<FavoriteCustomer?> show(
    BuildContext context,
    FavoriteKind kind,
  ) =>
      showModalBottomSheet<FavoriteCustomer>(
        context: context,
        isScrollControlled: true,
        backgroundColor: Colors.transparent,
        builder: (_) => FavoritePickerSheet(kind: kind),
      );

  @override
  ConsumerState<FavoritePickerSheet> createState() =>
      _FavoritePickerSheetState();
}

class _FavoritePickerSheetState extends ConsumerState<FavoritePickerSheet> {
  String _q = '';

  @override
  Widget build(BuildContext context) {
    final async = ref.watch(favoritesOfKindProvider(widget.kind));

    return Container(
      height: MediaQuery.sizeOf(context).height * .8,
      padding: const EdgeInsets.fromLTRB(22, 22, 22, 0),
      decoration: BoxDecoration(
        color: R.whiteA(.96),
        borderRadius: const BorderRadius.vertical(top: Radius.circular(R.rNav)),
      ),
      child: Column(
        children: [
          Container(
            width: 44,
            height: 4,
            decoration: BoxDecoration(
              color: R.inkA(.16),
              borderRadius: BorderRadius.circular(99),
            ),
          ),
          const SizedBox(height: 20),
          Text('اختر من المفضّلة', style: T.kufi(17, FontWeight.w600)),
          const SizedBox(height: 5),
          // النوع مكتوب صراحةً حتى لا يظنّ الوكيل أن مفضّلته ناقصة حين
          // لا يجد فيها عميلاً فضّله في نوعٍ آخر.
          Text('مستفيدو ${widget.kind.label}',
              style: T.plex(11.5, FontWeight.w400, color: R.inkA(.5))),
          const SizedBox(height: 16),
          TextField(
            autofocus: true,
            onChanged: (v) => setState(() => _q = v),
            style: T.value,
            decoration: InputDecoration(
              isDense: true,
              filled: true,
              fillColor: R.inkA(.05),
              prefixIcon:
                  Icon(Icons.search_rounded, size: 20, color: R.inkA(.5)),
              hintText: 'ابحث بالاسم أو الرقم',
              hintStyle: T.plex(13.5, FontWeight.w400, color: R.inkA(.42)),
              border: OutlineInputBorder(
                borderRadius: BorderRadius.circular(R.rRow),
                borderSide: BorderSide.none,
              ),
            ),
          ),
          const SizedBox(height: 14),
          Expanded(
            child: async.when(
              loading: () => const Center(
                  child: CircularProgressIndicator(color: R.primary)),
              error: (e, _) => Center(
                child: Text('$e',
                    textAlign: TextAlign.center,
                    style: T.plex(12.5, FontWeight.w500, color: R.errorText)),
              ),
              data: (all) {
                final rows = all.where((c) => c.matches(_q)).toList();
                if (rows.isEmpty) {
                  return Center(
                    child: Padding(
                      padding: const EdgeInsets.symmetric(horizontal: 24),
                      child: Text(
                        all.isEmpty
                            ? 'لا مفضّلة في ${widget.kind.label} بعد.\n'
                                'أضف المستفيد من شاشة نجاح الحوالة.'
                            : 'لا نتائج',
                        textAlign: TextAlign.center,
                        style: T.plex(13, FontWeight.w400,
                            color: R.inkA(.5), height: 1.7),
                      ),
                    ),
                  );
                }
                return ListView.separated(
                  padding: const EdgeInsets.only(bottom: 24),
                  itemCount: rows.length,
                  separatorBuilder: (_, _) =>
                      Divider(color: R.inkA(.07), height: 1),
                  itemBuilder: (_, i) {
                    final c = rows[i];
                    return InkWell(
                      onTap: () => Navigator.of(context).pop(c),
                      child: Padding(
                        padding: const EdgeInsets.symmetric(vertical: 13),
                        child: Row(
                          children: [
                            _Avatar(letter: c.initial),
                            const SizedBox(width: 12),
                            Expanded(
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Text(c.name,
                                      maxLines: 1,
                                      overflow: TextOverflow.ellipsis,
                                      style: T.kufi(14, FontWeight.w600)),
                                  const SizedBox(height: 4),
                                  Directionality(
                                    textDirection: TextDirection.ltr,
                                    child: Text(Fmt.phone(c.phone),
                                        style: T.plex(12, FontWeight.w400,
                                            color: R.inkA(.55))),
                                  ),
                                ],
                              ),
                            ),
                            Icon(Icons.chevron_left_rounded,
                                color: R.inkA(.35)),
                          ],
                        ),
                      ),
                    );
                  },
                );
              },
            ),
          ),
        ],
      ),
    );
  }
}
