import 'dart:io';

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';
import 'package:image_picker/image_picker.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../transfers/receipt.dart';
import 'arabic_to_latin.dart';
import 'branding_controller.dart';
import 'branding_repository.dart';

/// «هوية الشركة» — اسمها وشعارها وألوانها داخل التطبيق.
///
/// ⚠ ما لا تفعله هذه الشاشة، وهو الأهمّ: لا تمسّ رصيداً ولا حوالةً ولا
/// صلاحيةً ولا تقريراً. تخصيص الهوية طبقة عرض بقرار المالك، وأي حقل مالي
/// يظهر هنا يوماً يكون خطأً لا ميزة.
///
/// وهي متاحة للحساب الرئيسي وحده، والخادم هو من يقرّر ذلك (`can_edit`).
/// إخفاء الزرّ ليس حماية — لذلك الحفظ يُرفض في الخادم أيضاً.
class BrandingScreen extends ConsumerStatefulWidget {
  const BrandingScreen({super.key});

  @override
  ConsumerState<BrandingScreen> createState() => _BrandingScreenState();
}

class _BrandingScreenState extends ConsumerState<BrandingScreen> {
  final _nameAr = TextEditingController();
  final _nameEn = TextEditingController();

  /// قاعدة المشروع: كل حقل يكتب فيه الوكيل يأخذ [AutoClearFocus] — يُفرَغ
  /// عند دخول المؤشّر، ويُستعاد إن خرج بلا كتابة.
  late final _nameArFocus = AutoClearFocus(_nameAr, onChanged: _refresh);
  late final _nameEnFocus = AutoClearFocus(_nameEn);

  String? _themeKey;
  bool _busy = false;
  bool _seeded = false;

  /// آخر اسمٍ إنجليزيّ ولّدناه تلقائياً.
  ///
  /// يُقارَن به محتوى الحقل قبل كل تحديث: إن ساواه فالحقل ما زال تلقائياً
  /// ويجوز تحديثه، وإن خالفه فقد كتب فيه المستخدم بيده — فيُترك.
  String _enAuto = '';

  /// كتب المستخدم في الحقل الإنجليزي بنفسه، فتوقّف التوليد.
  ///
  /// مطلبٌ لا تحسين: اسم شركةٍ رسميّ مسجّل لدى مصرف ليبيا المركزي لا يجوز
  /// أن يمحوه توليدٌ آليّ لأن المستخدم صحّح حرفاً في الاسم العربي.
  bool _enManual = false;

  /// المعاينة تتبع ما يُكتب في اسم الشركة لحظةً بلحظة.
  void _refresh() {
    if (mounted) setState(() {});
  }

  @override
  void initState() {
    super.initState();
    _nameAr.addListener(_syncEnglish);
    _nameEn.addListener(_watchManualEdit);
  }

  /// ترجمة فورية بلا تدخّل: ما يُكتب بالعربية يظهر بالإنجليزية في حينه.
  void _syncEnglish() {
    if (_enManual) return;

    final next = ArabicToLatin.suggest(_nameAr.text);
    if (next == _nameEn.text) return;

    _enAuto = next;
    // `value` لا `text`: إسناد النصّ وحده يقفز بالمؤشّر إلى أوّل الحقل،
    // وهو مزعج لو كان المستخدم يقف داخله.
    _nameEn.value = TextEditingValue(
      text: next,
      selection: TextSelection.collapsed(offset: next.length),
    );
    _refresh();
  }

  void _watchManualEdit() {
    // الحقل يُفرَغ عند دخول المؤشّر (AutoClearFocus)، والفراغ ليس تدخّلاً.
    if (_nameEn.text.isEmpty) return;
    if (_nameEn.text != _enAuto) _enManual = true;
  }

  @override
  void dispose() {
    _nameAr.removeListener(_syncEnglish);
    _nameEn.removeListener(_watchManualEdit);
    _nameArFocus.dispose();
    _nameEnFocus.dispose();
    _nameAr.dispose();
    _nameEn.dispose();
    super.dispose();
  }

  /// تعبئة الحقول من الهوية المحفوظة — مرّة واحدة.
  ///
  /// إعادة التعبئة مع كل بناء تمحو ما يكتبه المستخدم في اللحظة نفسها،
  /// وهو عطبٌ يظهر ككيبورد «لا يكتب».
  void _seed(Branding b) {
    if (_seeded) return;
    _seeded = true;

    final ar = b.companyNameAr ?? '';
    final en = b.companyNameEn ?? '';

    // اسمٌ إنجليزيّ محفوظ يخالف ما نولّده = اسمٌ رسميّ كتبه أحدهم بيده.
    // يُعدّ يدويّاً منذ اللحظة الأولى فلا يمحوه التوليد.
    _enAuto = ArabicToLatin.suggest(ar);
    _enManual = en.isNotEmpty && en != _enAuto;

    _nameAr.text = ar;
    _nameEn.text = en.isNotEmpty ? en : _enAuto;
    _themeKey = b.themeKey;
  }

  @override
  Widget build(BuildContext context) {
    final st = ref.watch(brandingControllerProvider);
    final b = st.branding;
    _seed(b);

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(title: 'هوية الشركة', onBack: () => context.pop()),
          Expanded(
            child: ListView(
              padding:
                  const EdgeInsets.fromLTRB(R.padScreen, 18, R.padScreen, 40),
              children: [
                if (!b.canEdit) ...[
                  const WarnBanner(
                    text: 'تعديل هوية الشركة متاح للحساب الرئيسي فقط. '
                        'ما تراه هنا للعرض.',
                  ),
                  const SizedBox(height: 14),
                ],

                _Preview(
                  name: _nameAr.text.trim().isEmpty
                      ? b.displayName
                      : _nameAr.text.trim(),
                  logoUrl: b.logoUrl,
                  theme: _selectedTheme(b),
                ),
                const SizedBox(height: 16),

                _SectionTitle('الشعار'),
                const SizedBox(height: 10),
                _LogoCard(
                  logoUrl: b.logoUrl,
                  enabled: b.canEdit && !_busy,
                  onPick: _pickLogo,
                ),
                const SizedBox(height: 18),

                _SectionTitle('اسم الشركة'),
                const SizedBox(height: 10),
                _NameField(
                  label: 'الاسم بالعربية',
                  controller: _nameAr,
                  focusNode: _nameArFocus,
                  hint: 'شركة الرحالة للحوالات المالية',
                  enabled: b.canEdit && !_busy,
                ),
                const SizedBox(height: R.gapCard),
                _NameField(
                  label: 'الاسم بالإنجليزية',
                  controller: _nameEn,
                  focusNode: _nameEnFocus,
                  hint: 'Alrhalla Exchange',
                  ltr: true,
                  enabled: b.canEdit && !_busy,
                  // يُقال صراحةً حتى لا يظنّ المستخدم أن حقلاً يكتب نفسه عطب،
                  // وحتى يعرف أن تعديله بيده يُحترم ولا يُمحى.
                  note: _enManual
                      ? 'حرّرتَ هذا الحقل بنفسك — لن يتغيّر مع الاسم العربي.'
                      : 'يُترجَم تلقائياً من الاسم العربي. عدّله متى شئت.',
                ),
                const SizedBox(height: 18),

                // الثيم في حاوية مغلقة تُفتح عند الاختيار، لا شبكةً مفتوحة
                // دائماً (قرار المالك، 3 سبتمبر 2026): الشبكة كانت تأكل ثلث
                // الشاشة لاختيارٍ يقع مرّةً واحدة في عمر الشركة.
                //
                // ونمط الاختيار هو نمط منتقي المدينة والفرع نفسه — لا نمطٌ
                // ثانٍ يتعلّمه الوكيل.
                _ThemePicker(
                  selected: _selectedTheme(b),
                  enabled: b.canEdit && !_busy,
                  onTap: () => _pickTheme(b),
                ),
                const SizedBox(height: 22),

                if (b.canEdit) ...[
                  PrimaryButton(
                    label: 'حفظ الهوية',
                    loading: _busy,
                    onPressed: _busy ? null : _save,
                  ),
                  const SizedBox(height: 10),
                  GlassButton(
                    label: 'استعادة الهوية الافتراضية',
                    onPressed: _busy ? null : _confirmReset,
                  ),
                  const SizedBox(height: 12),
                  Text(
                    'الاستعادة تُرجع الألوان والثيم فقط — ولا تحذف اسم الشركة '
                    'ولا شعارها.',
                    textAlign: TextAlign.center,
                    style: T.plex(11.5, FontWeight.w400, color: R.inkA(.5),
                        height: 1.7),
                  ),
                ],
              ],
            ),
          ),
        ],
      ),
    );
  }

  Future<void> _pickTheme(Branding b) async {
    final picked = await showModalBottomSheet<String>(
      context: context,
      useRootNavigator: true,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (_) => _ThemeSheet(
        themes: b.themes,
        selected: _themeKey ?? b.themeKey,
      ),
    );
    if (picked != null && mounted) setState(() => _themeKey = picked);
  }

  BrandTheme? _selectedTheme(Branding b) {
    final key = _themeKey ?? b.themeKey;
    for (final t in b.themes) {
      if (t.key == key) return t;
    }
    return null;
  }

  Future<void> _pickLogo() async {
    final XFile? file;
    try {
      // تصغير الصورة قبل الرفع لا بعده: هاتفٌ حديث يلتقط صورةً بـ 4000 بكسل
      // ووزنٍ يتجاوز الحدّ، فيُرفض الرفع ولا يفهم الوكيل لماذا. و1024 كافية
      // لشعارٍ يُعرض بـ 220 بكسل على الأكثر.
      file = await ImagePicker().pickImage(
        source: ImageSource.gallery,
        maxWidth: 1024,
        maxHeight: 1024,
        imageQuality: 92,
      );
    } catch (_) {
      if (mounted) _toast('تعذّر فتح معرض الصور.');
      return;
    }
    if (file == null || !mounted) return;

    // فحص الحجم قبل الرفع: رفعُ ملفٍ كبير ثم رفضه يُهدر باقة الوكيل.
    final bytes = await File(file.path).length();
    if (bytes > 2 * 1024 * 1024) {
      if (!mounted) return;
      _toast('حجم الشعار أكبر من 2 ميغابايت — اختر صورة أصغر.');
      return;
    }

    setState(() => _busy = true);
    try {
      final b = await ref.read(brandingRepositoryProvider).uploadLogo(file.path);
      if (!mounted) return;
      ref.read(brandingControllerProvider.notifier).adopt(b);
      _toast('تم حفظ الشعار.');
    } on ApiFailure catch (e) {
      if (mounted) _toast(e.message);
    } catch (_) {
      if (mounted) _toast('تعذّر رفع الشعار — تحقّق من الاتصال.');
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  Future<void> _save() async {
    final b = ref.read(brandingControllerProvider).branding;

    // لا يُرسَل إلا ما تغيّر: إرسال الكلّ في كل حفظ يملأ سجلّ التدقيق
    // بصفوفٍ لا تدلّ على شيء، فيضيع «من غيّر ماذا» وسط الضجيج.
    final ar = _nameAr.text.trim();
    final en = _nameEn.text.trim();
    final key = _themeKey ?? b.themeKey;

    final nameAr = ar.isNotEmpty && ar != (b.companyNameAr ?? '') ? ar : null;
    final nameEn = en != (b.companyNameEn ?? '') ? en : null;
    final theme = key != b.themeKey ? key : null;

    if (nameAr == null && nameEn == null && theme == null) {
      _toast('لا يوجد تغيير للحفظ.');
      return;
    }

    setState(() => _busy = true);
    try {
      final next = await ref.read(brandingRepositoryProvider).save(
            nameAr: nameAr,
            nameEn: nameEn,
            themeKey: theme,
          );
      if (!mounted) return;
      ref.read(brandingControllerProvider.notifier).adopt(next);
      _toast('تم حفظ هوية الشركة.');
    } on ApiFailure catch (e) {
      if (mounted) _toast(e.message);
    } catch (_) {
      if (mounted) _toast('تعذّر الحفظ — تحقّق من الاتصال وأعد المحاولة.');
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  Future<void> _confirmReset() async {
    final ok = await showModalBottomSheet<bool>(
      context: context,
      useRootNavigator: true,
      backgroundColor: Colors.transparent,
      builder: (_) => const _ConfirmReset(),
    );
    if (ok != true || !mounted) return;

    setState(() => _busy = true);
    try {
      final b = await ref.read(brandingRepositoryProvider).reset();
      if (!mounted) return;
      _seeded = false; // إعادة تعبئة الحقول من الهوية المستعادة.
      ref.read(brandingControllerProvider.notifier).adopt(b);
      _toast('تمت استعادة الهوية الافتراضية.');
    } on ApiFailure catch (e) {
      if (mounted) _toast(e.message);
    } catch (_) {
      if (mounted) _toast('تعذّرت الاستعادة — تحقّق من الاتصال.');
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  void _toast(String message) {
    ScaffoldMessenger.of(context)
      ..hideCurrentSnackBar()
      ..showSnackBar(SnackBar(
        content: Text(message, style: T.plex(13, FontWeight.w500,
            color: Colors.white)),
        backgroundColor: R.inkA(.92),
        behavior: SnackBarBehavior.floating,
      ));
  }
}

/// معاينة حيّة — الترويسة كما سيراها الوكيل بعد الحفظ.
///
/// شرط المالك: أن يرى النتيجة قبل أن يعتمدها. والمعاينة تُبنى من ألوان
/// الثيم المختار لا من `R`، لأن `R` لا تتغيّر إلا بعد الحفظ فعلاً.
class _Preview extends StatelessWidget {
  const _Preview({required this.name, required this.logoUrl, required this.theme});

  final String name;
  final String? logoUrl;
  final BrandTheme? theme;

  @override
  Widget build(BuildContext context) {
    final start = theme?.secondary ?? R.primaryGradStart;
    final end = theme?.primary ?? R.primaryDark;

    return ClipRRect(
      borderRadius: BorderRadius.circular(R.rCardXl),
      child: Container(
        height: 132,
        decoration: BoxDecoration(
          gradient: LinearGradient(
            begin: const Alignment(0.5, -1),
            end: const Alignment(-0.5, 1),
            colors: [start, Color.lerp(start, end, .55)!, end],
            stops: const [0, .55, 1],
          ),
        ),
        child: Stack(
          clipBehavior: Clip.none,
          children: [
            // المعاينة تُطابق الترويسة الحقيقية حرفياً — بما في ذلك اختفاء
            // النقش الخلفي متى رُفع شعار للشركة (انظر [BrandWatermark]).
            // معاينةٌ تُظهر ما لن يظهر أسوأ من ألّا تكون هناك معاينة.
            if (logoUrl == null)
              PositionedDirectional(
                top: -26,
                end: -18,
                child: Opacity(
                  opacity: .16,
                  child: RhallaLogo(size: 130, color: R.whiteA(.56)),
                ),
              ),
            Padding(
              padding: const EdgeInsets.fromLTRB(18, 18, 18, 18),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  Text('معاينة',
                      style: T.plex(11, FontWeight.w500,
                          color: R.whiteA(.8))),
                  Text(name,
                      maxLines: 2,
                      overflow: TextOverflow.ellipsis,
                      style: T.kufi(16, FontWeight.w700, color: Colors.white)),
                  Directionality(
                    textDirection: TextDirection.ltr,
                    child: Row(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        Text('د.ل ',
                            style: T.kufi(13, FontWeight.w600,
                                color: R.whiteA(.86))),
                        Text(Fmt.money(2500),
                            style: T.kufi(20, FontWeight.w700,
                                color: Colors.white)),
                      ],
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}

/// حقل اسم — على نمط حقول نقاط البيع نفسه، لا نمطاً ثانياً.
class _NameField extends StatelessWidget {
  const _NameField({
    required this.label,
    required this.controller,
    required this.focusNode,
    required this.hint,
    required this.enabled,
    this.ltr = false,
    this.note,
  });

  final String label;
  final TextEditingController controller;
  final FocusNode focusNode;
  final String hint;
  final bool enabled;
  final bool ltr;

  /// سطر تفسير تحت الحقل — يشرح سلوكاً لا يظهر من الحقل نفسه.
  final String? note;

  @override
  Widget build(BuildContext context) {
    final field = TextField(
      controller: controller,
      focusNode: focusNode,
      enabled: enabled,
      textDirection: ltr ? TextDirection.ltr : null,
      // اسم شركةٍ يحمل نقطةً وشَرطة أحياناً («الرحالة - فرع طرابلس»)، فلا
      // يصلح فلتر الحروف والمسافات وحده؛ ويُمنع ما يفتح باب الحقن في عرضٍ
      // لاحق (`<`، `>`، الاقتباس).
      inputFormatters: [
        const WesternDigits(),
        FilteringTextInputFormatter.deny(RegExp(r'''[<>"'\\]''')),
        LengthLimitingTextInputFormatter(200),
      ],
      // الاسم الإنجليزي أطول من العربي عادةً («Al Amana Company for Financial
      // Transfers»)، فحجمٌ أصغر قليلاً يُظهره كاملاً في عرض البطاقة.
      style: ltr ? T.kufi(13, FontWeight.w600) : T.value,
      decoration: InputDecoration(
        isDense: true,
        border: InputBorder.none,
        counterText: '',
        hintText: hint,
        hintStyle: T.plex(13, FontWeight.w400, color: R.inkA(.42)),
      ),
    );

    return GlassCard(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(label, style: T.label),
          const SizedBox(height: 9),
          ltr
              ? Directionality(textDirection: TextDirection.ltr, child: field)
              : field,
          if (note != null) ...[
            const SizedBox(height: 8),
            Text(note!,
                style: T.plex(10.5, FontWeight.w400, color: R.inkA(.45),
                    height: 1.6)),
          ],
        ],
      ),
    );
  }
}

class _LogoCard extends StatelessWidget {
  const _LogoCard({
    required this.logoUrl,
    required this.enabled,
    required this.onPick,
  });

  final String? logoUrl;
  final bool enabled;
  final VoidCallback onPick;

  @override
  Widget build(BuildContext context) => GlassCard(
        padding: const EdgeInsets.fromLTRB(16, 16, 16, 16),
        child: Row(
          children: [
            Container(
              width: 64,
              height: 64,
              alignment: Alignment.center,
              decoration: BoxDecoration(
                color: R.whiteA(.7),
                borderRadius: BorderRadius.circular(R.rTile),
                border: Border.all(color: R.inkA(.08)),
              ),
              clipBehavior: Clip.antiAlias,
              child: logoUrl == null
                  ? Icon(Icons.image_outlined, size: 24, color: R.inkA(.35))
                  : Image.network(logoUrl!, fit: BoxFit.contain,
                      errorBuilder: (_, _, _) => Icon(
                          Icons.broken_image_outlined,
                          size: 24,
                          color: R.inkA(.35))),
            ),
            const SizedBox(width: 14),
            Expanded(
              child: Text(
                'PNG أو JPG أو WEBP · حتى 2 ميغابايت · لا يقلّ عن 64 بكسل',
                style: T.plex(11.5, FontWeight.w400, color: R.inkA(.55),
                    height: 1.7),
              ),
            ),
            const SizedBox(width: 10),
            Opacity(
              opacity: enabled ? 1 : .45,
              child: MiniButton(
                label: logoUrl == null ? 'اختيار' : 'تغيير',
                icon: Icons.upload_rounded,
                filled: true,
                onTap: enabled ? onPick : () {},
              ),
            ),
          ],
        ),
      );
}

class _SectionTitle extends StatelessWidget {
  const _SectionTitle(this.text);

  final String text;

  @override
  Widget build(BuildContext context) =>
      Text(text, style: T.section);
}

class _ConfirmReset extends StatelessWidget {
  const _ConfirmReset();

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(22, 22, 22, 26),
        decoration: BoxDecoration(
          color: R.whiteA(.94),
          borderRadius:
              const BorderRadius.vertical(top: Radius.circular(R.rNav)),
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            Center(
              child: Container(
                width: 44,
                height: 4,
                decoration: BoxDecoration(
                  color: R.inkA(.16),
                  borderRadius: BorderRadius.circular(99),
                ),
              ),
            ),
            const SizedBox(height: 20),
            Center(
              child: Text('استعادة الهوية الافتراضية',
                  style: T.kufi(17, FontWeight.w700)),
            ),
            const SizedBox(height: 10),
            Text(
              'تعود الألوان والثيم إلى هوية «الرحالة». اسم الشركة وشعارها '
              'يبقيان كما هما.',
              textAlign: TextAlign.center,
              style: T.plex(13, FontWeight.w500, color: R.inkA(.65),
                  height: 1.7),
            ),
            const SizedBox(height: 20),
            PrimaryButton(
              label: 'استعادة',
              onPressed: () => Navigator.of(context).pop(true),
            ),
            const SizedBox(height: 10),
            TextButton(
              onPressed: () => Navigator.of(context).pop(false),
              style: TextButton.styleFrom(minimumSize: const Size(44, 48)),
              child: Text('إلغاء',
                  style: T.plex(13, FontWeight.w500, color: R.inkA(.55))),
            ),
          ],
        ),
      );
}

/// حاوية الثيم المغلقة — تُظهر المختار وتفتح قائمةً عند الضغط.
///
/// شكلها شكل منتقي المدينة والفرع في شاشة الحوالة نفسه: عنوانٌ صغير، ثم
/// القيمة، ثم سهم لأسفل. نمطٌ واحد يتعلّمه الوكيل مرّة.
class _ThemePicker extends StatelessWidget {
  const _ThemePicker({
    required this.selected,
    required this.enabled,
    required this.onTap,
  });

  final BrandTheme? selected;
  final bool enabled;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => GlassCard(
        onTap: enabled ? onTap : null,
        child: Row(
          children: [
            if (selected != null) ...[
              _ThemeSwatch(theme: selected!, size: 34),
              const SizedBox(width: 12),
            ],
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text('الثيم', style: T.label),
                  const SizedBox(height: 6),
                  Text(
                    selected?.nameAr ?? 'اختر',
                    style: selected == null
                        ? T.plex(15, FontWeight.w600, color: R.inkA(.42))
                        : T.value,
                  ),
                ],
              ),
            ),
            Icon(Icons.keyboard_arrow_down_rounded,
                size: 22, color: R.inkA(.45)),
          ],
        ),
      );
}

/// شريط اللونين — يُري الثيم لا يصفه.
class _ThemeSwatch extends StatelessWidget {
  const _ThemeSwatch({required this.theme, required this.size});

  final BrandTheme theme;
  final double size;

  @override
  Widget build(BuildContext context) => Container(
        width: size,
        height: size,
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(size * .28),
          border: Border.all(color: R.inkA(.10)),
          gradient: LinearGradient(
            begin: Alignment.topRight,
            end: Alignment.bottomLeft,
            colors: [theme.secondary, theme.primary],
          ),
        ),
      );
}

/// قائمة الثيمات — تُفتح عند الطلب فقط.
class _ThemeSheet extends StatelessWidget {
  const _ThemeSheet({required this.themes, required this.selected});

  final List<BrandTheme> themes;
  final String selected;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.fromLTRB(18, 14, 18, 24),
        constraints: BoxConstraints(
          maxHeight: MediaQuery.sizeOf(context).height * .7,
        ),
        decoration: BoxDecoration(
          color: R.whiteA(.96),
          borderRadius:
              const BorderRadius.vertical(top: Radius.circular(R.rNav)),
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Center(
              child: Container(
                width: 44,
                height: 4,
                decoration: BoxDecoration(
                  color: R.inkA(.16),
                  borderRadius: BorderRadius.circular(99),
                ),
              ),
            ),
            const SizedBox(height: 16),
            Text('اختيار الثيم', style: T.kufi(17, FontWeight.w700)),
            const SizedBox(height: 14),
            Flexible(
              child: ListView.separated(
                shrinkWrap: true,
                itemCount: themes.length,
                separatorBuilder: (_, _) => const SizedBox(height: 8),
                itemBuilder: (_, i) {
                  final t = themes[i];
                  final on = t.key == selected;
                  return InkWell(
                    borderRadius: BorderRadius.circular(R.rRow),
                    onTap: () => Navigator.of(context).pop(t.key),
                    child: Container(
                      padding: const EdgeInsets.symmetric(
                          horizontal: 12, vertical: 12),
                      decoration: BoxDecoration(
                        color: on ? R.primaryA(.08) : Colors.transparent,
                        border: Border.all(
                          color: on ? R.primary : R.inkA(.08),
                          width: on ? 1.6 : 1,
                        ),
                        borderRadius: BorderRadius.circular(R.rRow),
                      ),
                      child: Row(
                        children: [
                          _ThemeSwatch(theme: t, size: 38),
                          const SizedBox(width: 12),
                          Expanded(
                            child: Text(t.nameAr,
                                style: T.kufi(14, FontWeight.w600)),
                          ),
                          if (on)
                            Icon(Icons.check_circle_rounded,
                                size: 20, color: R.primary),
                        ],
                      ),
                    ),
                  );
                },
              ),
            ),
          ],
        ),
      );
}
