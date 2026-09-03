import 'dart:ui';

import 'package:flutter/material.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/ambient.dart';
import '../../ui/widgets/glass.dart';

class SplashScreen extends StatefulWidget {
  const SplashScreen({super.key});

  @override
  State<SplashScreen> createState() => _SplashScreenState();
}

class _SplashScreenState extends State<SplashScreen>
    with SingleTickerProviderStateMixin {
  // مدّة الشريط في التصميم: 2s بعد تأخير .15s.
  late final _progress = AnimationController(
    vsync: this,
    duration: const Duration(milliseconds: 2000),
  );

  @override
  void initState() {
    super.initState();
    Future.delayed(const Duration(milliseconds: 150), () {
      if (mounted) _progress.forward();
    });
  }

  @override
  void dispose() {
    _progress.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Screen(
      child: Stack(
        alignment: Alignment.center,
        children: [
          // التلال
          Positioned(
            bottom: -60,
            left: -40,
            right: -40,
            child: RiseIn(
              duration: const Duration(milliseconds: 1100),
              child: Container(
                height: 330,
                decoration: BoxDecoration(
                  borderRadius: const BorderRadius.vertical(top: Radius.elliptical(400, 330)),
                  gradient: LinearGradient(
                    begin: Alignment.topCenter,
                    end: Alignment.bottomCenter,
                    colors: [R.primaryA(.16), R.primaryA(.02)],
                  ),
                ),
              ),
            ),
          ),

          Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              // الميدالية
              SizedBox(
                width: 150,
                height: 150,
                child: Stack(
                  alignment: Alignment.center,
                  children: [
                    const Positioned.fill(child: PulseRing()),
                    Positioned.fill(
                      left: 14,
                      right: 14,
                      top: 14,
                      bottom: 14,
                      child: ClipOval(
                        child: BackdropFilter(
                          filter: ImageFilter.blur(sigmaX: 10, sigmaY: 10),
                          child: Container(
                            decoration: BoxDecoration(
                              shape: BoxShape.circle,
                              color: R.whiteA(.62),
                              border: Border.all(color: R.whiteA(.9)),
                            ),
                            child: Center(
                              child: RhallaLogo(size: 82, color: R.primary),
                            ),
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
              ),

              const SizedBox(height: 24),

              RiseIn(
                delay: const Duration(milliseconds: 300),
                duration: const Duration(milliseconds: 700),
                child: Column(
                  children: [
                    Text('رحلة', style: T.kufi(40, FontWeight.w800, spacing: .8)),
                    const SizedBox(height: 12),
                    Directionality(
                      textDirection: TextDirection.ltr,
                      child: Text(
                        'RHALLA',
                        style: T.plex(11, FontWeight.w500,
                            color: R.inkA(.5), spacing: 4.6),
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),

          Positioned(
            bottom: 96,
            child: SizedBox(
              width: 132,
              height: 3,
              child: DecoratedBox(
                decoration: BoxDecoration(
                  color: R.inkA(.09),
                  borderRadius: BorderRadius.circular(99),
                ),
                child: AnimatedBuilder(
                  animation: _progress,
                  builder: (_, _) => FractionallySizedBox(
                    alignment: AlignmentDirectional.centerStart,
                    widthFactor:
                        const Cubic(.5, 0, .3, 1).transform(_progress.value),
                    child: DecoratedBox(
                      decoration: BoxDecoration(
                        color: R.primary,
                        borderRadius: BorderRadius.circular(99),
                      ),
                    ),
                  ),
                ),
              ),
            ),
          ),

          Positioned(
            bottom: 52,
            child: Text('وكالتك في جيبك',
                style: T.plex(11.5, FontWeight.w400, color: R.inkA(.5))),
          ),
        ],
      ),
    );
  }
}
