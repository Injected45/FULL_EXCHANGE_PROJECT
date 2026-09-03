import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/ambient.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../auth/auth_controller.dart';
import '../favorites/favorites_repository.dart';
import '../favorites/favorites_screen.dart';
import 'send_layout.dart';
import 'send_repository.dart';


class SendInternalScreen extends ConsumerStatefulWidget {
  const SendInternalScreen({super.key, this.prefill});

  /// عميل من المفضّلة — يُملأ اسمه وهاتفه سلفاً.
  final FavoriteCustomer? prefill;

  @override
  ConsumerState<SendInternalScreen> createState() => _SendInternalScreenState();
}

class _SendInternalScreenState extends ConsumerState<SendInternalScreen> {
  final _amount = TextEditingController();
  final _name = TextEditingController();
  final _phone = TextEditingController();
  // صفر بخانتين كسائر المبالغ — قرار المالك: لا يظهر «0» عارياً في حقل مال.
  final _commission = TextEditingController(text: '0.00');
  final _notes = TextEditingController();

  // كل حقل رقمي: يُفرَغ عند دخول المؤشّر، والمبالغ تُنسَّق عند الخروج.
  late final _amountFocus = AutoClearFocus(_amount,
      onChanged: () => setState(() {}), formatOnExit: true);
  late final _commissionFocus = AutoClearFocus(_commission,
      onChanged: () => setState(() {}), formatOnExit: true);
  late final _phoneFocus = AutoClearFocus(_phone, onChanged: () {
    setState(() {});
    // الرقم الليبي تسع خانات — باكتمالها ننتقل تلقائياً بلا ضغطة.
    if (Fmt.phoneForApi(_phone.text).length == 9) _afterPhone();
  });

  /// الحقول النصّية تُفرَغ عند الدخول أيضاً — قرار المالك: «أي حقل مملوء
  /// سابقاً بمجرّد الانتقال إليه يُفرَّغ تماماً لنتمكّن من تسجيل البيان
  /// الجديد». والاسترجاع عند الخروج بلا كتابة يمنع ضياعه بلمسة عابرة.
  late final _nameFocus =
      AutoClearFocus(_name, onChanged: () => setState(() {}));
  late final _notesFocus = AutoClearFocus(_notes);

  Ref2? _city;
  Ref2? _branch;
  String? _error;

  @override
  void initState() {
    super.initState();
    _applyFavorite(widget.prefill);
  }

  /// يملأ الاسم والهاتف من عميل مفضّل. الهاتف يُوحَّد إلى تسع خانات لأن
  /// الحقل هنا محدود بها، وما يعيده الخادم قد يحمل بادئة 218.
  void _applyFavorite(FavoriteCustomer? c) {
    if (c == null) return;
    _name.text = c.name;
    _phone.text = Fmt.phoneForApi(c.phone);
    if (mounted) setState(() {});
  }

  // ── سلسلة التنقّل التلقائي ───────────────────────────────────────────
  //
  // قرار المالك: يملأ الوكيل حقلاً فينتقل إلى التالي حتى يبلغ المراجعة.
  // التنقّل يقع عند **اكتمال** الحقل لا عند كل ضغطة: الاسم والمبلغ عند
  // ضغط «التالي» في لوحة المفاتيح، والهاتف من تلقائه عند الخانة التاسعة،
  // والمنتقيان بمجرّد الاختيار.

  void _afterAmount() => _nameFocus.requestFocus();

  void _afterName() => _phoneFocus.requestFocus();

  /// بعد الهاتف: نغلق لوحة المفاتيح ونفتح منتقي المدينة مباشرةً.
  void _afterPhone() {
    // مُلئت المدينة سلفاً — لا نُقحم ورقة على الوكيل، بل نتابع السلسلة
    // إلى العمولة كما لو اختارها للتوّ.
    if (_city != null) return _commissionFocus.requestFocus();
    FocusScope.of(context).unfocus();
    // تأخير قصير: فتح ورقة أثناء انسحاب لوحة المفاتيح يجعلها تقفز.
    Future.delayed(const Duration(milliseconds: 220), () {
      if (mounted && _city == null) _pickCity();
    });
  }

  void _pickCity() => _pick(
        title: 'اختر المدينة',
        provider: citiesProvider,
        onPicked: (r) {
          setState(() => _city = r);
          _resolveBranch(r);
          // آخر حلقة قبل المراجعة: العمولة.
          _commissionFocus.requestFocus();
        },
      );

  /// يشتقّ فرع الاستلام من المدينة بلا أن يُسأل الوكيل — انظر
  /// [resolveDeliveryBranch] لسبب كل قاعدة في الاختيار.
  Future<void> _resolveBranch(Ref2 city) async {
    // انتظار القائمة لا قراءتها: الوكيل قد يختار المدينة قبل أن تصل الفروع.
    final all = await ref.read(branchesProvider.future);
    if (!mounted) return;
    final b = resolveDeliveryBranch(all, city.id);
    if (b != null) setState(() => _branch = b);
  }

  Future<void> _pickFavorite() async {
    final c = await FavoritePickerSheet.show(context, FavoriteKind.internal);
    _applyFavorite(c);
  }

  @override
  void dispose() {
    _amountFocus.dispose();
    _commissionFocus.dispose();
    _phoneFocus.dispose();
    _nameFocus.dispose();
    _notesFocus.dispose();
    for (final c in [_amount, _name, _phone, _commission, _notes]) {
      c.dispose();
    }
    super.dispose();
  }

  double get _amountValue => Fmt.num_(_amount.text);
  double get _commissionValue => Fmt.num_(_commission.text);

  bool get _valid =>
      _amountValue >= 1 &&
      // «الاسم الثلاثي» إيحاءٌ لا شرط — قرار المالك: لا يُفرض عدد محارف
      // ولا عدد كلمات. يبقى الاسم مطلوباً فقط، فحوالةٌ بلا مستفيد لا تُسلَّم.
      _name.text.trim().isNotEmpty &&
      Fmt.isValidLibyanPhone(_phone.text) &&
      _city != null &&
      _branch != null;

  void _review() {
    if (!_valid) {
      setState(() => _error = _firstProblem());
      return;
    }
    setState(() => _error = null);

    final user = ref.read(authControllerProvider).user;
    context.push('/send/internal/review', extra: TransferDraft(
      receiverName: _name.text.trim(),
      receiverPhone: Fmt.phoneForApi(_phone.text),
      amount: _amountValue,
      commission: _commissionValue,
      city: _city!,
      branch: _branch!,
      currencyId: user?.currencyId ?? 1,
      notes: _notes.text,
    ));
  }

  String _firstProblem() {
    if (_amountValue < 1) return 'أدخل مبلغاً لا يقل عن 1.';
    if (_name.text.trim().isEmpty) return 'أدخل اسم المستفيد.';
    if (!Fmt.isValidLibyanPhone(_phone.text)) {
      return 'أدخل رقماً ليبياً من 9 أرقام يبدأ بـ 9.';
    }
    if (_city == null) return 'اختر مدينة الاستلام.';
    // الفرع مخفيّ ويُشتقّ من المدينة، فبقاؤه فارغاً يعني أن قائمة الفروع
    // لم تصل بعد — والرسالة تقول ذلك بدل أن تطلب من الوكيل حقلاً لا يراه.
    return 'تعذّر تحديد وجهة الاستلام — تحقّق من الاتصال ثم أعد اختيار المدينة.';
  }

  @override
  Widget build(BuildContext context) {
    final user = ref.watch(authControllerProvider).user;
    final currency = user?.currencyCode ?? 'د.ل';
    // مراقبة الفروع رغم إخفاء منتقيها: بعد الإخفاء لم يعد أحد يراقب هذا
    // المزوّد، وهو autoDispose — فكان يبقى غير محمَّل، ويعجز _resolveBranch
    // عن إيجاد وجهة، فتقف الحوالة عند حقلٍ لا يراه الوكيل.
    ref.watch(branchesProvider);

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'حوالة محلية',
            subtitle: 'داخل ليبيا · $currency',
            onBack: () => context.pop(),
          ),

          // المبلغ في الصدارة — هو أول ما يُدخله الوكيل.
          RiseIn(
            duration: const Duration(milliseconds: 500),
            child: Padding(
              padding: const EdgeInsets.fromLTRB(26, 4, 26, 0),
              child: Column(
                children: [
                  Text('مبلغ الحوالة', style: T.label),
                  const SizedBox(height: 10),
                  // الرمز على يسار الرقم وملاصقٌ له، والاثنان في الوسط.
                  // لم ينفع هنا prefixText: الحقل يمتدّ بعرض الشاشة فيقف
                  // الرمز عند حافته اليسرى ويبتعد عن رقم قصير. الحلّ أن
                  // يتبع عرضُ الحقل ما فيه — IntrinsicWidth.
                  Directionality(
                    textDirection: TextDirection.ltr,
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Text(currency, style: T.meta),
                        const SizedBox(width: 10),
                        Flexible(
                          child: IntrinsicWidth(
                            child: ConstrainedBox(
                              // حدّ أدنى حتى يبقى الحقل قابلاً للنقر وهو فارغ.
                              constraints: const BoxConstraints(minWidth: 56),
                              child: TextField(
                                controller: _amount,
                                focusNode: _amountFocus,
                                // المؤشّر ينبض عند المبلغ فور فتح الشاشة.
                                autofocus: true,
                                textInputAction: TextInputAction.next,
                                onSubmitted: (_) => _afterAmount(),
                                onChanged: (_) => setState(() {}),
                                keyboardType:
                                    const TextInputType.numberWithOptions(
                                        decimal: true),
                                inputFormatters: moneyInputFormatters,
                                textAlign: TextAlign.center,
                                style: T.kufi(30, FontWeight.w800),
                                decoration: InputDecoration(
                                  isDense: true,
                                  border: InputBorder.none,
                                  hintText: '0.00',
                                  hintStyle: T.kufi(28, FontWeight.w800,
                                      color: R.inkA(.2)),
                                ),
                              ),
                            ),
                          ),
                        ),
                      ],
                    ),
                  ),
                  const SizedBox(height: 8),
                  Container(
                    width: 168,
                    height: 2,
                    decoration: BoxDecoration(
                      color: R.primaryA(.35),
                      borderRadius: BorderRadius.circular(99),
                    ),
                  ),
                ],
              ),
            ),
          ),

          Expanded(
            child: ListView(
              padding:
                  const EdgeInsets.fromLTRB(R.padScreen, 8, R.padScreen, 2),
              children: [
                GlassCard(
                  padding: kCardPad,
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Row(
                        children: [
                          Text('اسم المستفيد', style: T.label),
                          const Spacer(),
                          FavoriteFieldButton(onTap: _pickFavorite),
                        ],
                      ),
                      const SizedBox(height: kGapLabel),
                      TextField(
                        controller: _name,
                        focusNode: _nameFocus,
                        inputFormatters: lettersOnlyFormatters,
                        textInputAction: TextInputAction.next,
                        onSubmitted: (_) => _afterName(),
                        onChanged: (_) => setState(() {}),
                        style: T.value,
                        decoration: InputDecoration(
                          isDense: true,
                          border: InputBorder.none,
                          hintText: 'الاسم الثلاثي',
                          hintStyle: T.plex(13.5, FontWeight.w400,
                              color: R.inkA(.42)),
                        ),
                      ),
                      const SizedBox(height: kGapRule),
                      Divider(color: R.inkA(.07), height: 1),
                      const SizedBox(height: kGapRule),
                      Text('هاتف المستفيد', style: T.label),
                      const SizedBox(height: kGapLabel),
                      Directionality(
                        textDirection: TextDirection.ltr,
                        child: Row(
                          children: [
                            const LibyaFlag(),
                            const SizedBox(width: 12),
                            Text('+218', style: T.kufi(16, FontWeight.w600)),
                            const SizedBox(width: 12),
                            Container(width: 1, height: 26, color: R.inkA(.1)),
                            const SizedBox(width: 12),
                            Expanded(
                              child: TextField(
                                controller: _phone,
                                textInputAction: TextInputAction.next,
                                onSubmitted: (_) => _afterPhone(),
                                focusNode: _phoneFocus,
                                onChanged: (_) => setState(() {}),
                                keyboardType: TextInputType.number,
                                inputFormatters: [
                                  const WesternDigits(),
                                  FilteringTextInputFormatter.digitsOnly,
                                  LengthLimitingTextInputFormatter(9),
                                ],
                                style: T.kufi(17, FontWeight.w600, spacing: 1),
                                decoration: InputDecoration(
                                  isDense: true,
                                  border: InputBorder.none,
                                  counterText: '',
                                  hintText: '9XXXXXXXX',
                                  hintStyle: T.kufi(17, FontWeight.w600,
                                      color: R.inkA(.28)),
                                ),
                              ),
                            ),
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: kGap),

                // «فرع الاستلام» مخفيّ عن الوكيل — لا محذوف. يُشتقّ من هذه
                // المدينة في _resolveBranch ويُرسَل في branch_id كما كان.
                _Picker(
                  label: 'مدينة الاستلام',
                  value: _city?.name,
                  onTap: _pickCity,
                ),
                const SizedBox(height: kGap),

                _CommissionCard(
                  controller: _commission,
                  focusNode: _commissionFocus,
                  onChanged: () => setState(() {}),
                  onSubmitted: () => _notesFocus.requestFocus(),
                  currency: currency,
                ),
                const SizedBox(height: kGap),

                GlassCard(
                  padding: kCardPad,
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text('ملاحظات · اختياري', style: T.label),
                      const SizedBox(height: kGapLabel),
                      TextField(
                        controller: _notes,
                        focusNode: _notesFocus,
                        inputFormatters: lettersOnlyFormatters,
                        // سطر واحد: يسمح لـ«تمّ» بإنهاء السلسلة، ويُبقي
                        // الشاشة كلّها ظاهرة بلا تمرير.
                        maxLines: 1,
                        textInputAction: TextInputAction.done,
                        onSubmitted: (_) => FocusScope.of(context).unfocus(),
                        style: T.plex(14, FontWeight.w500, height: 1.7),
                        decoration: InputDecoration(
                          isDense: true,
                          border: InputBorder.none,
                          hintText: 'سبب الحوالة، أو أي تفصيل يخصّ المستفيد',
                          hintStyle: T.plex(13.5, FontWeight.w400,
                              color: R.inkA(.42)),
                        ),
                      ),
                    ],
                  ),
                ),
                if (_error != null) ...[
                  const SizedBox(height: 14),
                  Text(_error!,
                      style: T.plex(12, FontWeight.w500,
                          color: R.error, height: 1.5)),
                ],
              ],
            ),
          ),

          Container(
            padding:
                const EdgeInsets.fromLTRB(R.padScreen, 6, R.padScreen, 8),
            decoration: BoxDecoration(
              gradient: LinearGradient(
                begin: Alignment.topCenter,
                end: Alignment.bottomCenter,
                colors: [Color(0x00F1F8F5), Color(0xF0F1F8F5), R.scrimBottom],
                stops: [0, .34, 1],
              ),
            ),
            child: Column(
              children: [
                Padding(
                  padding: const EdgeInsets.fromLTRB(4, 0, 4, 4),
                  child: Row(
                    children: [
                      Text('الإجمالي المخصوم',
                          style: T.plex(12.5, FontWeight.w500,
                              color: R.inkA(.55))),
                      const Spacer(),
                      Directionality(
                        textDirection: TextDirection.ltr,
                        child: Row(
                          crossAxisAlignment: CrossAxisAlignment.baseline,
                          textBaseline: TextBaseline.alphabetic,
                          children: [
                            Text(currency,
                                style: T.plex(11.5, FontWeight.w400,
                                    color: R.inkA(.55))),
                            const SizedBox(width: 6),
                            Text(Fmt.money(_amountValue + _commissionValue),
                                style: T.kufi(19, FontWeight.w700)),
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
                PrimaryButton(
                  height: kButtonHeight,
                  // النصّ يتبدّل باكتمال البيانات: «مراجعة» ما دام ناقصاً،
                  // و«إرسال» حين يصير الضغط خطوةً نحو تحويل مالٍ فعليّ.
                  label: _valid ? 'إرسال الحوالة' : 'تنفيذ الحوالة',
                  onPressed: _valid ? _review : null,
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Future<void> _pick({
    required String title,
    required ProviderListenable<AsyncValue<List<Ref2>>> provider,
    required ValueChanged<Ref2> onPicked,
  }) async {
    final picked = await showModalBottomSheet<Ref2>(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (_) => _PickerSheet(title: title, provider: provider),
    );
    if (picked != null) onPicked(picked);
  }
}

class _Picker extends StatelessWidget {
  const _Picker({required this.label, required this.value, required this.onTap});

  final String label;
  final String? value;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => GlassCard(
        padding: kCardPad,
        onTap: onTap,
        child: Row(
          children: [
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(label, style: T.label),
                  const SizedBox(height: kGapLabel),
                  Text(
                    value ?? 'اختر',
                    style: value == null
                        ? T.plex(15, FontWeight.w600, color: R.inkA(.42))
                        : T.value,
                  ),
                ],
              ),
            ),
            Icon(Icons.keyboard_arrow_down_rounded, size: 22, color: R.inkA(.45)),
          ],
        ),
      );
}

class _CommissionCard extends StatelessWidget {
  const _CommissionCard({
    required this.controller,
    required this.focusNode,
    required this.onSubmitted,
    required this.onChanged,
    required this.currency,
  });

  final TextEditingController controller;
  final FocusNode focusNode;

  /// إتمام السلسلة: «التالي» في لوحة المفاتيح ينقل من العمولة إلى الملاحظات.
  final VoidCallback onSubmitted;
  final VoidCallback onChanged;
  final String currency;

  @override
  Widget build(BuildContext context) => Container(
        padding: kCardPad,
        decoration: BoxDecoration(
          color: R.primaryA(.07),
          border: Border.all(color: R.primaryA(.18)),
          borderRadius: BorderRadius.circular(R.rCard),
        ),
        // صفّ واحد لا صفّان: العمولة رقم صغير، وحاويةٌ بارتفاع حقلٍ كامل
        // من أجله تترك فراغاً يشوّه الصفّ ويزيح ما بعده.
        child: Row(
          children: [
            Text('العمولة', style: T.label),
            const Spacer(),
            Directionality(
              textDirection: TextDirection.ltr,
              child: SizedBox(
                // عرضٌ يكفي «د.ل  99,999.00» ولا يزيد.
                width: 152,
                child: TextField(
                  controller: controller,
                  focusNode: focusNode,
                  onChanged: (_) => onChanged(),
                  textInputAction: TextInputAction.next,
                  onSubmitted: (_) => onSubmitted(),
                  keyboardType:
                      const TextInputType.numberWithOptions(decimal: true),
                  inputFormatters: moneyInputFormatters,
                  style: T.kufi(16, FontWeight.w600),
                  decoration: InputDecoration(
                    isDense: true,
                    contentPadding: EdgeInsets.zero,
                    border: InputBorder.none,
                    // مسافة لاصقة بالرمز — بدونها يلتصق «د.ل» بالرقم: د.ل0
                    prefixText: '$currency  ',
                    prefixStyle:
                        T.plex(12, FontWeight.w400, color: R.inkA(.5)),
                  ),
                ),
              ),
            ),
          ],
        ),
      );
}

class _PickerSheet extends ConsumerStatefulWidget {
  const _PickerSheet({required this.title, required this.provider});

  final String title;
  final ProviderListenable<AsyncValue<List<Ref2>>> provider;

  @override
  ConsumerState<_PickerSheet> createState() => _PickerSheetState();
}

class _PickerSheetState extends ConsumerState<_PickerSheet> {
  String _q = '';

  @override
  Widget build(BuildContext context) {
    final async = ref.watch(widget.provider);

    return Container(
      height: MediaQuery.sizeOf(context).height * .8,
      padding: const EdgeInsets.fromLTRB(22, 22, 22, 0),
      decoration: BoxDecoration(
        color: R.whiteA(.96),
        borderRadius: const BorderRadius.vertical(top: Radius.circular(R.rNav)),
      ),
      child: Column(
        children: [
          Container(
            width: 44,
            height: 4,
            decoration: BoxDecoration(
              color: R.inkA(.16),
              borderRadius: BorderRadius.circular(99),
            ),
          ),
          const SizedBox(height: 20),
          Text(widget.title, style: T.kufi(17, FontWeight.w600)),
          const SizedBox(height: 16),
          TextField(
            autofocus: true,
            onChanged: (v) => setState(() => _q = v.trim()),
            style: T.value,
            decoration: InputDecoration(
              isDense: true,
              filled: true,
              fillColor: R.inkA(.05),
              prefixIcon: Icon(Icons.search_rounded, size: 20, color: R.inkA(.5)),
              hintText: 'ابحث',
              hintStyle: T.plex(13.5, FontWeight.w400, color: R.inkA(.42)),
              border: OutlineInputBorder(
                borderRadius: BorderRadius.circular(R.rRow),
                borderSide: BorderSide.none,
              ),
            ),
          ),
          const SizedBox(height: 14),
          Expanded(
            child: async.when(
              loading: () =>
                  Center(child: CircularProgressIndicator(color: R.primary)),
              error: (e, _) => Center(
                child: Text('$e',
                    textAlign: TextAlign.center,
                    style: T.plex(12.5, FontWeight.w500, color: R.errorText)),
              ),
              data: (list) {
                final rows = _q.isEmpty
                    ? list
                    : list.where((r) => r.name.contains(_q)).toList();
                if (rows.isEmpty) {
                  return Center(
                    child: Text('لا نتائج',
                        style: T.kufi(15, FontWeight.w600)),
                  );
                }
                return ListView.separated(
                  padding: const EdgeInsets.only(bottom: 24),
                  itemCount: rows.length,
                  separatorBuilder: (_, _) => Divider(
                      color: R.inkA(.07), height: 1),
                  itemBuilder: (_, i) => InkWell(
                    onTap: () => Navigator.of(context).pop(rows[i]),
                    child: Container(
                      constraints: const BoxConstraints(minHeight: 52),
                      alignment: AlignmentDirectional.centerStart,
                      child: Text(rows[i].name, style: T.value),
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
}
