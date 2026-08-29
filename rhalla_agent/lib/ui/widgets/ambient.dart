import 'dart:ui';

import 'package:flutter/material.dart';

import '../../core/theme/tokens.dart';

/// الخلفية المتحركة — أربع كتل تدور ببطء خلف كل شاشة.
/// مطابقة لـ orbit1/2/3 و hueDrift في نظام التصميم.
///
/// تُبنى مرة واحدة فوق الـ Navigator حتى لا تعيد الحركة البدء مع كل شاشة.
class AmbientBackground extends StatefulWidget {
  const AmbientBackground({super.key, required this.child});

  final Widget child;

  @override
  State<AmbientBackground> createState() => _AmbientBackgroundState();
}

class _AmbientBackgroundState extends State<AmbientBackground>
    with TickerProviderStateMixin {
  late final _c1 = AnimationController(vsync: this, duration: const Duration(seconds: 24))..repeat();
  late final _c2 = AnimationController(vsync: this, duration: const Duration(seconds: 31))..repeat();
  late final _c3 = AnimationController(vsync: this, duration: const Duration(seconds: 27))..repeat();
  late final _c4 = AnimationController(vsync: this, duration: const Duration(seconds: 18))
    ..repeat(reverse: true);

  @override
  void dispose() {
    _c1.dispose();
    _c2.dispose();
    _c3.dispose();
    _c4.dispose();
    super.dispose();
  }

  /// حركة orbit — إزاحة وتحجيم دوريان.
  Offset _lerpPath(double t, List<Offset> stops) {
    final n = stops.length - 1;
    final scaled = t * n;
    final i = scaled.floor().clamp(0, n - 1);
    final f = Curves.easeInOut.transform(scaled - i);
    return Offset.lerp(stops[i], stops[i + 1], f)!;
  }

  double _lerpScale(double t, List<double> stops) {
    final n = stops.length - 1;
    final scaled = t * n;
    final i = scaled.floor().clamp(0, n - 1);
    final f = Curves.easeInOut.transform(scaled - i);
    return lerpDouble(stops[i], stops[i + 1], f)!;
  }

  Widget _blob({
    required AnimationController c,
    required List<Offset> path,
    required List<double> scales,
    required double size,
    required Color color,
    required double blur,
    double? top,
    double? bottom,
    double? start,
    double? end,
  }) {
    return PositionedDirectional(
      top: top,
      bottom: bottom,
      start: start,
      end: end,
      child: AnimatedBuilder(
        animation: c,
        builder: (_, _) {
          final o = _lerpPath(c.value, path);
          final s = _lerpScale(c.value, scales);
          return Transform.translate(
            offset: o,
            child: Transform.scale(
              scale: s,
              child: ImageFiltered(
                imageFilter: ImageFilter.blur(sigmaX: blur, sigmaY: blur),
                child: Container(
                  width: size,
                  height: size,
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    gradient: RadialGradient(
                      center: const Alignment(-0.16, -0.24),
                      colors: [color, color.withValues(alpha: 0)],
                      stops: const [0, .7],
                    ),
                  ),
                ),
              ),
            ),
          );
        },
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return DecoratedBox(
      decoration: const BoxDecoration(gradient: R.screenBackground),
      child: Stack(
        children: [
          Positioned.fill(
            child: IgnorePointer(
              child: ClipRect(
                child: Stack(
                  children: [
                    _blob(
                      c: _c1,
                      top: -90,
                      end: -70,
                      size: 340,
                      blur: 9,
                      color: const Color(0xFF00C489).withValues(alpha: .55),
                      path: const [Offset(0, 0), Offset(58, 44), Offset(-34, 74), Offset(0, 0)],
                      scales: const [1, 1.16, .92, 1],
                    ),
                    _blob(
                      c: _c2,
                      top: 210,
                      start: -120,
                      size: 300,
                      blur: 11,
                      color: const Color(0xFF00BEBE).withValues(alpha: .34),
                      path: const [Offset(0, 0), Offset(-66, -38), Offset(30, 32), Offset(0, 0)],
                      scales: const [1.05, .88, 1.2, 1.05],
                    ),
                    _blob(
                      c: _c3,
                      bottom: -60,
                      end: -40,
                      size: 320,
                      blur: 13,
                      color: const Color(0xFF00875E).withValues(alpha: .30),
                      path: const [Offset(0, 0), Offset(46, -58), Offset(0, 0)],
                      scales: const [.95, 1.24, .95],
                    ),
                    // الطبقة الكهرمانية — hueDrift
                    Positioned.fill(
                      child: FadeTransition(
                        opacity: Tween(begin: .55, end: .95).animate(_c4),
                        child: DecoratedBox(
                          decoration: BoxDecoration(
                            gradient: RadialGradient(
                              center: const Alignment(0.6, 0.6),
                              radius: .9,
                              colors: [
                                const Color(0xFFFFD678).withValues(alpha: .16),
                                const Color(0xFFFFD678).withValues(alpha: 0),
                              ],
                              stops: const [0, .65],
                            ),
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
          widget.child,
        ],
      ),
    );
  }
}

/// حركة الدخول — riseIn / riseInSm من نظام التصميم.
class RiseIn extends StatefulWidget {
  const RiseIn({
    super.key,
    required this.child,
    this.delay = Duration.zero,
    this.duration = const Duration(milliseconds: 600),
    this.offset = 18,
  });

  final Widget child;
  final Duration delay;
  final Duration duration;
  final double offset;

  const RiseIn.small({
    super.key,
    required this.child,
    this.delay = Duration.zero,
  })  : duration = const Duration(milliseconds: 450),
        offset = 9;

  @override
  State<RiseIn> createState() => _RiseInState();
}

class _RiseInState extends State<RiseIn> with SingleTickerProviderStateMixin {
  late final _c = AnimationController(vsync: this, duration: widget.duration);

  @override
  void initState() {
    super.initState();
    if (widget.delay == Duration.zero) {
      _c.forward();
    } else {
      Future.delayed(widget.delay, () {
        if (mounted) _c.forward();
      });
    }
  }

  @override
  void dispose() {
    _c.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final curved = CurvedAnimation(parent: _c, curve: const Cubic(.2, .8, .2, 1));
    return AnimatedBuilder(
      animation: curved,
      builder: (_, child) => Opacity(
        opacity: curved.value.clamp(0, 1),
        child: Transform.translate(
          offset: Offset(0, widget.offset * (1 - curved.value)),
          child: child,
        ),
      ),
      child: widget.child,
    );
  }
}

/// لمعان يمرّ فوق البطاقات الكبيرة — sheen.
class Sheen extends StatefulWidget {
  const Sheen({super.key});

  @override
  State<Sheen> createState() => _SheenState();
}

class _SheenState extends State<Sheen> with SingleTickerProviderStateMixin {
  late final _c = AnimationController(vsync: this, duration: const Duration(seconds: 7))..repeat();

  @override
  void dispose() {
    _c.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return IgnorePointer(
      child: AnimatedBuilder(
        animation: _c,
        builder: (_, _) {
          // يمرّ في أول 55% ثم يستريح.
          final t = (_c.value / .55).clamp(0.0, 1.0);
          final eased = Curves.easeInOut.transform(t);
          return FractionalTranslation(
            translation: Offset(lerpDouble(1.4, -2.4, eased)!, 0),
            child: Transform(
              transform: Matrix4.skewX(-0.31), // ‑18°
              child: DecoratedBox(
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    begin: Alignment.topLeft,
                    end: Alignment.bottomRight,
                    colors: [
                      Colors.transparent,
                      Colors.white.withValues(alpha: .4),
                      Colors.transparent,
                    ],
                    stops: const [.38, .5, .62],
                  ),
                ),
              ),
            ),
          );
        },
      ),
    );
  }
}

/// طفو بطيء — floatA.
class Floaty extends StatefulWidget {
  const Floaty({super.key, required this.child, this.seconds = 6.5, this.dy = 13});

  final Widget child;
  final double seconds;
  final double dy;

  @override
  State<Floaty> createState() => _FloatyState();
}

class _FloatyState extends State<Floaty> with SingleTickerProviderStateMixin {
  late final _c = AnimationController(
    vsync: this,
    duration: Duration(milliseconds: (widget.seconds * 500).round()),
  )..repeat(reverse: true);

  @override
  void dispose() {
    _c.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) => AnimatedBuilder(
        animation: _c,
        builder: (_, child) => Transform.translate(
          offset: Offset(0, -widget.dy * Curves.easeInOut.transform(_c.value)),
          child: child,
        ),
        child: widget.child,
      );
}

/// هالة نابضة — ringOut.
class PulseRing extends StatefulWidget {
  const PulseRing({super.key, this.color, this.seconds = 2.6});

  final Color? color;
  final double seconds;

  @override
  State<PulseRing> createState() => _PulseRingState();
}

class _PulseRingState extends State<PulseRing> with SingleTickerProviderStateMixin {
  late final _c = AnimationController(
    vsync: this,
    duration: Duration(milliseconds: (widget.seconds * 1000).round()),
  )..repeat();

  @override
  void dispose() {
    _c.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) => IgnorePointer(
        child: AnimatedBuilder(
          animation: _c,
          builder: (_, _) {
            final t = Curves.easeOut.transform(_c.value);
            final opacity = t < .7 ? .55 * (1 - t / .7) : 0.0;
            return Transform.scale(
              scale: lerpDouble(.9, 1.5, t)!,
              child: Container(
                decoration: BoxDecoration(
                  shape: BoxShape.circle,
                  border: Border.all(
                    color: (widget.color ?? R.primary).withValues(alpha: opacity.clamp(0, 1)),
                  ),
                ),
              ),
            );
          },
        ),
      );
}
