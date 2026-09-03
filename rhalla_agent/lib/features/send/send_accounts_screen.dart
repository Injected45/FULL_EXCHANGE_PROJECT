import 'dart:async';

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
import 'accounts_repository.dart';

class SendAccountsScreen extends ConsumerStatefulWidget {
  const SendAccountsScreen({super.key, this.prefill});

  /// عميل من المفضّلة — يُملأ هاتفه ويُبحث عن حسابه فوراً.
  final FavoriteCustomer? prefill;

  @override
  ConsumerState<SendAccountsScreen> createState() => _SendAccountsScreenState();
}

class _SendAccountsScreenState extends ConsumerState<SendAccountsScreen> {
  final _phone = TextEditingController();
  final _amount = TextEditingController();
  final _notes = TextEditingController();

  AccountRef? _target;
  List<AccountRef>? _matches;
  bool _searching = false;
  String? _searchError;

  AccountsFee? _fee;
  bool _feeMissing = false;
  bool _feeLoading = false;
  Timer? _feeDebounce;

  String? _error;

  // كل حقل رقمي: يُفرَغ عند دخول المؤشّر، والمبالغ تُنسَّق عند الخروج.
  late final _amountFocus = AutoClearFocus(_amount,
      onChanged: () => setState(() {}), formatOnExit: true);
  late final _phoneFocus =
      AutoClearFocus(_phone, onChanged: () => setState(() {}));
  /// النصّية أيضاً تُفرَغ عند الدخول — قرار المالك.
  late final _notesFocus = AutoClearFocus(_notes);

  @override
  void initState() {
    super.initState();
    final p = widget.prefill;
    if (p == null) return;
    _phone.text = Fmt.phoneForApi(p.phone);
    // ‏_search يستدعي setState، ولا يجوز ذلك داخل initState.
    Future.microtask(_search);
  }

  Future<void> _pickFavorite() async {
    final c = await FavoritePickerSheet.show(context, FavoriteKind.accounts);
    if (c == null) return;
    _phone.text = Fmt.phoneForApi(c.phone);
    if (!mounted) return;
    setState(() {});
    await _search();
  }

  @override
  void dispose() {
    _feeDebounce?.cancel();
    _amountFocus.dispose();
    _phoneFocus.dispose();
    _notesFocus.dispose();
    for (final c in [_phone, _amount, _notes]) {
      c.dispose();
    }
    super.dispose();
  }

  double get _amountValue => Fmt.num_(_amount.text);

  bool get _valid =>
      _target != null && _amountValue > 0 && _fee != null && !_feeMissing;

  Future<void> _search() async {
    final raw = _phone.text.trim();
    if (raw.length < 6) return;
    setState(() {
      _searching = true;
      _searchError = null;
      _matches = null;
      _target = null;
    });
    try {
      final list =
          await ref.read(accountsRepositoryProvider).searchByPhone(raw);
      if (!mounted) return;
      setState(() {
        _matches = list;
        if (list.length == 1) _target = list.first;
        if (list.isEmpty) {
          _searchError = 'لا يوجد حساب بهذا الرقم. (رقمك أنت لا يظهر هنا.)';
        }
      });
    } on ApiFailure catch (e) {
      if (mounted) setState(() => _searchError = e.message);
    } finally {
      if (mounted) setState(() => _searching = false);
    }
  }

  void _onAmountChanged() {
    _feeDebounce?.cancel();
    setState(() {
      _fee = null;
      _feeMissing = false;
    });
    if (_amountValue <= 0) return;
    _feeDebounce = Timer(const Duration(milliseconds: 550), _loadFee);
  }

  Future<void> _loadFee() async {
    final amount = _amountValue;
    if (amount <= 0) return;
    setState(() => _feeLoading = true);
    try {
      final f = await ref.read(accountsRepositoryProvider).fee(amount);
      if (!mounted || _amountValue != amount) return;
      setState(() {
        _fee = f;
        _feeMissing = f == null;
      });
    } on ApiFailure catch (e) {
      if (mounted) setState(() => _error = e.message);
    } finally {
      if (mounted) setState(() => _feeLoading = false);
    }
  }

  /// إلى المراجعة، لا إلى التنفيذ.
  ///
  /// كانت هذه الشاشة تنفّذ التحويل مباشرة من النموذج، مع أنه فوري ولا رجعة
  /// فيه — تماماً كالحوالة الداخلية التي لها شاشة مراجعة للسبب نفسه.
  /// التنفيذ الآن في [ReviewAccountsScreen]، ولا تُبنى المسوّدة إلا وفيها
  /// عمولة صالحة من الخادم (`_valid` يمنع المتابعة عند ثغرة الشريحة).
  void _review() {
    if (!_valid) return;
    FocusScope.of(context).unfocus();
    setState(() => _error = null);

    context.push(
      '/send/accounts/review',
      extra: AccountsDraft(
        target: _target!,
        amount: _amountValue,
        fee: _fee!,
        notes: _notes.text,
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    final user = ref.watch(authControllerProvider).user;
    final currency = user?.currencyCode ?? 'د.ل';

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'تحويل بين الحسابات',
            subtitle: 'من حسابك إلى حساب آخر داخل المنظومة',
            onBack: () => context.pop(),
          ),
          Expanded(
            child: ListView(
              padding: const EdgeInsets.fromLTRB(
                  R.padScreen, 20, R.padScreen, 30),
              children: [
                _phoneCard(),
                if (_searching) ...[
                  const SizedBox(height: R.gapCard),
                  Center(child: Text('جارٍ البحث…', style: T.meta)),
                ],
                if (_searchError != null) ...[
                  const SizedBox(height: 10),
                  Text(_searchError!,
                      style: T.plex(12, FontWeight.w500,
                          color: R.errorText, height: 1.6)),
                ],
                if (_matches != null && _matches!.length > 1) ...[
                  const SizedBox(height: R.gapCard),
                  Text('اختر الحساب', style: T.label),
                  const SizedBox(height: 8),
                  for (final a in _matches!) ...[
                    _AccountTile(
                      account: a,
                      selected: a.accId == _target?.accId,
                      onTap: () => setState(() => _target = a),
                    ),
                    const SizedBox(height: R.gapRow),
                  ],
                ] else if (_target != null) ...[
                  const SizedBox(height: R.gapCard),
                  _AccountTile(account: _target!, selected: true, onTap: null),
                ],

                const SizedBox(height: R.gapCard),
                _amountCard(currency),
                const SizedBox(height: R.gapCard),
                _feeCard(currency),
                const SizedBox(height: R.gapCard),

                GlassCard(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text('ملاحظة (اختياري)', style: T.label),
                      const SizedBox(height: 9),
                      TextField(
                        controller: _notes,
                        focusNode: _notesFocus,
                        inputFormatters: lettersOnlyFormatters,
                        maxLines: 2,
                        style: T.value,
                        decoration: InputDecoration(
                          isDense: true,
                          border: InputBorder.none,
                          hintText: 'سبب التحويل',
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
                          color: R.error, height: 1.6)),
                ],

                const SizedBox(height: 16),
                const WarnBanner(
                  text:
                      'العمولة هنا يحدّدها النظام لا الوكيل، والتحويل التالي بعد 3 دقائق.',
                ),
                const SizedBox(height: 16),
                PrimaryButton(
                  label: 'مراجعة التحويل',
                  onPressed: _valid ? _review : null,
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _phoneCard() => GlassCard(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Text('هاتف صاحب الحساب', style: T.label),
                const Spacer(),
                FavoriteFieldButton(onTap: _pickFavorite),
              ],
            ),
            const SizedBox(height: 9),
            Row(
              children: [
                Expanded(
                  child: Directionality(
                    textDirection: TextDirection.ltr,
                    child: TextField(
                      controller: _phone,
                      focusNode: _phoneFocus,
                      keyboardType: TextInputType.phone,
                      textInputAction: TextInputAction.search,
                      onSubmitted: (_) => _search(),
                      onChanged: (_) => setState(() {}),
                      // أرقام فقط: لا حروف ولا إشارات. البادئة 218 أو 00218
                      // أو الصفر تُزال في Fmt.phoneForApi، فلا حاجة إلى «+».
                      inputFormatters: [
                        const WesternDigits(),
                        FilteringTextInputFormatter.digitsOnly,
                      ],
                      style: T.kufi(17, FontWeight.w600),
                      decoration: InputDecoration(
                        isDense: true,
                        border: InputBorder.none,
                        hintText: '09XXXXXXXX',
                        hintStyle: T.kufi(17, FontWeight.w600,
                            color: R.inkA(.22)),
                      ),
                    ),
                  ),
                ),
                const SizedBox(width: 8),
                SizedBox(
                  height: 44,
                  child: TextButton(
                    onPressed:
                        _phone.text.trim().length >= 6 && !_searching
                            ? _search
                            : null,
                    style: TextButton.styleFrom(
                      backgroundColor: R.primaryA(.1),
                      shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(R.rRow)),
                      padding: const EdgeInsets.symmetric(horizontal: 18),
                    ),
                    child: Text('بحث',
                        style: T.kufi(12.5, FontWeight.w600,
                            color: R.primaryGradEnd)),
                  ),
                ),
              ],
            ),
          ],
        ),
      );

  Widget _amountCard(String currency) => GlassCard(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text('المبلغ', style: T.label),
            const SizedBox(height: 9),
            Directionality(
              textDirection: TextDirection.ltr,
              child: TextField(
                controller: _amount,
                focusNode: _amountFocus,
                onChanged: (_) => _onAmountChanged(),
                keyboardType:
                    const TextInputType.numberWithOptions(decimal: true),
                inputFormatters: moneyInputFormatters,
                style: T.kufi(24, FontWeight.w700),
                decoration: InputDecoration(
                  isDense: true,
                  border: InputBorder.none,
                  hintText: '0.00',
                  hintStyle:
                      T.kufi(24, FontWeight.w700, color: R.inkA(.22)),
                  // مسافة لاصقة بالرمز — بدونها يلتصق «د.ل» بالرقم: د.ل0
                  prefixText: '$currency  ',
                  prefixStyle:
                      T.plex(12, FontWeight.w400, color: R.inkA(.5)),
                ),
              ),
            ),
          ],
        ),
      );

  Widget _feeCard(String currency) {
    if (_feeMissing) {
      return Container(
        padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 16),
        decoration: BoxDecoration(
          color: R.error.withValues(alpha: .07),
          border: Border.all(color: R.error.withValues(alpha: .22)),
          borderRadius: BorderRadius.circular(R.rCard),
        ),
        child: Text(
          'لا توجد شريحة عمولة تغطّي هذا المبلغ، والخادم سيرفض التحويل. '
          'جرّب مبلغاً آخر أو راجع الفرع.',
          style: T.plex(12.5, FontWeight.w500, color: R.errorText, height: 1.7),
        ),
      );
    }

    return GlassCard(
      child: Column(
        children: [
          Row(
            children: [
              Text('العمولة', style: T.label),
              const SizedBox(width: 7),
              Container(
                padding:
                    const EdgeInsets.symmetric(horizontal: 9, vertical: 4),
                decoration: BoxDecoration(
                  color: R.inkA(.06),
                  borderRadius: BorderRadius.circular(99),
                ),
                child: Text('يحدّدها النظام',
                    style: T.plex(11, FontWeight.w500, color: R.inkA(.55))),
              ),
              const Spacer(),
              if (_feeLoading)
                SizedBox(
                  width: 16,
                  height: 16,
                  child: CircularProgressIndicator(
                      strokeWidth: 2, color: R.primary),
                )
              else
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text(
                    _fee == null ? '—' : Fmt.money(_fee!.commission),
                    style: T.kufi(15, FontWeight.w700),
                  ),
                ),
            ],
          ),
          const SizedBox(height: 14),
          Divider(color: R.inkA(.07), height: 1),
          const SizedBox(height: 14),
          Row(
            children: [
              Text('الإجمالي المخصوم',
                  style: T.plex(12.5, FontWeight.w600)),
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
                    Text(
                      _fee == null ? '—' : Fmt.money(_fee!.total),
                      style: T.kufi(18, FontWeight.w700),
                    ),
                  ],
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }
}

class _AccountTile extends StatelessWidget {
  const _AccountTile({
    required this.account,
    required this.selected,
    required this.onTap,
  });

  final AccountRef account;
  final bool selected;
  final VoidCallback? onTap;

  @override
  Widget build(BuildContext context) => GlassRow(
        onTap: onTap,
        children: [
          IconTile(
            letter: account.initial,
            background: selected ? R.primaryA(.18) : R.primaryA(.1),
          ),
          const SizedBox(width: 12),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(account.name.isEmpty ? '—' : account.name,
                    style: T.name, maxLines: 1, overflow: TextOverflow.ellipsis),
                const SizedBox(height: 3),
                Directionality(
                  textDirection: TextDirection.ltr,
                  child: Text(
                    [account.code, account.branchName]
                        .where((s) => s.isNotEmpty)
                        .join('  ·  '),
                    style: T.meta,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                  ),
                ),
              ],
            ),
          ),
          if (selected)
            Container(
              width: 22,
              height: 22,
              decoration: BoxDecoration(
                  gradient: R.primaryGradient, shape: BoxShape.circle),
              child: const Icon(Icons.check_rounded,
                  size: 14, color: Colors.white),
            ),
        ],
      );
}

/// نجاح التحويل بين الحسابات.
class AccountsDoneScreen extends StatelessWidget {
  const AccountsDoneScreen({super.key, required this.transfer});

  final AccountsTransfer transfer;

  @override
  Widget build(BuildContext context) => PopScope(
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
                        child: const Icon(Icons.swap_horiz_rounded,
                            size: 44, color: Colors.white),
                      ),
                    ],
                  ),
                ),
              ),
              const SizedBox(height: 28),
              Text('تمّ التحويل',
                  textAlign: TextAlign.center, style: T.titleSm),
              const SizedBox(height: 10),
              Text('خُصم المبلغ والعمولة من حسابك فوراً.',
                  textAlign: TextAlign.center,
                  style: T.plex(13, FontWeight.w400,
                      color: R.inkA(.58), height: 1.8)),
              const SizedBox(height: 26),
              GlassCard(
                large: true,
                sheen: true,
                padding:
                    const EdgeInsets.symmetric(horizontal: 22, vertical: 22),
                child: Column(
                  children: [
                    _row('إلى', transfer.receiverName),
                    const SizedBox(height: 12),
                    _amountRow('المبلغ', transfer.amount),
                    const SizedBox(height: 12),
                    _amountRow('العمولة', transfer.commission),
                    const SizedBox(height: 14),
                    Divider(color: R.inkA(.07), height: 1),
                    const SizedBox(height: 14),
                    _row('الرمز', transfer.shareCode, mono: true),
                  ],
                ),
              ),
              const SizedBox(height: 18),
              AddToFavoritesButton(
                code: transfer.code,
                kind: FavoriteKind.accounts,
                name: transfer.receiverName,
                phone: transfer.receiverPhone,
              ),
              const SizedBox(height: 18),
              PrimaryButton(
                label: 'كشف الحساب',
                onPressed: () => context.pushReplacement('/statement'),
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

  static Widget _row(String label, String value, {bool mono = false}) => Row(
        children: [
          Text(label,
              style: T.plex(12, FontWeight.w400, color: R.inkA(.55))),
          const Spacer(),
          if (mono)
            Directionality(
              textDirection: TextDirection.ltr,
              child: SelectableText(value.isEmpty ? '—' : value,
                  style: T.kufi(15, FontWeight.w700, spacing: 1.5)),
            )
          else
            Flexible(
              child: Text(value.isEmpty ? '—' : value,
                  style: T.plex(13.5, FontWeight.w600),
                  maxLines: 1,
                  overflow: TextOverflow.ellipsis),
            ),
        ],
      );

  static Widget _amountRow(String label, double v) => Row(
        children: [
          Text(label,
              style: T.plex(12, FontWeight.w400, color: R.inkA(.55))),
          const Spacer(),
          Directionality(
            textDirection: TextDirection.ltr,
            child: Text(Fmt.money(v), style: T.kufi(14, FontWeight.w700)),
          ),
        ],
      );
}
