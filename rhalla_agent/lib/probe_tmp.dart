// مؤقت للتشخيص فقط — يُحذف بعد الانتهاء.
import 'dart:ui' as ui;

import 'package:flutter/material.dart';
import 'package:flutter/scheduler.dart';

void main() {
  WidgetsFlutterBinding.ensureInitialized();

  void report(String tag) {
    final views = ui.PlatformDispatcher.instance.views;
    debugPrint('PROBE[$tag] views=${views.length}');
    for (final v in views) {
      debugPrint('PROBE[$tag] physicalSize=${v.physicalSize} '
          'dpr=${v.devicePixelRatio} '
          'padding=${v.padding} '
          'display=${v.display.size}@${v.display.devicePixelRatio}');
    }
  }

  report('boot');
  SchedulerBinding.instance.addPostFrameCallback((_) => report('firstFrame'));
  Future.delayed(const Duration(seconds: 3), () => report('t+3s'));

  runApp(const _Probe());
}

class _Probe extends StatelessWidget {
  const _Probe();

  @override
  Widget build(BuildContext context) {
    debugPrint('PROBE[build] mq=${MediaQuery.maybeOf(context)?.size}');
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      home: Builder(builder: (c) {
        debugPrint('PROBE[home] mq=${MediaQuery.of(c).size}');
        return Container(
          color: Colors.red,
          child: const Center(
            child: Text('PROBE',
                style: TextStyle(color: Colors.white, fontSize: 40)),
          ),
        );
      }),
    );
  }
}
