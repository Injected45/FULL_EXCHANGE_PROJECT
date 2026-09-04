import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

/// إغلاق لوحة المفاتيح — بخطوتين لا بخطوة (قرار المالك، 4 سبتمبر 2026).
///
/// `unfocus()` وحدها لا تكفي: هي تُسقط التركيز داخل فلاتر، أما اللوحة فيُبقيها
/// النظام مفتوحة إن كان اتصال الإدخال قد بقي مفتوحاً على المنصّة — وهو ما يقع
/// حين يُنتزع الحقل من الشجرة وهو مركَّز (تبديل تبويب، دفع شاشة، إعادة بناء).
/// فتبقى اللوحة معلّقة فوق شاشةٍ لا حقل فيها وتأكل نصفها.
///
/// و`TextInput.hide` وحدها لا تكفي كذلك: تُخفي اللوحة والتركيز باقٍ، فتعود
/// عند أول إعادة بناء. فالاثنتان معاً.
void hideKeyboard() {
  FocusManager.instance.primaryFocus?.unfocus();
  SystemChannels.textInput.invokeMethod<void>('TextInput.hide');
}

/// يُغلق اللوحة عند كل انتقال بين الشاشات.
///
/// الحاجة إليه أن الحقل قد يكون في الشاشة المغادَرة: فلا لمسة في الفراغ تقع
/// ولا زرّ رجوع يُضغط — يُدفع مسارٌ من زرّ، أو يُبدَّل تبويب، فتصحب اللوحة
/// الوكيل إلى شاشةٍ لا تحتاجها.
///
/// ويُسجَّل مرّة واحدة في `GoRouter.observers`، فيغطّي كل مسارات التطبيق —
/// بديله كتابةُ الإغلاق في كل زرّ تنقّل، وواحدٌ منسيّ يعيد العطب.
class KeyboardDismisser extends NavigatorObserver {
  @override
  void didPush(Route<dynamic> route, Route<dynamic>? previousRoute) =>
      hideKeyboard();

  @override
  void didPop(Route<dynamic> route, Route<dynamic>? previousRoute) =>
      hideKeyboard();

  @override
  void didReplace({Route<dynamic>? newRoute, Route<dynamic>? oldRoute}) =>
      hideKeyboard();
}
