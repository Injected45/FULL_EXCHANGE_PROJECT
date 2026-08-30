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
import '../favorites/favorites_repository.dart';
import '../favorites/favorites_screen.dart';
import '../shell/auto_refresh.dart';
import 'external_repository.dart';
import 'limit_dialog.dart';
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
  late final _amountFocus = NumericFieldFocus(_amount,
      onChanged: () => setState(() => _quote = null), formatOnExit: true);
  late final _commissionFocus = NumericFieldFocus(_commission,
      onChanged: () => setState(() {}), formatOnExit: true);
  late final _phoneFocus =
      NumericFieldFocus(_phone, onChanged: () => setState(() {}));

  @override
  void dispose() {
    _amountFocus.dispose();
    _commissionFocus.dispose();
    _phoneFocus.dispose();
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
      _branch != null &&
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

  Future<void> _send() async {
    if (!_valid) {
      setState(() => _error = 'أكمل بيانات الحوالة أولاً.');
      return;
    }
    final user = ref.read(authControllerProvider).user;
    if (user == null) return;

    setState(() {
      _sending = true;
      _error = null;
    });

    final draft = ExternalDraft(
      country: _country!,
      city: _city!,
      branch: _branch!,
      service: _service!,
      receiverName: _name.text.trim(),
      receiverPhone: _phone.text.trim(),
      amountLyd: _amountValue,
      commission: _commissionValue,
      deliveredCurrencyId: _deliveredCurrency,
      quote: _quote,
    );

    try {
      final row = await ref
          .read(externalRepositoryProvider)
          .create(d: draft, accId: user.accId);
      if (!mounted) return;
      refreshAfterMoneyAction(ref);
      // المُشغِّل يحسب NetTotal/TransPrice بعد الإدراج، والخادم يعيد الصف
      // بعدها — فهذه أرقام ما كُتب فعلاً، لا تقدير العميل.
      context.pushReplacement('/send/external/done', extra: _DoneArgs(
        code: '${row['codeForMobile'] ?? row['Code'] ?? ''}',
        favoriteCode: '${row['Code'] ?? ''}'.trim(),
        name: draft.receiverName,
        phone: draft.receiverPhone,
        amount: draft.amountLyd,
        commission: draft.commission,
        net: Fmt.num_(row['NetTotal']),
        rate: Fmt.num_(row['TransPrice']),
        currencyCode: _quote?.currencyCode ?? '',
      ));
    } on ApiFailure catch (e) {
      if (!mounted) return;
      // تجاوز السقف حدٌّ لا خطأ — حوار في وسط الشاشة، ولا يُكتب في _error
      // كي لا يبقى نصّاً أحمر أسفل النموذج بعد إغلاق الحوار.
      final overLimit = TransferLimitExceeded.from(e);
      setState(() {
        _sending = false;
        _error = overLimit == null ? e.message : null;
      });
      if (overLimit != null) await showLimitExceededDialog(context, overLimit);
    }
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
          ),
          Expanded(
            child: countries.when(
              loading: () =>
                  const Center(child: CircularProgressIndicator(color: R.primary)),
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

          Consumer(builder: (_, r, _) {
            final branches = r.watch(branchesProvider);
            return _PickerCard(
              label: 'الفرع المُصدِّر',
              value: _branch?.name,
              loading: branches.isLoading,
              onTap: () => _pickFrom('اختر الفرع',
                  branches.valueOrNull ?? const [], (v) => setState(() => _branch = v)),
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

class _DoneArgs {
  const _DoneArgs({
    required this.code,
    required this.favoriteCode,
    required this.name,
    required this.phone,
    required this.amount,
    required this.commission,
    required this.net,
    required this.rate,
    required this.currencyCode,
  });

  /// الرمز المعروض للوكيل والمستفيد.
  final String code;

  /// `ExternalEx.Code` — **وليس** رمز الموبايل. المفضّلة تُخزَّن به لأن
  /// `Favorites_GetByUserID` يربط `code_Favorite` بعمود `Code` وحده، فحفظ
  /// رمز الموبايل يعني صفّاً لا يظهر في القائمة أبداً.
  final String favoriteCode;
  final String name;
  final String phone;
  final double amount;
  final double commission;

  /// ما يستلمه المستفيد بعملة الوجهة — من الصف المُدرَج.
  final double net;
  final double rate;
  final String currencyCode;
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
  final VoidCallback onTap;
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
                    loading ? 'جارٍ التحميل…' : (value ?? 'اختر'),
                    style: value == null
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
              const SizedBox(
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
                const SizedBox(
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
    final a = args as _DoneArgs?;
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
                  Text('رمز الحوالة', style: T.label),
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
                  const SizedBox(height: 12),
                  Row(
                    children: [
                      Text('المخصوم منك',
                          style: T.plex(12, FontWeight.w400,
                              color: R.inkA(.55))),
                      const Spacer(),
                      Directionality(
                        textDirection: TextDirection.ltr,
                        child: Text(
                            Fmt.money((a?.amount ?? 0) + (a?.commission ?? 0)),
                            style: T.kufi(14, FontWeight.w700)),
                      ),
                    ],
                  ),
                  if ((a?.net ?? 0) > 0) ...[
                    const SizedBox(height: 12),
                    Row(
                      children: [
                        Text('يستلم المستفيد',
                            style: T.plex(12, FontWeight.w400,
                                color: R.inkA(.55))),
                        const Spacer(),
                        Directionality(
                          textDirection: TextDirection.ltr,
                          child: Row(
                            crossAxisAlignment: CrossAxisAlignment.baseline,
                            textBaseline: TextBaseline.alphabetic,
                            children: [
                              Text(Fmt.money(a!.net),
                                  style: T.kufi(14, FontWeight.w700)),
                              const SizedBox(width: 5),
                              Text(a.currencyCode,
                                  style: T.plex(10.5, FontWeight.w400,
                                      color: R.inkA(.5))),
                            ],
                          ),
                        ),
                      ],
                    ),
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
            GlassButton(
              label: 'العودة إلى الرئيسية',
              onPressed: () => context.go('/'),
            ),
          ],
        ),
      ),
    );
  }
}
