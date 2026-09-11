import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/ambient.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../auth/auth_controller.dart';
import '../employee_app/employee_session.dart';
import '../favorites/favorites_repository.dart';
import '../favorites/favorites_screen.dart';
import 'external_repository.dart';
import 'send_repository.dart';

class SendExternalScreen extends ConsumerStatefulWidget {
  const SendExternalScreen({super.key, this.prefill});

  /// عميل من المفضّلة — يُملأ اسمه وهاتفه سلفاً.
  final FavoriteCustomer? prefill;

  @override
  ConsumerState<SendExternalScreen> createState() => _SendExternalScreenState();
}

class _SendExternalScreenState extends ConsumerState<SendExternalScreen> {
  final _amount = TextEditingController();
  final _name = TextEditingController();
  final _phone = TextEditingController();
  // صفر بخانتين كسائر المبالغ — قرار المالك: لا يظهر «0» عارياً في حقل مال.
  final _commission = TextEditingController(text: '0.00');

  Ref2? _country;
  Ref2? _city;
  Ref2? _branch;
  ServiceType? _service;
  int _deliveredCurrency = 0;

  ExternalQuote? _quote;
  bool _quoting = false;
  bool _sending = false;
  String? _error;

  /// يملأ الاسم والهاتف من عميل مفضّل. الرقم هنا أجنبي، فلا يُوحَّد بصيغة
  /// ليبية — يُنظَّف من غير الأرقام فقط، مطابقةً لمرشِّح الحقل.
  void _applyFavorite(FavoriteCustomer? c) {
    if (c == null) return;
    _name.text = c.name;
    _phone.text = c.phone.replaceAll(RegExp(r'\D'), '');
    if (mounted) setState(() {});
  }

  Future<void> _pickFavorite() async {
    final c = await FavoritePickerSheet.show(context, FavoriteKind.external);
    _applyFavorite(c);
  }

  @override
  void initState() {
    super.initState();
    _applyFavorite(widget.prefill);
    // بلد واحد اليوم (مصر) — نختاره تلقائياً بدل إجبار المستخدم على خطوة صورية.
    Future.microtask(() async {
      final list = await ref.read(serviceCountriesProvider.future);
      if (!mounted || list.length != 1) return;
      await _setCountry(list.first);
    });
  }

  // كل حقل رقمي: يُفرَغ عند دخول المؤشّر، والمبالغ تُنسَّق عند الخروج.
  late final _amountFocus = AutoClearFocus(_amount,
      onChanged: () => setState(() => _quote = null), formatOnExit: true);
  late final _commissionFocus = AutoClearFocus(_commission,
      onChanged: () => setState(() {}), formatOnExit: true);
  late final _phoneFocus =
      AutoClearFocus(_phone, onChanged: () => setState(() {}));
  /// الحقول النصّية تُفرَغ عند الدخول أيضاً — قرار المالك.
  late final _nameFocus =
      AutoClearFocus(_name, onChanged: () => setState(() {}));

  @override
  void dispose() {
    _amountFocus.dispose();
    _commissionFocus.dispose();
    _phoneFocus.dispose();
    _nameFocus.dispose();
    for (final c in [_amount, _name, _phone, _commission]) {
      c.dispose();
    }
    super.dispose();
  }

  Future<void> _setCountry(Ref2 c) async {
    final cur = await ref.read(externalRepositoryProvider).defaultCurrencyOf(c.id);
    if (!mounted) return;
    setState(() {
      _country = c;
      _city = null;
      _service = null;
      _deliveredCurrency = cur;
      _quote = null;
    });
  }

  /// فرعُ الوكيل نفسِه من قائمة الفروع — أو `null` قبل وصولها.
  ///
  /// ⚠ يُطابَق بالمعرّف لا بالاسم: الأسماءُ تتشابه وتُحرَّر في القاعدة،
  /// والمعرّفُ عقدٌ ثابت.
  ///
  /// ⚠ وفي وضع الموظف لا حسابَ وكيلٍ في الجلسة، فيبقى `null` وتُعرض شرطةٌ
  /// مكانَه — والخادمُ يملأ الفرعَ من جلسة الوكيل الذي ينفّذ باسمه على أيّ
  /// حال، فلا شيءَ ينقص الحوالة.
  Ref2? _ownBranch(List<Ref2>? branches) {
    if (branches == null || branches.isEmpty) return null;
    final id = ref.read(authControllerProvider).user?.branchId;
    if (id == null || id == 0) return null;
    for (final b in branches) {
      if (b.id == id) return b;
    }
    return null;
  }

  double get _amountValue => Fmt.num_(_amount.text);
  double get _commissionValue => Fmt.num_(_commission.text);

  bool get _valid =>
      _amountValue > 0 &&
      // «الاسم الثلاثي» إيحاءٌ لا شرط — قرار المالك: لا يُفرض عدد محارف
      // ولا عدد كلمات. يبقى الاسم مطلوباً فقط.
      _name.text.trim().isNotEmpty &&
      _phone.text.trim().length >= 6 &&
      _country != null &&
      _city != null &&
      /*
       * ⚠ الفرعُ لم يعد شرطاً في النموذج: لم يعد يُختار.
       *
       * الخادمُ يملؤه من جلسة المُرسِل ويدهس ما يصله، فاشتراطُ اختيارٍ لم
       * يعد موجوداً كان سيُبقي زرَّ الإرسال معطّلاً إلى الأبد — نموذجٌ
       * مكتملٌ لا يُرسَل ولا يقول لماذا.
       */
      _service != null;

  Future<void> _refreshQuote() async {
    if (_service == null || _country == null || _amountValue <= 0) return;
    setState(() {
      _quoting = true;
      _error = null;
    });
    try {
      final q = await ref.read(externalRepositoryProvider).quote(
            countryIdTo: _country!.id,
            amount: _amountValue,
            serviceType: _service!.id,
          );
      if (mounted) setState(() => _quote = q);
    } on ApiFailure catch (e) {
      if (mounted) setState(() => _error = e.message);
    } finally {
      if (mounted) setState(() => _quoting = false);
    }
  }

  /*
   * ══════════════════════════════════════════════════════════════════════
   *  ⚠ لم يعد هذا الزرُّ يُنشئ الحوالة — أمرُ المالك (11 سبتمبر 2026)
   * ══════════════════════════════════════════════════════════════════════
   *
   * «ننشئ شاشةَ مراجعةٍ للحوالة الخارجية لنتمكّن من إرسال الرمز والتحقّق
   *  **بنفس آليّة الداخلية**».
   *
   * فصار يبني المسودّة ويدفع [ReviewExternalScreen] — كما يفعل زرُّ الحوالة
   * المحلّية حرفاً بحرف. والإنشاءُ يقع هناك **بعد قبول الخادم للرمز**.
   *
   * ⚠ ولا يُمَسّ بذلك مالٌ ولا قيد: نداءُ الإنشاء هو هو بمعاملاته كما هي،
   * انتقل موضعُه فقط. ولا استعلامَ جديداً ولا جدولَ ولا عمود.
   *
   * ⚠ و`_sending` بقيت هنا: الشاشةُ تُقفل أزرارَها أثناء بناء المسودّة
   * والانتقال، فضغطتان متسارعتان لا تفتحان شاشتَي مراجعة.
   */
  Future<void> _send() async {
    // حارسٌ صريح لا يتّكل على تعطيل الزرّ: ضغطةٌ مزدوجة أو حدثٌ مكرّر كان
    // سيفتح شاشتَي مراجعة، ولكلٍّ منهما مفتاحُ طلبٍ مستقلّ.
    if (_sending) return;
    if (!_valid) {
      setState(() => _error = 'أكمل بيانات الحوالة أولاً.');
      return;
    }

    /*
     * ⚠ **حسابُ الوكيل غائبٌ في وضع الموظف — ولا يجوز أن يوقف الإرسال.**
     *
     * جلسةُ الموظف ليست جلسةَ وكيل، فـ`authControllerProvider.user` فارغٌ
     * عنده. و`if (user == null) return;` كانت ستجعل الزرَّ **يُضغط فلا يقع
     * شيء** — والقاعدةُ في هذا المشروع أنّ حارساً يصمت أسوأُ من حارسٍ يرفض.
     *
     * ⚠ ولا يضيع بذلك شيء: `AccFrom` **يُدهَس في الخادم** بحساب الجلسة قبل
     * أن يُقرأ.
     */
    final asEmployee =
        ref.read(employeeAuthProvider).status == EmpSessionStatus.signedIn;
    if (ref.read(authControllerProvider).user == null && !asEmployee) return;

    setState(() {
      _sending = true;
      _error = null;
    });

    final draft = ExternalDraft(
      country: _country!,
      city: _city!,
      /*
       * ⚠ فرعُ الوكيل إن عُرف، وإلّا فصفر — والخادمُ يدهسه في الحالتين
       * بفرع الجلسة (انظر `transInsertExternal`). فهذه قيمةٌ للعرض في
       * المسودّة لا قيمةٌ تُقرَّر بها الحوالة.
       */
      branch: _ownBranch(ref.read(branchesProvider).valueOrNull) ??
          _branch ??
          const Ref2(0, ''),
      service: _service!,
      receiverName: _name.text.trim(),
      receiverPhone: _phone.text.trim(),
      amountLyd: _amountValue,
      commission: _commissionValue,
      deliveredCurrencyId: _deliveredCurrency,
      quote: _quote,
    );

    // ⚠ يُفكّ التجميد عند العودة من المراجعة («تعديل البيانات»)، وإلّا عاد
    // الوكيلُ إلى نموذجٍ مجمَّدٍ لا يقول لماذا.
    await context.push('/send/external/review', extra: draft);
    if (mounted) setState(() => _sending = false);
  }

  @override
  Widget build(BuildContext context) {
    final user = ref.watch(authControllerProvider).user;
    final currency = user?.currencyCode ?? 'د.ل';
    final countries = ref.watch(serviceCountriesProvider);

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'حوالة خارجية',
            subtitle: 'خارج ليبيا · المبلغ بالـ$currency',
            onBack: _sending ? null : () => context.pop(),
            // بابُ «أسعار العملات» — هنا لأنّ السؤال يُسأل هنا: الوكيلُ
            // يسعّر لزبونٍ واقفٍ أمامه فيحتاج اللوحةَ في اللحظة نفسِها.
            //
            // ⚠ ولا يُسحب منها رقمٌ إلى هذه الشاشة: التسعيرُ يبقى من
            // `externalQuote` وحدَه، واللوحةُ عرضٌ لا مصدرَ حساب.
            trailing: CircleIconButton(
              onPressed: _sending ? null : () => context.push('/rates'),
              child: Icon(Icons.currency_exchange_rounded,
                  size: 18, color: R.ink),
            ),
          ),
          Expanded(
            child: countries.when(
              loading: () =>
                  Center(child: CircularProgressIndicator(color: R.primary)),
              error: (e, _) => _Failed(
                message: '$e',
                onRetry: () => ref.invalidate(serviceCountriesProvider),
              ),
              data: (list) {
                if (list.isEmpty) return const _NoDestinations();
                return _form(list, currency);
              },
            ),
          ),
        ],
      ),
    );
  }

  Widget _form(List<Ref2> countries, String currency) => ListView(
        padding: const EdgeInsets.fromLTRB(R.padScreen, 20, R.padScreen, 30),
        children: [
          _PickerCard(
            label: 'دولة الوجهة',
            value: _country?.name,
            onTap: () => _pickFrom(
              'اختر الدولة',
              countries,
              (r) => _setCountry(r),
            ),
          ),
          const SizedBox(height: R.gapCard),

          if (_country != null) ...[
            Consumer(builder: (_, r, _) {
              final cities = r.watch(destCitiesProvider(_country!.id));
              return _PickerCard(
                label: 'مدينة الاستلام',
                value: _city?.name,
                loading: cities.isLoading,
                onTap: () => _pickFrom('اختر المدينة',
                    cities.valueOrNull ?? const [], (v) => setState(() => _city = v)),
              );
            }),
            const SizedBox(height: R.gapCard),

            Consumer(builder: (_, r, _) {
              final services = r.watch(servicesProvider(_country!.id));
              return _ServiceChips(
                async: services,
                selected: _service,
                onPick: (s) {
                  setState(() => _service = s);
                  _refreshQuote();
                },
              );
            }),
            const SizedBox(height: R.gapCard),
          ],

          /*
           * ══════════════════════════════════════════════════════════════
           *  ⚠⚠ الفرعُ المُصدِّر يُعرض ولا يُختار — بلاغُ المالك (11 سبتمبر 2026)
           * ══════════════════════════════════════════════════════════════
           *
           * «أرسلتُ حوالاتٍ خارجية ولم تظهر في التطبيق كصادرة».
           *
           * وسببُه أنّ هذه القائمة كانت تفتح **كلَّ الفروع**، فيختار الوكيلُ
           * منها فرعاً غيرَ فرعه. و`RecievedBranchID` ليس حقلَ عرض: به
           * تُحسم — عند اعتماد الحوالة — الجهةُ التي يُقيَّد عليها القيد.
           * وقيسَ ذلك على الحوالات الحيّة: خمسٌ بفرعٍ آخر قُيّدت على الحساب
           * 272 فلم تظهر لصاحبها، وواحدةٌ بفرعه قُيّدت على حسابه 530 فظهرت.
           *
           * ⚠ **والخادمُ هو الحارس** — يدهس ما يصله بفرع الجلسة — وهذا
           * العرضُ صدقٌ في الواجهة لا حماية: بابٌ يُترك مفتوحاً على خيارٍ
           * لا أثرَ له يُعلّم المستخدم أنّ اختيارَه يعني شيئاً وهو لا يعني.
           *
           * ⚠ **ولم يُحذف الحقل**: يبقى ظاهراً باسمه وقيمته — فرعُ الوكيل
           * نفسِه — لأنه بيانٌ يقرؤه قبل الإرسال ويُطبع في الفاتورة. تغيّر
           * أنه لم يعد بابَ خطأ.
           */
          Consumer(builder: (_, r, _) {
            final branches = r.watch(branchesProvider);
            final mine = _ownBranch(branches.valueOrNull);
            return _PickerCard(
              label: 'الفرع المُصدِّر',
              value: mine?.name ?? _branch?.name,
              loading: branches.isLoading,
              // بلا `onTap`: يُعرض ولا يُفتح.
              onTap: null,
            );
          }),
          const SizedBox(height: R.gapCard),

          GlassCard(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text('المبلغ المستلَم من المرسل', style: T.label),
                const SizedBox(height: 9),
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: TextField(
                    controller: _amount,
                    focusNode: _amountFocus,
                    onChanged: (_) => setState(() => _quote = null),
                    onEditingComplete: _refreshQuote,
                    keyboardType:
                        const TextInputType.numberWithOptions(decimal: true),
                    inputFormatters: moneyInputFormatters,
                    style: T.kufi(24, FontWeight.w700),
                    decoration: InputDecoration(
                      isDense: true,
                      border: InputBorder.none,
                      hintText: '0.00',
                      hintStyle: T.kufi(24, FontWeight.w700, color: R.inkA(.22)),
                      prefixText: '$currency  ',
                      prefixStyle:
                          T.plex(12, FontWeight.w400, color: R.inkA(.5)),
                    ),
                  ),
                ),
              ],
            ),
          ),
          const SizedBox(height: R.gapCard),

          _QuoteCard(
            quote: _quote,
            loading: _quoting,
            onRefresh: _refreshQuote,
            enabled: _service != null && _country != null && _amountValue > 0,
          ),
          const SizedBox(height: R.gapCard),

          GlassCard(
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
                const SizedBox(height: 9),
                TextField(
                  controller: _name,
                  focusNode: _nameFocus,
                  inputFormatters: lettersOnlyFormatters,
                  onChanged: (_) => setState(() {}),
                  style: T.value,
                  decoration: InputDecoration(
                    isDense: true,
                    border: InputBorder.none,
                    hintText: 'الاسم كما في وثيقته',
                    hintStyle:
                        T.plex(13.5, FontWeight.w400, color: R.inkA(.42)),
                  ),
                ),
                const SizedBox(height: 14),
                Divider(color: R.inkA(.07), height: 1),
                const SizedBox(height: 14),
                Text('هاتف المستفيد', style: T.label),
                const SizedBox(height: 9),
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: TextField(
                    controller: _phone,
                    focusNode: _phoneFocus,
                    onChanged: (_) => setState(() {}),
                    keyboardType: TextInputType.phone,
                    // كان بلا مرشِّح إطلاقاً فيقبل الحروف والرموز —
                    // keyboardType يقترح لوحة أرقام ولا يمنع شيئاً.
                    inputFormatters: [
                      const WesternDigits(),
                      FilteringTextInputFormatter.digitsOnly,
                    ],
                    style: T.kufi(16, FontWeight.w600),
                    decoration: InputDecoration(
                      isDense: true,
                      border: InputBorder.none,
                      // ليس رقماً ليبياً — لا نفرض صيغة 9XXXXXXXX هنا.
                      hintText: 'رقم بلد الوجهة',
                      hintStyle:
                          T.plex(13.5, FontWeight.w400, color: R.inkA(.42)),
                    ),
                  ),
                ),
              ],
            ),
          ),
          const SizedBox(height: R.gapCard),

          Container(
            padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 16),
            decoration: BoxDecoration(
              color: R.primaryA(.07),
              border: Border.all(color: R.primaryA(.18)),
              borderRadius: BorderRadius.circular(R.rCard),
            ),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Row(
                  children: [
                    Text('العمولة', style: T.label),
                    const SizedBox(width: 7),
                    Container(
                      padding: const EdgeInsets.symmetric(
                          horizontal: 9, vertical: 4),
                      decoration: BoxDecoration(
                        color: R.primaryA(.16),
                        borderRadius: BorderRadius.circular(99),
                      ),
                      child: Text('تحدّدها أنت',
                          style: T.plex(11, FontWeight.w500,
                              color: R.primaryDark)),
                    ),
                  ],
                ),
                const SizedBox(height: 9),
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: TextField(
                    controller: _commission,
                    focusNode: _commissionFocus,
                    onChanged: (_) => setState(() {}),
                    keyboardType:
                        const TextInputType.numberWithOptions(decimal: true),
                    inputFormatters: moneyInputFormatters,
                    style: T.kufi(16, FontWeight.w600),
                    decoration: InputDecoration(
                      isDense: true,
                      border: InputBorder.none,
                      prefixText: '$currency  ',
                      prefixStyle:
                          T.plex(12, FontWeight.w400, color: R.inkA(.5)),
                    ),
                  ),
                ),
              ],
            ),
          ),

          if (_error != null) ...[
            const SizedBox(height: 14),
            Text(_error!,
                style:
                    T.plex(12, FontWeight.w500, color: R.error, height: 1.5)),
          ],

          const SizedBox(height: 20),
          Row(
            children: [
              Text('الإجمالي المخصوم',
                  style:
                      T.plex(12.5, FontWeight.w500, color: R.inkA(.55))),
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
          const SizedBox(height: 14),
          const WarnBanner(
            text:
                'بعد الإرسال لا يمكن تعديل الحوالة — إلغاؤها يتطلّب مراجعة الفرع.',
          ),
          const SizedBox(height: 16),
          PrimaryButton(
            label: 'تأكيد وإرسال',
            loading: _sending,
            onPressed: _valid && !_sending ? _send : null,
          ),
        ],
      );

  Future<void> _pickFrom(
    String title,
    List<Ref2> list,
    ValueChanged<Ref2> onPicked,
  ) async {
    if (list.isEmpty) return;
    final picked = await showModalBottomSheet<Ref2>(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (_) => _SimplePicker(title: title, items: list),
    );
    if (picked != null) onPicked(picked);
  }
}

class _PickerCard extends StatelessWidget {
  const _PickerCard({
    required this.label,
    required this.value,
    required this.onTap,
    this.loading = false,
  });

  final String label;
  final String? value;

  /// `null` يعني **بيانٌ يُعرض لا خيارٌ يُفتح** — فيسقط معه سهمُ القائمة.
  final VoidCallback? onTap;
  final bool loading;

  @override
  Widget build(BuildContext context) => GlassCard(
        onTap: loading ? null : onTap,
        child: Row(
          children: [
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(label, style: T.label),
                  const SizedBox(height: 9),
                  Text(
                    loading
                        ? 'جارٍ التحميل…'
                        : (value ?? (onTap == null ? '—' : 'اختر')),
                    style: value == null
                        ? T.plex(15, FontWeight.w600, color: R.inkA(.42))
                        : T.value,
                  ),
                ],
              ),
            ),
            // ⚠ السهمُ يَعِد بقائمةٍ تُفتح. وحقلٌ يُعرض ولا يُفتح ومعه سهمٌ
            // يُنقَر بلا أثر يُقرأ عطباً في التطبيق.
            if (onTap != null)
              Icon(Icons.keyboard_arrow_down_rounded,
                  size: 22, color: R.inkA(.45)),
          ],
        ),
      );
}

class _ServiceChips extends StatelessWidget {
  const _ServiceChips({
    required this.async,
    required this.selected,
    required this.onPick,
  });

  final AsyncValue<List<ServiceType>> async;
  final ServiceType? selected;
  final ValueChanged<ServiceType> onPick;

  @override
  Widget build(BuildContext context) => Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Padding(
            padding: const EdgeInsets.only(right: 4, bottom: 10),
            child: Text('نوع الخدمة', style: T.label),
          ),
          async.when(
            loading: () => GlassCard(
              child: SizedBox(
                height: 20,
                child: Align(
                  alignment: AlignmentDirectional.centerStart,
                  child: Text('جارٍ التحميل…', style: T.meta),
                ),
              ),
            ),
            error: (e, _) => GlassCard(
              child: Text('$e',
                  style: T.plex(12, FontWeight.w500, color: R.errorText)),
            ),
            data: (list) => list.isEmpty
                ? GlassCard(
                    child: Text('لا توجد خدمات لهذه الدولة',
                        style: T.plex(13, FontWeight.w500, color: R.inkA(.55))),
                  )
                : Wrap(
                    spacing: 8,
                    runSpacing: 8,
                    children: [
                      for (final s in list)
                        _Chip(
                          label: s.name,
                          on: s.id == selected?.id,
                          onTap: () => onPick(s),
                        ),
                    ],
                  ),
          ),
        ],
      );
}

class _Chip extends StatelessWidget {
  const _Chip({required this.label, required this.on, required this.onTap});

  final String label;
  final bool on;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => Material(
        color: on ? Colors.transparent : R.whiteA(.7),
        borderRadius: BorderRadius.circular(99),
        child: InkWell(
          borderRadius: BorderRadius.circular(99),
          onTap: onTap,
          child: Ink(
            height: 44,
            decoration: BoxDecoration(
              gradient: on ? R.primaryGradient : null,
              border: on ? null : Border.all(color: R.whiteA(.9)),
              borderRadius: BorderRadius.circular(99),
            ),
            child: Center(
              // widthFactor: 1 وإلا أخذ Center أقصى عرض في Wrap فامتدت الرقاقة.
              widthFactor: 1,
              child: Padding(
                padding: const EdgeInsets.symmetric(horizontal: 18),
                child: Text(label,
                    style: T.kufi(12, FontWeight.w600,
                        color: on ? Colors.white : R.inkA(.6))),
              ),
            ),
          ),
        ),
      );
}

/// تسعيرة الخادم — نفس أرقام المُشغِّل، فما يظهر هنا هو ما يُكتب في الحوالة.
class _QuoteCard extends StatelessWidget {
  const _QuoteCard({
    required this.quote,
    required this.loading,
    required this.onRefresh,
    required this.enabled,
  });

  final ExternalQuote? quote;
  final bool loading;
  final VoidCallback onRefresh;
  final bool enabled;

  @override
  Widget build(BuildContext context) {
    final q = quote;

    if (q == null) {
      return GlassCard(
        child: Row(
          children: [
            Text('ما يستلمه المستفيد', style: T.label),
            const Spacer(),
            if (loading)
              SizedBox(
                width: 16,
                height: 16,
                child:
                    CircularProgressIndicator(strokeWidth: 2, color: R.primary),
              )
            else
              TextButton(
                onPressed: enabled ? onRefresh : null,
                style: TextButton.styleFrom(minimumSize: const Size(44, 44)),
                child: Text('احسب',
                    style: T.plex(12.5, FontWeight.w600,
                        color: enabled ? R.primaryGradEnd : R.inkA(.35))),
              ),
          ],
        ),
      );
    }

    return GlassCard(
      sheen: true,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Text('ما يستلمه المستفيد', style: T.label),
              const Spacer(),
              if (loading)
                SizedBox(
                  width: 14,
                  height: 14,
                  child: CircularProgressIndicator(
                      strokeWidth: 2, color: R.primary),
                ),
            ],
          ),
          const SizedBox(height: 10),
          Directionality(
            textDirection: TextDirection.ltr,
            child: Row(
              crossAxisAlignment: CrossAxisAlignment.baseline,
              textBaseline: TextBaseline.alphabetic,
              children: [
                Text(Fmt.money(q.net), style: T.kufi(26, FontWeight.w800)),
                const SizedBox(width: 7),
                Text(q.currencyCode,
                    style: T.plex(12.5, FontWeight.w500, color: R.inkA(.55))),
              ],
            ),
          ),
          const SizedBox(height: 14),
          Divider(color: R.inkA(.07), height: 1),
          const SizedBox(height: 12),
          // الوحدة نسبة لا مبلغ — «ج.م» وحدها تقرأ كأنّ السعر مبلغ بالجنيه.
          _line('سعر الصرف', Fmt.rate(q.rate), '${q.currencyCode} / د.ل'),
          const SizedBox(height: 8),
          _line('قبل الرسوم', Fmt.money(q.delivered), q.currencyCode),
          const SizedBox(height: 8),
          _line('رسوم الخدمة', Fmt.money(q.serviceFee), q.currencyCode),
          const SizedBox(height: 12),
          Text('التسعيرة لحظية — سعر الصرف قد يتغيّر قبل التنفيذ.',
              style: T.plex(10.5, FontWeight.w400,
                  color: R.inkA(.45), height: 1.6)),
        ],
      ),
    );
  }

  static Widget _line(String label, String value, String currency) => Row(
        children: [
          Text(label,
              style: T.plex(11.5, FontWeight.w400, color: R.inkA(.55))),
          const Spacer(),
          Directionality(
            textDirection: TextDirection.ltr,
            child: Row(
              crossAxisAlignment: CrossAxisAlignment.baseline,
              textBaseline: TextBaseline.alphabetic,
              children: [
                Text(currency,
                    style: T.plex(10, FontWeight.w400, color: R.inkA(.45))),
                const SizedBox(width: 5),
                Text(value, style: T.kufi(12.5, FontWeight.w600)),
              ],
            ),
          ),
        ],
      );
}

class _SimplePicker extends StatefulWidget {
  const _SimplePicker({required this.title, required this.items});

  final String title;
  final List<Ref2> items;

  @override
  State<_SimplePicker> createState() => _SimplePickerState();
}

class _SimplePickerState extends State<_SimplePicker> {
  String _q = '';

  @override
  Widget build(BuildContext context) {
    final rows = _q.isEmpty
        ? widget.items
        : widget.items.where((r) => r.name.contains(_q)).toList();

    return Container(
      height: MediaQuery.sizeOf(context).height * .7,
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
            onChanged: (v) => setState(() => _q = v.trim()),
            style: T.value,
            decoration: InputDecoration(
              isDense: true,
              filled: true,
              fillColor: R.inkA(.05),
              prefixIcon:
                  Icon(Icons.search_rounded, size: 20, color: R.inkA(.5)),
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
            child: ListView.separated(
              padding: const EdgeInsets.only(bottom: 24),
              itemCount: rows.length,
              separatorBuilder: (_, _) =>
                  Divider(color: R.inkA(.07), height: 1),
              itemBuilder: (_, i) => InkWell(
                onTap: () => Navigator.of(context).pop(rows[i]),
                child: Container(
                  constraints: const BoxConstraints(minHeight: 52),
                  alignment: AlignmentDirectional.centerStart,
                  child: Text(rows[i].name, style: T.value),
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}

class _NoDestinations extends StatelessWidget {
  const _NoDestinations();

  @override
  Widget build(BuildContext context) => Center(
        child: Padding(
          padding: const EdgeInsets.all(40),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              RhallaLogo(size: 56, color: R.primaryA(.3)),
              const SizedBox(height: 20),
              Text('لا توجد وجهات خارجية مفعّلة',
                  textAlign: TextAlign.center,
                  style: T.kufi(15, FontWeight.w600, height: 1.5)),
              const SizedBox(height: 10),
              Text('تُفعَّل الدول من المكتب الخلفي.',
                  textAlign: TextAlign.center,
                  style: T.plex(12.5, FontWeight.w400,
                      color: R.inkA(.55), height: 1.7)),
            ],
          ),
        ),
      );
}

class _Failed extends StatelessWidget {
  const _Failed({required this.message, required this.onRetry});

  final String message;
  final VoidCallback onRetry;

  @override
  Widget build(BuildContext context) => Center(
        child: Padding(
          padding: const EdgeInsets.all(R.padScreen),
          child: GlassCard(
            child: Column(
              mainAxisSize: MainAxisSize.min,
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(message,
                    style: T.plex(12.5, FontWeight.w500,
                        color: R.errorText, height: 1.6)),
                const SizedBox(height: 12),
                TextButton(
                  onPressed: onRetry,
                  style: TextButton.styleFrom(minimumSize: const Size(44, 44)),
                  child: Text('إعادة المحاولة',
                      style: T.plex(12.5, FontWeight.w600,
                          color: R.primaryGradEnd)),
                ),
              ],
            ),
          ),
        ),
      );
}

/// شاشة نجاح الحوالة الخارجية — تعيد استعمال بنية الداخلية.
class ExternalDoneScreen extends StatelessWidget {
  const ExternalDoneScreen({super.key, required this.args});

  final Object? args;

  @override
  Widget build(BuildContext context) {
    final a = args as ExternalDoneArgs?;
    return PopScope(
      canPop: false,
      child: Screen(
        child: ListView(
          padding: const EdgeInsets.fromLTRB(26, 60, 26, 30),
          children: [
            Center(
              child: SizedBox(
                width: 132,
                height: 132,
                child: Stack(
                  alignment: Alignment.center,
                  children: [
                    const Positioned.fill(child: PulseRing(seconds: 2.4)),
                    Container(
                      width: 96,
                      height: 96,
                      decoration: BoxDecoration(
                        shape: BoxShape.circle,
                        gradient: R.primaryGradient,
                        boxShadow: [
                          BoxShadow(
                            color: R.primaryA(.36),
                            blurRadius: 44,
                            offset: const Offset(0, 22),
                          )
                        ],
                      ),
                      child: const Icon(Icons.check_rounded,
                          size: 44, color: Colors.white),
                    ),
                  ],
                ),
              ),
            ),
            const SizedBox(height: 28),
            Text('تمّت الحوالة الخارجية',
                textAlign: TextAlign.center, style: T.titleSm),
            const SizedBox(height: 10),
            Text('أرسل الرمز للمستفيد ليستلمها من وجهته.',
                textAlign: TextAlign.center,
                style: T.plex(13, FontWeight.w400,
                    color: R.inkA(.58), height: 1.8)),
            const SizedBox(height: 26),
            GlassCard(
              large: true,
              sheen: true,
              padding: const EdgeInsets.symmetric(horizontal: 22, vertical: 22),
              child: Column(
                children: [
                  Text('رقم الحوالة', style: T.label),
                  const SizedBox(height: 14),
                  Directionality(
                    textDirection: TextDirection.ltr,
                    child: SelectableText(a?.code ?? '—',
                        textAlign: TextAlign.center,
                        style: T.kufi(34, FontWeight.w800, spacing: 3.5)),
                  ),
                  const SizedBox(height: 18),
                  Divider(color: R.inkA(.07), height: 1),
                  const SizedBox(height: 14),
                  Row(
                    children: [
                      Text('المستفيد',
                          style: T.plex(12, FontWeight.w400,
                              color: R.inkA(.55))),
                      const Spacer(),
                      Text(a?.name ?? '',
                          style: T.plex(13.5, FontWeight.w600)),
                    ],
                  ),
                  /*
                   * ══════════════════════════════════════════════════════
                   *  الوجهةُ والخدمة — أمرُ المالك (11 سبتمبر 2026)
                   * ══════════════════════════════════════════════════════
                   *
                   * «الخارجية تُظهر سعرَ الصرف والقيمةَ بالعملة المحلية وكم
                   *  بالعملة المحوَّل لها، واسمَ الخدمة والدولة والمدينة».
                   *
                   * ⚠ وهي **من المسودّة كما اختارها المُرسِل**، لا من الصفّ
                   * المُعاد: الصفُّ يحمل معرّفاتٍ لا أسماء، وترجمتُها هنا
                   * تعني ثلاثةَ استعلاماتٍ على شاشةِ نجاح — والأسماءُ بين
                   * يديه أصلاً منذ النموذج.
                   *
                   * ⚠ وكلُّ سطرٍ يغيب إن غاب بيانُه: سطرٌ فارغ على ورقةِ
                   * نجاحٍ يُقرأ نقصاً في الحوالة لا نقصاً في العرض.
                   */
                  if ((a?.country ?? '').isNotEmpty) ...[
                    const SizedBox(height: 12),
                    _DoneRow('الدولة', a!.country),
                  ],
                  if ((a?.city ?? '').isNotEmpty) ...[
                    const SizedBox(height: 12),
                    _DoneRow('المدينة', a!.city),
                  ],
                  if ((a?.service ?? '').isNotEmpty) ...[
                    const SizedBox(height: 12),
                    _DoneRow('نوع الخدمة', a!.service),
                  ],

                  const SizedBox(height: 14),
                  Divider(color: R.inkA(.07), height: 1),
                  const SizedBox(height: 14),

                  _DoneMoney('المبلغ بالعملة المحلية', a?.amount ?? 0,
                      currency: 'د.ل'),
                  if ((a?.commission ?? 0) > 0) ...[
                    const SizedBox(height: 12),
                    _DoneMoney('العمولة', a!.commission, currency: 'د.ل'),
                  ],
                  const SizedBox(height: 12),
                  _DoneMoney('المخصوم منك', a?.total ?? 0,
                      currency: 'د.ل', strong: true),

                  if ((a?.rate ?? 0) > 0) ...[
                    const SizedBox(height: 12),
                    _DoneRow('سعر الصرف', Fmt.rate(a!.rate), ltr: true),
                  ],
                  if ((a?.net ?? 0) > 0) ...[
                    const SizedBox(height: 12),
                    _DoneMoney('يستلم المستفيد', a!.net,
                        currency: a.currencyCode, strong: true),
                  ],
                ],
              ),
            ),
            const SizedBox(height: 18),
            AddToFavoritesButton(
              code: a?.favoriteCode ?? '',
              kind: FavoriteKind.external,
              name: a?.name ?? '',
              phone: a?.phone ?? '',
            ),
            const SizedBox(height: 18),
            PrimaryButton(
              label: 'حوالة خارجية جديدة',
              onPressed: () => context.pushReplacement('/send/external'),
            ),
            const SizedBox(height: 10),
            /*
             * ⚠ الوجهةُ تُحسب من الوضع لا تُفترض — القاعدةُ نفسُها المسجَّلة
             * في شاشة المراجعة الداخلية.
             *
             * صارت هذه الشاشةُ مشتركةً بين الوكيل والموظف (11 سبتمبر 2026)،
             * و`/` مسارُ الوكيل وحدَه. وإرسالُ الموظف إليه يجعله يمرّ بإعادة
             * توجيهٍ في المُوجِّه قبل أن يستقرّ — أو يقف حيث لا شاشة له.
             */
            Consumer(builder: (_, r, _) {
              final asEmployee = r.watch(employeeAuthProvider).status ==
                  EmpSessionStatus.signedIn;
              return GlassButton(
                label: 'العودة إلى الرئيسية',
                onPressed: () =>
                    context.go(asEmployee ? '/employee/home' : '/'),
              );
            }),
          ],
        ),
      ),
    );
  }
}

/// سطرٌ في ورقة النجاح — تسميةٌ يميناً وقيمةٌ يساراً.
///
/// ⚠ عنصرٌ واحدٌ لكلّ السطور: ستّةُ صفوفٍ مكتوبةٍ يدوياً كانت ستفترق في
/// الحجم واللون عند أوّل تعديل، فتبدو الورقةُ غيرَ مرتّبة.
class _DoneRow extends StatelessWidget {
  const _DoneRow(this.label, this.value, {this.ltr = false});

  final String label;
  final String value;

  /// قيمةٌ لاتينيّة (سعرُ صرفٍ مثلاً) — يُفرض اتجاهُها وإلّا قلبتها الفقرة.
  final bool ltr;

  @override
  Widget build(BuildContext context) => Row(
        children: [
          Text(label,
              style: T.plex(12, FontWeight.w400, color: R.inkA(.55))),
          const SizedBox(width: 12),
          Expanded(
            child: Align(
              alignment: AlignmentDirectional.centerEnd,
              child: ltr
                  ? Directionality(
                      textDirection: TextDirection.ltr,
                      child: Text(value,
                          style: T.kufi(14, FontWeight.w700)),
                    )
                  : Text(value,
                      maxLines: 1,
                      overflow: TextOverflow.ellipsis,
                      textAlign: TextAlign.end,
                      style: T.plex(13.5, FontWeight.w600)),
            ),
          ),
        ],
      );
}

/// سطرُ مبلغٍ في ورقة النجاح — العملةُ يسارَ الرقم كقاعدة التطبيق كلِّه.
class _DoneMoney extends StatelessWidget {
  const _DoneMoney(this.label, this.value,
      {required this.currency, this.strong = false});

  final String label;
  final double value;
  final String currency;
  final bool strong;

  @override
  Widget build(BuildContext context) => Row(
        children: [
          Text(label,
              style: T.plex(strong ? 12.5 : 12,
                  strong ? FontWeight.w600 : FontWeight.w400,
                  color: strong ? R.inkA(.72) : R.inkA(.55))),
          const Spacer(),
          Directionality(
            // رقمٌ هو مقطعٌ لاتينيّ — يُفرض اتجاهُه، والعملةُ أوّلُ أطفاله
            // فتقع يسارَه على الشاشة.
            textDirection: TextDirection.ltr,
            child: Row(
              crossAxisAlignment: CrossAxisAlignment.baseline,
              textBaseline: TextBaseline.alphabetic,
              children: [
                if (currency.isNotEmpty) ...[
                  Text(currency,
                      style: T.plex(10.5, FontWeight.w400, color: R.inkA(.5))),
                  const SizedBox(width: 5),
                ],
                Text(Fmt.money(value),
                    style: T.kufi(strong ? 15 : 14,
                        strong ? FontWeight.w800 : FontWeight.w700)),
              ],
            ),
          ),
        ],
      );
}
