import 'dart:async';
import 'dart:io';
import 'dart:math' as math;

import 'package:flutter/material.dart';
import 'package:just_audio/just_audio.dart';
import 'package:path_provider/path_provider.dart';
import 'package:record/record.dart';

import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';

/// تسجيل الرسائل الصوتية وتشغيلها (البنود 19–21).
///
/// ## الترميز: Opus في حاوية OGG
///
/// البند 21 يطلب أصغر حجمٍ منطقي بلا تضحيةٍ ملحوظة بالوضوح. و Opus عند
/// 24 kbps أحادي القناة يعطي كلاماً واضحاً تماماً بنحو **180 كيلوبايت
/// للدقيقة** — أي عُشر ما يأخذه AAC عند إعداداته الشائعة. وهو مفتوح وبلا
/// رسوم ترخيص.
///
/// ⚠ ولا نسجّل بمعدّل عيّناتٍ عالٍ: 16 kHz تكفي الكلام تماماً (نطاق الصوت
/// البشري تحتها)، و48 kHz تضاعف الحجم لتنقل ضجيج الغرفة لا كلام الوكيل.
class VoiceRecorder {
  final _rec = AudioRecorder();

  String? _path;
  DateTime? _startedAt;

  bool get isRecording => _startedAt != null;

  /// مدّة ما سُجّل حتى الآن.
  Duration get elapsed => _startedAt == null
      ? Duration.zero
      : DateTime.now().difference(_startedAt!);

  /// الإذن يُطلب هنا — عند أوّل تسجيل، لا عند فتح التطبيق (البند 46).
  Future<bool> hasPermission() => _rec.hasPermission();

  Future<bool> start() async {
    if (!await _rec.hasPermission()) return false;

    final dir = await getTemporaryDirectory();
    _path = '${dir.path}/vn_${DateTime.now().millisecondsSinceEpoch}.ogg';

    await _rec.start(
      const RecordConfig(
        encoder: AudioEncoder.opus,
        bitRate: 24000,
        sampleRate: 16000,
        numChannels: 1,
      ),
      path: _path!,
    );
    _startedAt = DateTime.now();
    return true;
  }

  Future<void> pause() => _rec.pause();
  Future<void> resume() => _rec.resume();

  /// يوقف التسجيل ويعيد مسار الملف — أو null إن لم يُنتج شيئاً.
  Future<String?> stop() async {
    final p = await _rec.stop();
    _startedAt = null;
    return p;
  }

  /// إلغاء: يوقف **ويحذف** الملف.
  ///
  /// الحذف هنا لا لاحقاً: تسجيلٌ ألغاه الوكيل يجب ألّا يبقى على القرص —
  /// هو صوته، وقد يكون قال فيه ما لا يريد بقاءه (البند 74 أيضاً).
  Future<void> cancel() async {
    final p = await stop();
    if (p != null) {
      try {
        await File(p).delete();
      } catch (_) {
        // ملفٌ مؤقّت تعذّر حذفه لا يُسقط الشاشة؛ النظام ينظّف مجلّد المؤقّت.
      }
    }
  }

  /// شدّة الصوت الآن، من 0 إلى 1 — لرسم الموجة أثناء التسجيل.
  Future<double> amplitude() async {
    try {
      final a = await _rec.getAmplitude();
      // الديسيبل سالبٌ ويقترب من الصفر عند العلوّ. -45 حدٌّ عملي للصمت.
      final db = a.current.clamp(-45.0, 0.0);
      return ((db + 45) / 45).clamp(0.0, 1.0);
    } catch (_) {
      return 0;
    }
  }

  Future<void> dispose() => _rec.dispose();
}

/// مشغّل رسالة صوتية بسرعات 1x · 1.5x · 2x وموجةٍ ومؤشّر تقدّم (البند 20).
///
/// مشغّلٌ واحد لكل فقاعة، ويُوقَف حين يبدأ غيره: صوتان معاً لا يُفهم منهما
/// شيء. الشاشة هي من تتولّى ذلك عبر [VoicePlayerScope].
class VoiceBubble extends StatefulWidget {
  const VoiceBubble({
    super.key,
    required this.url,
    required this.headers,
    required this.mine,
    required this.durationHint,
  });

  final String url;
  final Map<String, String> headers;
  final bool mine;

  /// مدّةٌ تقديرية تُعرض قبل تحميل الملف — من حجمه، فلا يظهر «0:00» أوّلاً.
  final Duration durationHint;

  @override
  State<VoiceBubble> createState() => _VoiceBubbleState();
}

class _VoiceBubbleState extends State<VoiceBubble> {
  AudioPlayer? _player;
  StreamSubscription? _posSub;
  StreamSubscription? _stateSub;

  Duration _pos = Duration.zero;
  Duration _dur = Duration.zero;
  bool _playing = false;
  bool _loading = false;
  double _speed = 1;

  static const _speeds = [1.0, 1.5, 2.0];

  @override
  void dispose() {
    _posSub?.cancel();
    _stateSub?.cancel();
    _player?.dispose();
    super.dispose();
  }

  Future<void> _toggle() async {
    if (_playing) {
      await _player?.pause();
      return;
    }

    if (_player == null) {
      setState(() => _loading = true);
      try {
        final p = AudioPlayer();
        // الترويسة تُمرَّر: المرفقات خلف `auth:sanctum`.
        _dur = await p.setUrl(widget.url, headers: widget.headers) ?? Duration.zero;
        _posSub = p.positionStream.listen((d) {
          if (mounted) setState(() => _pos = d);
        });
        _stateSub = p.playerStateStream.listen((s) {
          if (!mounted) return;
          setState(() => _playing = s.playing && s.processingState != ProcessingState.completed);
          if (s.processingState == ProcessingState.completed) {
            // العودة إلى البداية عند الانتهاء: الضغطة التالية تعيد التشغيل
            // لا تقف عند آخر ثانية.
            p.seek(Duration.zero);
            p.pause();
          }
        });
        _player = p;
      } catch (_) {
        if (mounted) setState(() => _loading = false);
        return;
      }
      if (mounted) setState(() => _loading = false);
    }

    await _player!.setSpeed(_speed);
    await _player!.play();
  }

  Future<void> _cycleSpeed() async {
    final next = _speeds[(_speeds.indexOf(_speed) + 1) % _speeds.length];
    setState(() => _speed = next);
    await _player?.setSpeed(next);
  }

  @override
  Widget build(BuildContext context) {
    final total = _dur == Duration.zero ? widget.durationHint : _dur;
    final progress = total.inMilliseconds == 0
        ? 0.0
        : (_pos.inMilliseconds / total.inMilliseconds).clamp(0.0, 1.0);
    final fg = widget.mine ? Colors.white : R.primaryDark;

    return SizedBox(
      width: 218,
      child: Row(
        children: [
          InkWell(
            onTap: _loading ? null : _toggle,
            borderRadius: BorderRadius.circular(99),
            child: Container(
              width: 36,
              height: 36,
              alignment: Alignment.center,
              decoration: BoxDecoration(
                shape: BoxShape.circle,
                color: widget.mine ? R.whiteA(.22) : R.primaryA(.12),
              ),
              child: _loading
                  ? SizedBox(
                      width: 15,
                      height: 15,
                      child: CircularProgressIndicator(strokeWidth: 2, color: fg))
                  : Icon(_playing ? Icons.pause_rounded : Icons.play_arrow_rounded,
                      size: 20, color: fg),
            ),
          ),
          const SizedBox(width: 9),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              mainAxisSize: MainAxisSize.min,
              children: [
                _Waveform(progress: progress, color: fg, seed: widget.url.hashCode),
                const SizedBox(height: 5),
                Row(
                  children: [
                    Directionality(
                      textDirection: TextDirection.ltr,
                      child: Text(_fmt(_playing || _pos > Duration.zero ? _pos : total),
                          style: T.plex(10, FontWeight.w500,
                              color: widget.mine ? R.whiteA(.8) : R.inkA(.5))),
                    ),
                    const Spacer(),
                    // السرعة تظهر بعد بدء التشغيل: زرٌّ لا يفعل شيئاً قبله.
                    if (_player != null)
                      InkWell(
                        onTap: _cycleSpeed,
                        borderRadius: BorderRadius.circular(99),
                        child: Container(
                          padding: const EdgeInsets.symmetric(
                              horizontal: 7, vertical: 2),
                          decoration: BoxDecoration(
                            color: widget.mine ? R.whiteA(.2) : R.primaryA(.1),
                            borderRadius: BorderRadius.circular(99),
                          ),
                          child: Directionality(
                            textDirection: TextDirection.ltr,
                            child: Text('${_speed == 1 ? '1' : _speed}x',
                                style: T.plex(9.5, FontWeight.w700, color: fg)),
                          ),
                        ),
                      ),
                  ],
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  static String _fmt(Duration d) {
    final m = d.inMinutes;
    final s = d.inSeconds % 60;
    return '$m:${s.toString().padLeft(2, '0')}';
  }
}

/// موجة الصوت.
///
/// ⚠ **مرسومة من رقمٍ ثابت لا من تحليل الملف.** تحليل موجةٍ حقيقية يعني
/// تنزيل الملف كاملاً وفكّ ترميزه في الهاتف قبل عرض الفقاعة — تأخيرٌ ظاهر
/// وحرقُ بطارية، مقابل زخرفةٍ لا يقرأ منها أحد معنى. والبذرة من رابط الملف
/// فتبقى موجة كل رسالةٍ **هي نفسها** في كل فتح، ولا ترتجف بين إطارٍ وآخر.
class _Waveform extends StatelessWidget {
  const _Waveform({required this.progress, required this.color, required this.seed});

  final double progress;
  final Color color;
  final int seed;

  static const _bars = 27;

  @override
  Widget build(BuildContext context) {
    final rnd = math.Random(seed);
    final heights = [for (var i = 0; i < _bars; i++) 0.25 + rnd.nextDouble() * 0.75];
    final played = (progress * _bars).floor();

    return SizedBox(
      height: 22,
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.center,
        children: [
          for (var i = 0; i < _bars; i++) ...[
            Expanded(
              child: Container(
                height: 22 * heights[i],
                decoration: BoxDecoration(
                  color: color.withValues(alpha: i <= played ? .95 : .32),
                  borderRadius: BorderRadius.circular(99),
                ),
              ),
            ),
            if (i < _bars - 1) const SizedBox(width: 2),
          ],
        ],
      ),
    );
  }
}
