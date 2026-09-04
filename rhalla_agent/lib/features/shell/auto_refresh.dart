import 'dart:async';

import 'package:flutter/widgets.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../home/home_repository.dart';
import '../pos/pos_repository.dart';
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

/// يُحدّث بيانات **التبويب الظاهر وحده** كل عشر ثوانٍ.
///
/// **لماذا التبويب الظاهر وحده:** `StatefulShellRoute.indexedStack` يُبقي
/// التبويبات الأربعة حيّة في الشجرة، فمزوّداتها كلها مشتركة ولو لم تُرَ.
/// تحديثها جميعاً كل عشر ثوانٍ يعني أربعة طلبات لكل نبضة — وأحدها
/// (`InternalEx_SelectType_View_not_coustmers_get`) يعيد كل الصفوف بلا
/// ترقيم؛ رُصد 522 صفاً. فنُحدّث ما ينظر إليه الوكيل الآن لا غير.
///
/// و`commissionsProvider` مستثنى عمداً: `CommtionRetview_get` تستغرق
/// ~8 ثوانٍ، فنبضةٌ كل عشر ثوانٍ تعني طلباً لم ينتهِ حتى يبدأ التالي.
///
/// لا وميض عند التحديث: `AsyncValue.when` يُبقي القيمة السابقة معروضة
/// أثناء إعادة الجلب (`skipLoadingOnRefresh` مفعّل افتراضياً).
class AutoRefresh extends ConsumerStatefulWidget {
  const AutoRefresh({
    super.key,
    required this.tabIndex,
    required this.child,
  });

  final int tabIndex;
  final Widget child;

  static const every = Duration(seconds: 10);

  @override
  ConsumerState<AutoRefresh> createState() => _AutoRefreshState();
}

class _AutoRefreshState extends ConsumerState<AutoRefresh>
    with WidgetsBindingObserver {
  Timer? _timer;

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addObserver(this);
    _start();
  }

  @override
  void didUpdateWidget(AutoRefresh old) {
    super.didUpdateWidget(old);
    // انتقل إلى تبويب آخر: نُحدّثه فوراً بدل انتظار عشر ثوانٍ أمام رقم قديم.
    if (old.tabIndex != widget.tabIndex) {
      _tick();
      _start();
    }
  }

  @override
  void didChangeAppLifecycleState(AppLifecycleState state) {
    if (state == AppLifecycleState.resumed) {
      // عاد الوكيل: يرى أحدث رصيد فوراً، لا بعد عشر ثوانٍ.
      _tick();
      _start();
      return;
    }
    // في الخلفية لا شاشة تُقرأ — النبض عندها استنزاف للبطارية وحزمة البيانات.
    _timer?.cancel();
  }

  void _start() {
    _timer?.cancel();
    _timer = Timer.periodic(AutoRefresh.every, (_) => _tick());
  }

  void _tick() {
    if (!mounted) return;
    switch (widget.tabIndex) {
      case 0:
        ref.invalidate(homeSnapshotProvider);
      case 1:
        // المُبطَل بلا مستمعين يُهمَل بلا طلب، فالتبويب غير المفتوح لا يكلّف.
        ref.invalidate(incomingTransfersProvider);
        ref.invalidate(deliveredTransfersProvider);
      case 2:
        ref.invalidate(posListProvider);
      // تبويب الحساب ثابت — لا شيء فيه يأتي من الخادم دورياً.
    }
  }

  @override
  void dispose() {
    _timer?.cancel();
    WidgetsBinding.instance.removeObserver(this);
    super.dispose();
  }

  @override
  Widget build(BuildContext context) => widget.child;
}
