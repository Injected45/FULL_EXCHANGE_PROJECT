import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/storage/secure_store.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../auth/auth_controller.dart';

final _deviceIdProvider =
    FutureProvider.autoDispose<String>((ref) => SecureStore().deviceId());

/// الخصوصية والأمان.
///
/// لا تخترع هذه الشاشة إعدادات لا يملكها الخادم. الخادم يربط **جهازاً واحداً**
/// بكل مستخدم ويرفض غيره، وهذه أهم حقيقة أمنية في التطبيق — وأخطرها على
/// الوكيل: إعادة تثبيت التطبيق تولّد مُعرّفاً جديداً فيُرفض الدخول، ولا
/// يُستعاد الحساب إلا بإعادة تعيين `Reg='NO'` من المكتب الخلفي.
class SecurityScreen extends ConsumerWidget {
  const SecurityScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final user = ref.watch(authControllerProvider).user;
    final deviceId = ref.watch(_deviceIdProvider);

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'الخصوصية والأمان',
            subtitle: 'الجهاز والجلسة',
            onBack: () => context.pop(),
          ),
          Expanded(
            child: ListView(
              padding: const EdgeInsets.fromLTRB(
                  R.padScreen, 20, R.padScreen, 120),
              children: [
                GlassCard(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Row(
                        children: [
                          IconTile(
                            icon: const Icon(Icons.smartphone_rounded,
                                size: 19, color: R.primaryGradEnd),
                          ),
                          const SizedBox(width: 13),
                          Expanded(
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                Text('جهاز واحد لكل حساب', style: T.name),
                                const SizedBox(height: 5),
                                Text('هذا الجهاز هو المرتبط بحسابك.',
                                    style: T.meta),
                              ],
                            ),
                          ),
                        ],
                      ),
                      const SizedBox(height: 16),
                      Divider(color: R.inkA(.07), height: 1),
                      const SizedBox(height: 14),
                      Text('مُعرّف الجهاز', style: T.label),
                      const SizedBox(height: 8),
                      deviceId.when(
                        loading: () => Text('…', style: T.value),
                        error: (_, _) => Text('غير متاح', style: T.meta),
                        data: (id) => _Copyable(value: id),
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: R.gapCard),

                const WarnBanner(
                  text:
                      'لا تحذف التطبيق ولا تُعِد ضبط الجهاز قبل مراجعة الفرع: '
                      'المُعرّف يتغيّر فيرفض الخادم الدخول، ولا يُستعاد الحساب '
                      'إلا بإعادة تفعيله من المكتب الخلفي.',
                ),
                const SizedBox(height: R.gapCard),

                GlassCard(
                  padding:
                      const EdgeInsets.symmetric(horizontal: 18, vertical: 6),
                  child: Column(
                    children: [
                      _Fact(
                        icon: Icons.password_rounded,
                        label: 'الدخول',
                        value: 'برمز تحقّق عبر واتساب',
                      ),
                      _Fact(
                        icon: Icons.lock_clock_outlined,
                        label: 'الجلسة',
                        value: 'مفتوحة حتى تسجيل الخروج',
                      ),
                      _Fact(
                        icon: Icons.badge_outlined,
                        label: 'الهاتف المسجّل',
                        value: user == null ? '—' : Fmt.phone(user.phone),
                        ltr: true,
                      ),
                      _Fact(
                        icon: Icons.tag_rounded,
                        label: 'رقم الحساب',
                        value: user == null ? '—' : '${user.accId}',
                        ltr: true,
                        last: true,
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: R.gapCard),

                GlassCard(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text('ما يُخزَّن على الجهاز', style: T.section),
                      const SizedBox(height: 12),
                      _Bullet('رمز الجلسة ومُعرّف الجهاز — في مخزن النظام '
                          'المُعمّى (Keystore)، لا في ملفات التطبيق.'),
                      _Bullet('اسمك ورقم حسابك، لعرضهما قبل اكتمال الاتصال.'),
                      _Bullet('لا تُخزَّن أرصدة ولا حوالات على الجهاز — '
                          'تُطلب من الخادم في كل مرة.'),
                      const SizedBox(height: 6),
                      Text('تسجيل الخروج يمحو الرمز وبياناتك المحفوظة، '
                          'ويُبقي مُعرّف الجهاز كما هو عمداً حتى لا يُرفض دخولك '
                          'في المرة القادمة.',
                          style: T.plex(12, FontWeight.w400,
                              color: R.inkA(.55), height: 1.8)),
                    ],
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}

class _Copyable extends StatelessWidget {
  const _Copyable({required this.value});

  final String value;

  @override
  Widget build(BuildContext context) => Row(
        children: [
          Expanded(
            child: Directionality(
              textDirection: TextDirection.ltr,
              child: SelectableText(value,
                  style: T.kufi(12, FontWeight.w600, color: R.inkA(.7))),
            ),
          ),
          IconButton(
            tooltip: 'نسخ',
            onPressed: () {
              Clipboard.setData(ClipboardData(text: value));
              ScaffoldMessenger.of(context)
                ..hideCurrentSnackBar()
                ..showSnackBar(SnackBar(
                  content: Text('نُسخ مُعرّف الجهاز',
                      style: T.plex(13, FontWeight.w500, color: Colors.white)),
                  backgroundColor: R.primaryGradEnd,
                  behavior: SnackBarBehavior.floating,
                  margin: const EdgeInsets.fromLTRB(16, 0, 16, 100),
                  shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(16)),
                ));
            },
            icon: Icon(Icons.copy_rounded, size: 18, color: R.inkA(.5)),
            constraints: const BoxConstraints(minWidth: 44, minHeight: 44),
          ),
        ],
      );
}

class _Fact extends StatelessWidget {
  const _Fact({
    required this.icon,
    required this.label,
    required this.value,
    this.ltr = false,
    this.last = false,
  });

  final IconData icon;
  final String label;
  final String value;
  final bool ltr;
  final bool last;

  @override
  Widget build(BuildContext context) => Column(
        children: [
          Container(
            constraints: const BoxConstraints(minHeight: 52),
            child: Row(
              children: [
                Icon(icon, size: 19, color: R.inkA(.45)),
                const SizedBox(width: 13),
                Expanded(child: Text(label, style: T.plex(13.5, FontWeight.w500))),
                ltr
                    ? Directionality(
                        textDirection: TextDirection.ltr,
                        child: Text(value,
                            style: T.kufi(13, FontWeight.w600,
                                color: R.inkA(.6))),
                      )
                    : Text(value,
                        style:
                            T.plex(12.5, FontWeight.w400, color: R.inkA(.55))),
              ],
            ),
          ),
          if (!last) Divider(color: R.inkA(.06), height: 1),
        ],
      );
}

class _Bullet extends StatelessWidget {
  const _Bullet(this.text);

  final String text;

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.only(bottom: 10),
        child: Row(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Container(
              margin: const EdgeInsetsDirectional.only(top: 8, end: 10),
              width: 5,
              height: 5,
              decoration: const BoxDecoration(
                  color: R.primary, shape: BoxShape.circle),
            ),
            Expanded(
              child: Text(text,
                  style: T.plex(12.5, FontWeight.w400,
                      color: R.inkA(.68), height: 1.8)),
            ),
          ],
        ),
      );
}
