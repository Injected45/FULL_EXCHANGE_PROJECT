import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/net/api_client.dart';
import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';

/// الشروط والأحكام.
///
/// ⚠️ النقطة تعيد **نصاً واحداً مدموجاً** لا مصفوفة: الخادم يجمع صفوف
/// `AppTerms` بـ `implode("\n\n")` بعد أن يسبق كلاً منها بـ `Order . '. '`.
/// فالتقسيم إلى بنود يحدث هنا، على `\n\n`، لا في الخادم.
///
/// وقد يعود **فارغاً** — جدول `AppTerms` خالٍ في القاعدة المحلية،
/// و`success` يبقى `true`. فالفراغ حالة عادية لا خطأ.
final termsProvider = FutureProvider.autoDispose<String>((ref) async {
  final api = ref.watch(apiClientProvider);
  try {
    final env = await api.get('/device/exchange/AppTerms_get');
    final p = env.payload;
    return p is String ? p.trim() : '';
  } on ApiFailure catch (e) {
    if (e.isEmptyResult) return '';
    rethrow;
  }
});

class TermsScreen extends ConsumerWidget {
  const TermsScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final async = ref.watch(termsProvider);

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'الشروط والأحكام',
            subtitle: 'شركة الرحالة للصرافة',
            onBack: () => context.pop(),
          ),
          Expanded(
            child: async.when(
              loading: () =>
                  const Center(child: CircularProgressIndicator(color: R.primary)),
              error: (e, _) => _Failed(
                message: '$e',
                onRetry: () => ref.invalidate(termsProvider),
              ),
              data: (text) {
                if (text.isEmpty) return const _NoTerms();

                final items = text
                    .split(RegExp(r'\n\s*\n'))
                    .map((s) => s.trim())
                    .where((s) => s.isNotEmpty)
                    .toList();

                return ListView.separated(
                  padding: const EdgeInsets.fromLTRB(
                      R.padScreen, 20, R.padScreen, 120),
                  itemCount: items.length,
                  separatorBuilder: (_, _) => const SizedBox(height: R.gapRow),
                  itemBuilder: (_, i) => GlassCard(
                    child: Text(items[i],
                        style: T.plex(13, FontWeight.w400,
                            color: R.inkA(.78), height: 1.9)),
                  ),
                );
              },
            ),
          ),
        ],
      ),
    );
  }
}

class _NoTerms extends StatelessWidget {
  const _NoTerms();

  @override
  Widget build(BuildContext context) => Center(
        child: Padding(
          padding: const EdgeInsets.all(40),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              RhallaLogo(size: 56, color: R.primaryA(.3)),
              const SizedBox(height: 20),
              Text('لم تُنشر الشروط بعد',
                  textAlign: TextAlign.center,
                  style: T.kufi(15, FontWeight.w600, height: 1.5)),
              const SizedBox(height: 10),
              Text('تُضاف من المكتب الخلفي وتظهر هنا فور نشرها.',
                  textAlign: TextAlign.center,
                  style: T.plex(12.5, FontWeight.w400,
                      color: R.inkA(.55), height: 1.7)),
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
          padding: const EdgeInsets.all(R.padScreen),
          child: GlassCard(
            child: Column(
              mainAxisSize: MainAxisSize.min,
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(message,
                    style: T.plex(12.5, FontWeight.w500,
                        color: R.errorText, height: 1.6)),
                const SizedBox(height: 12),
                TextButton(
                  onPressed: onRetry,
                  style: TextButton.styleFrom(minimumSize: const Size(44, 44)),
                  child: Text('إعادة المحاولة',
                      style: T.plex(12.5, FontWeight.w600,
                          color: R.primaryGradEnd)),
                ),
              ],
            ),
          ),
        ),
      );
}
