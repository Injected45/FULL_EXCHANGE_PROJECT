import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/ambient.dart';
import '../../ui/widgets/glass.dart';
import 'auth_controller.dart';

class _Slide {
  const _Slide(this.title, this.body);
  final String title;
  final String body;
}

const _slides = [
  _Slide('وكالتك كاملة\nبين يديك',
      'أنشئ الحوالات، سلّمها، وتابع رصيدك وسقوفك وعمولاتك — من الهاتف، بلا رجوع إلى المكتب.'),
  _Slide('كل حوالة\nبرمز واحد',
      'الرمز يصل المستفيد عبر واتساب فور الإنشاء، ويستلم به من أي فرع.'),
  _Slide('نقاط بيعك\nتحت عينك',
      'أضف مخوّلاً، أوقفه، وتابع حركته — دون مراجعة الإدارة.'),
];

class OnboardingScreen extends ConsumerStatefulWidget {
  const OnboardingScreen({super.key});

  @override
  ConsumerState<OnboardingScreen> createState() => _OnboardingScreenState();
}

class _OnboardingScreenState extends ConsumerState<OnboardingScreen> {
  final _pc = PageController();
  int _i = 0;

  @override
  void dispose() {
    _pc.dispose();
    super.dispose();
  }

  Future<void> _finish() async {
    await ref.read(authControllerProvider.notifier).markOnboarded();
    if (mounted) context.go('/phone');
  }

  @override
  Widget build(BuildContext context) {
    final last = _i == _slides.length - 1;

    return Screen(
      child: Stack(
        children: [
          // البطاقات العائمة
          Positioned(
            top: 104,
            right: 0,
            left: 0,
            height: 320,
            child: Stack(
              children: [
                PositionedDirectional(
                  top: 26,
                  start: 44,
                  child: Floaty(
                    child: SizedBox(
                      width: 250,
                      child: GlassCard(
                        large: true,
                        sheen: true,
                        padding: const EdgeInsets.fromLTRB(22, 20, 22, 20),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text('رصيد الوكالة',
                                style: T.plex(11, FontWeight.w400, color: R.inkA(.55))),
                            const SizedBox(height: 9),
                            Directionality(
                              textDirection: TextDirection.ltr,
                              child: Row(
                                crossAxisAlignment: CrossAxisAlignment.baseline,
                                textBaseline: TextBaseline.alphabetic,
                                children: [
                                  const Spacer(),
                                  Text('د.ل',
                                      style: T.plex(12, FontWeight.w400,
                                          color: R.inkA(.5))),
                                  const SizedBox(width: 8),
                                  Text('48,320.75',
                                      style: T.kufi(28, FontWeight.w700)),
                                ],
                              ),
                            ),
                          ],
                        ),
                      ),
                    ),
                  ),
                ),
                PositionedDirectional(
                  top: 152,
                  start: 150,
                  child: Floaty(
                    seconds: 7.5,
                    dy: 9,
                    child: Container(
                      width: 160,
                      padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 13),
                      decoration: BoxDecoration(
                        color: R.primary,
                        borderRadius: BorderRadius.circular(R.rRow),
                        boxShadow: [
                          BoxShadow(
                            color: R.primaryA(.30),
                            blurRadius: 32,
                            offset: const Offset(0, 16),
                          )
                        ],
                      ),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text('حوالة جديدة · 1,250.000',
                              style: T.kufi(12, FontWeight.w600, color: Colors.white)),
                          const SizedBox(height: 6),
                          Text('بانتظار التسليم',
                              style: T.plex(10.5, FontWeight.w400,
                                  color: R.whiteA(.8))),
                        ],
                      ),
                    ),
                  ),
                ),
                PositionedDirectional(
                  top: 214,
                  start: 56,
                  child: Floaty(
                    seconds: 2.8,
                    dy: 6,
                    child: RhallaLogo(size: 54, color: R.primaryGradEnd),
                  ),
                ),
              ],
            ),
          ),

          // النص والتحكّم
          Positioned(
            bottom: 0,
            right: 0,
            left: 0,
            child: Container(
              // 132 للنص + 22 فجوة + 52 للتحكّم + 26 حشو سفلي = 232.
              // كانت 210، فطفح السطر الثاني من النص الشارح بـ 14px.
              height: 240,
              padding: const EdgeInsets.fromLTRB(30, 0, 30, 26),
              decoration: BoxDecoration(
                gradient: LinearGradient(
                  begin: Alignment.topCenter,
                  end: Alignment.bottomCenter,
                  colors: [Color(0x00F1F8F5), Color(0xD9F3FAF7), R.scrimBottom],
                  stops: [0, .3, 1],
                ),
              ),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.end,
                children: [
                  SizedBox(
                    // عنوان سطرين (24×1.35×2 ≈ 65) + 10 + نص سطرين (13×1.7×2 ≈ 45).
                    height: 132,
                    child: PageView.builder(
                      controller: _pc,
                      itemCount: _slides.length,
                      onPageChanged: (i) => setState(() => _i = i),
                      itemBuilder: (_, i) => Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text(_slides[i].title,
                              style: T.kufi(24, FontWeight.w700, height: 1.35)),
                          const SizedBox(height: 10),
                          Text(_slides[i].body, style: T.body),
                        ],
                      ),
                    ),
                  ),
                  const SizedBox(height: 22),
                  Row(
                    children: [
                      for (var i = 0; i < _slides.length; i++) ...[
                        if (i > 0) const SizedBox(width: 7),
                        AnimatedContainer(
                          duration: const Duration(milliseconds: 500),
                          curve: const Cubic(.2, .9, .2, 1),
                          width: i == _i ? 24 : 7,
                          height: 7,
                          decoration: BoxDecoration(
                            color: i == _i ? R.primary : R.inkA(.16),
                            borderRadius: BorderRadius.circular(99),
                          ),
                        ),
                      ],
                      const Spacer(),
                      if (!last)
                        TextButton(
                          onPressed: _finish,
                          style: TextButton.styleFrom(
                            minimumSize: const Size(44, 44),
                          ),
                          child: Text('تخطي',
                              style: T.plex(13, FontWeight.w500,
                                  color: R.inkA(.55))),
                        ),
                      const SizedBox(width: 8),
                      _NextPill(
                        label: last ? 'ابدأ الآن' : 'التالي',
                        onTap: () {
                          if (last) {
                            _finish();
                          } else {
                            _pc.nextPage(
                              duration: const Duration(milliseconds: 420),
                              curve: const Cubic(.22, .9, .2, 1),
                            );
                          }
                        },
                      ),
                    ],
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

class _NextPill extends StatelessWidget {
  const _NextPill({required this.label, required this.onTap});

  final String label;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => DecoratedBox(
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(R.rPill),
          boxShadow: R.shPill,
        ),
        child: Material(
          color: Colors.transparent,
          borderRadius: BorderRadius.circular(R.rPill),
          child: InkWell(
            borderRadius: BorderRadius.circular(R.rPill),
            onTap: onTap,
            child: Ink(
              height: 52,
              decoration: BoxDecoration(
                gradient: R.primaryGradient,
                borderRadius: BorderRadius.circular(R.rPill),
              ),
              child: Padding(
                padding: const EdgeInsets.symmetric(horizontal: 26),
                child: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Text(label, style: T.kufi(14, FontWeight.w600, color: Colors.white)),
                    const SizedBox(width: 10),
                    const Icon(Icons.arrow_back_ios_new, size: 15, color: Colors.white),
                  ],
                ),
              ),
            ),
          ),
        ),
      );
}
