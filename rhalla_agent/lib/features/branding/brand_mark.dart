import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/theme/tokens.dart';
import '../../ui/widgets/glass.dart';
import 'branding_controller.dart';

/// شعار الشركة، أو شعار الرحالة حين لا شعار لها.
///
/// كل موضع يعرض شعاراً بعد الدخول يمرّ من هنا، لا من `RhallaLogo` مباشرةً:
/// موضعٌ واحد يُنسى يعني شعار الرحالة داخل تطبيق شركةٍ أخرى.
///
/// وحين يفشل تحميل الصورة — شبكةٌ منقطعة أو ملفٌ محذوف — يعود إلى شعار
/// الرحالة بدل أيقونة عطب: شاشةٌ فيها شعارٌ آخر خيرٌ من شاشةٍ فيها كسر.
class BrandMark extends ConsumerWidget {
  const BrandMark({super.key, required this.size, this.color});

  final double size;

  /// لون شعار الرحالة الاحتياطي. لا يُطبَّق على صورة الشركة — تلوين شعار
  /// شركةٍ يشوّهه.
  final Color? color;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final b = ref.watch(brandingControllerProvider).branding;
    final fallback = RhallaLogo(size: size, color: color ?? R.primary);

    if (!b.hasLogo) return fallback;

    return Image.network(
      // ملاحظة: هذه الصورة تُعرض **كما هي**، فهي تصلح على خلفية بيضاء
      // (الفاتورة، بطاقة الشعار). أما خلف ترويسة ملوّنة فانظر [BrandWatermark].
      b.logoUrl!,
      width: size,
      height: size,
      fit: BoxFit.contain,
      errorBuilder: (_, _, _) => fallback,
      // لا مؤشّر تحميل: الشعار زينةٌ في الترويسة، ودوّامةٌ مكانه تلفت النظر
      // إلى ما لا يعني الوكيل.
      loadingBuilder: (_, child, progress) =>
          progress == null ? child : SizedBox(width: size, height: size),
    );
  }
}

/// العلامة المائية خلف الترويسة الملوّنة.
///
/// **ليست [BrandMark].** الفرق ليس تنسيقاً بل صحّة عرض:
///
/// شعار «الرحالة» ملفّ SVG أحادي اللون بخلفية شفّافة، فبسطُه كبيراً بشفافية
/// خفيفة يعطي نقشاً خلف الترويسة. أما شعار الشركة فصورة نقطية يرفعها
/// المستخدم، وأغلب الشعارات تُحفظ **بخلفية بيضاء صريحة** (JPEG لا يحمل
/// شفافية أصلاً) — فبسطُها خلف ترويسة زرقاء يُظهر **مستطيلاً أبيض** لا علامة
/// مائية. وهذا ما حدث فعلاً مع شعار شركة الأمانة.
///
/// فالقاعدة هنا: النقش الخلفي زينةٌ لا هوية، ولا يُرسم إلا حين يكون الشعار
/// أحادي اللون شفّاف الخلفية. وحين ترفع الشركة شعارها **تُترك الترويسة
/// نظيفة** — لا مستطيلٌ أبيض، ولا شعار «الرحالة» داخل تطبيق شركةٍ أخرى.
/// وهوية الشركة تبقى ظاهرة في الترويسة باسمها وألوانها، وشعارها يظهر
/// واضحاً حيث يليق به: الفاتورة، وشاشة الهوية، وكشف الحساب.
///
/// وتعود العلامة المائية بشعار الشركة متى رُفع **PNG بخلفية شفّافة**.
class BrandWatermark extends ConsumerWidget {
  const BrandWatermark({super.key, required this.size});

  final double size;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final b = ref.watch(brandingControllerProvider).branding;
    if (b.hasLogo) return const SizedBox.shrink();

    // نفس شفافية التصميم الأصلي: ‎.16 × ‎.56 ≈ ‎.09
    return Opacity(
      opacity: .16,
      child: RhallaLogo(size: size, color: R.whiteA(.56)),
    );
  }
}
