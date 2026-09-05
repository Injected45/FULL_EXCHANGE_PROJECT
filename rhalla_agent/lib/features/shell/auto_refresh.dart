import 'dart:async';

import 'package:flutter/widgets.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../alerts/incoming_alerts.dart';
import '../chat/chat_unread.dart';
import '../chat/chat_repository.dart';
import '../home/home_repository.dart';
import '../transfers/transfers_repository.dart';

/// كل ما قد يتغيّر على الخادم بعد عملية تُحرّك المال.
///
/// يُستدعى فور نجاح أي حوالة أو تسليم. الرصيد وآخر العمليات وكشف الحساب
/// تتغيّر كلها بالعملية الواحدة، وإبطال الرئيسية وحدها يترك الكشف قديماً
/// فيرى الوكيل رقمين مختلفين للرصيد نفسه في شاشتين.
void refreshAfterMoneyAction(WidgetRef ref) {
  ref.invalidate(homeSnapshotProvider);
  ref.invalidate(statementProvider);
  ref.invalidate(incomingTransfersProvider);
  ref.invalidate(deliveredTransfersProvider);
}

/// يُحدّث بيانات **التبويب الظاهر وحده**، في الخلفية وبلا أن يُرى.
///
/// **لماذا التبويب الظاهر وحده:** `StatefulShellRoute.indexedStack` يُبقي
/// التبويبات حيّة في الشجرة، فمزوّداتها كلها مشتركة ولو لم تُرَ. تحديثها
/// جميعاً في كل نبضة يعني ثلاثة طلبات لكل نبضة — وأحدها
/// (`InternalEx_SelectType_View_not_coustmers_get`) يعيد كل الصفوف بلا
/// ترقيم؛ رُصد 522 صفاً. فنُحدّث ما ينظر إليه الوكيل الآن لا غير.
///
/// و`commissionsProvider` مستثنى عمداً: `CommtionRetview_get` تستغرق
/// ~8 ثوانٍ، فنبضةٌ أقصر منها تعني طلباً لم ينتهِ حتى يبدأ التالي.
///
/// ## أن يكون التحديث غير مرئي — ثلاثة شروط، لا واحد
///
/// كان الوكيل يرى الشاشة «تومض» كل عشر ثوانٍ. الوميض لم يكن من النبضة بل
/// من ثلاثة أشياء اجتمعت، وإصلاحُ أحدها وحده لا يكفي:
///
/// 1. **الشاشة كانت تخلط «إعادة جلب» بـ«تحميل أول».** `AsyncValue.isLoading`
///    تصير `true` في كل تحديث ولو كانت القيمة السابقة في اليد، فكان الرصيد
///    يصير مستطيلاً رمادياً ثم يعود رقماً — ويقرأ الوكيل ذلك اضطراباً في
///    رصيده لا تحديثاً له. العلاج في `home_screen`: الهيكل حين لا قيمة
///    أصلاً، لا حين تُجدَّد قيمة قائمة. (و`AsyncValue.when` يفعل هذا وحده،
///    فما كان يومض هو ما قرأ `isLoading` مباشرة.)
/// 2. **كانت تنبض والإصبع على الشاشة.** إبطالُ مزوّدٍ أثناء سحب قائمة يعيد
///    بناءها تحت الإصبع، فتقفز. النبضة الآن تُلغى ما دام هناك تمرير جارٍ،
///    وتُعاد في موعدها التالي.
/// 3. **كانت تنبض وفوقها شاشة أخرى.** الوكيل يملأ حوالة أو يقرأ فاتورة،
///    والهيكل تحته يجلب بيانات لا يراها أحد — طلباتٌ على شبكة الفرع بلا
///    مقابل. `ModalRoute.isCurrent` يقول إن كان الهيكل هو المعروض فعلاً.
///
/// والفترة صارت 25 ثانية لا 10: نبضة الرئيسية وحدها ثلاثة طلبات متوازية،
/// أحدها يُشغّل مزامنة الوارد في الخادم. ستّ نبضات في الدقيقة تعني ثمانية
/// عشر طلباً — ثمنٌ على شبكة الفرع لا يشتري شيئاً، فالرصيد لا يتغيّر ستّ
/// مرّات في الدقيقة. وحين يريد الوكيل رقماً فوريّاً فالسحب للتحديث في يده.
class AutoRefresh extends ConsumerStatefulWidget {
  const AutoRefresh({
    super.key,
    required this.tabIndex,
    required this.child,
  });

  final int tabIndex;
  final Widget child;

  static const every = Duration(seconds: 25);

  @override
  ConsumerState<AutoRefresh> createState() => _AutoRefreshState();
}

class _AutoRefreshState extends ConsumerState<AutoRefresh>
    with WidgetsBindingObserver {
  Timer? _timer;

  /// مُتحكِّم الجرس، مأخوذ مرّة — يُستعمل في `dispose` حيث لا تصحّ القراءة.
  IncomingAlertsController? _alerts;

  /// شارة الدردشة — تُؤخذ مثل الجرس، وللسبب نفسه.
  ChatUnreadController? _chat;

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addObserver(this);
    _start();
    // جرس الوارد يعمل من الهيكل لا من الشاشة الرئيسية: الوكيل قد يكون في
    // التقارير أو الحساب حين تصل حوالة، وجرسٌ لا يرنّ إلا وصاحبُه ينظر
    // إليه ليس جرساً. وهو خارج الهيكل مستحيل — الهيكل هو ما يعيش طوال
    // الجلسة، والشاشات تحته تُبنى وتُهدَم.
    WidgetsBinding.instance.addPostFrameCallback((_) {
      if (!mounted) return;
      final c = ref.read(incomingAlertsProvider.notifier);
      _alerts = c;
      c.start();

      // شارة الدردشة تتبع الجرس نفسه: كلاهما يعمل من الهيكل لا من شاشة،
      // ويتوقّفان معاً في الخلفية.
      final chat = ref.read(chatUnreadProvider.notifier);
      _chat = chat;
      chat.start();
    });
  }

  @override
  void didUpdateWidget(AutoRefresh old) {
    super.didUpdateWidget(old);
    // انتقل إلى تبويب آخر: نُحدّثه فوراً بدل انتظار نبضةٍ أمام رقم قديم.
    if (old.tabIndex != widget.tabIndex) {
      _scrolling = false;
      _tick();
      _start();
    }
  }

  @override
  void didChangeAppLifecycleState(AppLifecycleState state) {
    if (state == AppLifecycleState.resumed) {
      // خروجٌ من التطبيق والإصبع على الشاشة لا يُنتج `ScrollEndNotification`،
      // فتبقى الراية مرفوعة ويُلغى كل تحديث بعدها إلى الأبد. تُصفَّر هنا:
      // لا تمرير جارٍ في اللحظة التي يعود فيها التطبيق.
      _scrolling = false;
      // عاد الوكيل: يرى أحدث رصيد فوراً، لا بعد نبضة كاملة.
      _tick();
      _start();
      _alerts?.start();
      _chat?.start();
      return;
    }
    // في الخلفية لا شاشة تُقرأ — النبض عندها استنزاف للبطارية وحزمة البيانات.
    _timer?.cancel();
    // والجرس معه: رنّةٌ لشاشةٍ مغلقة ليست تنبيهاً بل إزعاجاً بلا سياق.
    // التنبيه والتطبيقُ مغلق شأن الإشعارات لا شأن هذا المؤقّت.
    _alerts?.stop();
    _chat?.stop();
  }

  void _start() {
    _timer?.cancel();
    _timer = Timer.periodic(AutoRefresh.every, (_) => _tick());
  }

  /// نبضةٌ في وقتٍ يراها الوكيل تُفسد ما ينظر إليه، لا تُحسّنه.
  ///
  /// و«تفويت» نبضة بلا ثمن: المؤقّت دوري، فالنبضة التالية تأتي في موعدها
  /// حين يرفع الوكيل إصبعه أو يُغلق الشاشة التي فوق الهيكل — فلا حاجة إلى
  /// طابورٍ يتذكّر ما فات.
  bool get _quiet {
    if (_scrolling) return false;
    // `isCurrent` تُصدَّق فقط داخل شجرةٍ مركَّبة.
    final route = ModalRoute.of(context);
    if (route != null && !route.isCurrent) return false;
    return true;
  }

  /// إصبعٌ على الشاشة الآن.
  ///
  /// حقلٌ لا حالة: تغيّره لا يستدعي إعادة بناء، و`setState` هنا تعني إعادة
  /// بناء الهيكل كلّه مع كل بداية تمرير ونهايته.
  bool _scrolling = false;

  void _tick() {
    if (!mounted) return;
    if (!_quiet) return;
    // الشريط أربعة: الرئيسية · التقارير · الدردشة · الحساب — واثنان منها
    // فقط يقرآن من الخادم دورياً.
    //
    // وكانت هنا حالتان تُبطلان مزوّدات «الحوالات» و«نقاط البيع» بعد أن خرج
    // التبويبان من الشريط: تُبطلان مزوّدين لا مستمع لهما — أي لا تفعلان
    // شيئاً — بينما تقولان إن التبويبين يُحدَّثان. حُذفتا.
    //
    // وما تفتحه التقارير والحساب من شاشات (كشف الحساب، نقاط البيع،
    // الحوالات) شاشاتٌ مدفوعة تجلب عند فتحها، ولها السحب للتحديث.
    switch (widget.tabIndex) {
      case 0:
        ref.invalidate(homeSnapshotProvider);
      case 2:
        // تبويب الدردشة: القائمة وعدّاداتها. والمحادثة المفتوحة لها نبضتها
        // الأسرع داخل شاشتها — هذه للقائمة وحدها.
        ref.invalidate(chatThreadsProvider);
      // التقارير قائمةُ روابط، والحساب بيانات الجلسة — لا شيء دوريّ فيهما.
    }
  }

  @override
  void dispose() {
    _timer?.cancel();
    // الهيكل لا يُهدَم إلا بالخروج من التطبيق أو تسجيل الخروج، فهذا هو موضع
    // إسكات الجرس ونسيان خطّ أساسه: بلا ذلك يبقى مؤقّتُه يسأل الخادم برمزٍ
    // ميت، ويرى الوكيل التالي على الجهاز نفسه عدّاد من قبله.
    //
    // والمُتحكِّم مأخوذ في `initState` لا هنا: قراءة مزوّد أثناء الهدم قد
    // تقع بعد هدم النطاق نفسه.
    _alerts?.reset();
    _chat?.reset();
    WidgetsBinding.instance.removeObserver(this);
    super.dispose();
  }

  @override
  Widget build(BuildContext context) =>
      NotificationListener<ScrollNotification>(
        // `false` تعني: مرّرها لمن فوقنا أيضاً. الابتلاع هنا يكسر أي
        // مستمعٍ آخر للتمرير في الشجرة.
        onNotification: (n) {
          if (n is ScrollStartNotification) _scrolling = true;
          if (n is ScrollEndNotification) _scrolling = false;
          return false;
        },
        child: widget.child,
      );
}
