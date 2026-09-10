import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/format/fmt.dart';
import '../../core/net/api_client.dart';
import '../../core/net/api_envelope.dart';
import '../../core/theme/app_theme.dart';
import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import 'employee_header.dart';

/// إضافةُ مستفيدٍ جديد — من حوالاتِه هو.
///
/// ══════════════════════════════════════════════════════════════════════════
///  ⚠ «إضافة مستفيد» في هذه المنظومة ليست نموذجَ اسمٍ وهاتف
/// ══════════════════════════════════════════════════════════════════════════
///
/// المفضّلةُ في القاعدة مخزَّنةٌ **لكلّ حوالة** لا لكلّ شخص: العمودُ المفتاحُ
/// هو `code_Favorite`، ويربطه الإجراءُ المخزَّن `Favorites_GetByUserID`
/// بـ`Code` في `InternalEx` و`ExternalEx` و`TransBetweenAccountsTB`. فلا
/// وجودَ لصفٍّ بلا حوالةٍ خلفه.
///
/// ومعنى ذلك أنّ «إضافةَ مستفيدٍ جديد» تعني حرفياً: **اختر حوالةً نفّذتَها
/// واحفظ مستفيدَها**. وهو ما يفعله الوكيلُ نفسُه من فاتورة الحوالة
/// (`AddToFavoritesButton`) — القاعدةُ واحدة، والشاشةُ هنا تجمع ما يفعله هو
/// من فواتيرَ متفرّقة في مكانٍ واحد.
///
/// ⚠ ولم يُخترَع نموذجُ إدخالٍ حرّ رغم أنّ ظاهرَ الطلب يوحي به: صفٌّ يُكتب
/// بلا `code_Favorite` صحيح **لا يظهر في القائمة أصلاً** — لأن الإجراءَ
/// يقرأ بالربط — فيبدو للموظف أنّ الإضافة «لا تعمل»، وهو أسوأُ من بابٍ
/// يقول بصراحةٍ من أين يُضاف المستفيد.
///
/// ⚠ ومن حوالاتِه هو وحدَها — قاعدةُ المالك (8 سبتمبر 2026): الموظف لا يرى
/// عمل زميله.
class EmployeeAddFavoriteScreen extends ConsumerStatefulWidget {
  const EmployeeAddFavoriteScreen({super.key});

  @override
  ConsumerState<EmployeeAddFavoriteScreen> createState() =>
      _EmployeeAddFavoriteScreenState();
}

class _EmployeeAddFavoriteScreenState
    extends ConsumerState<EmployeeAddFavoriteScreen> {
  /// رقمُ الحوالة التي يجري حفظُها الآن — لتعطيل صفِّها وحدَه.
  String? _busy;

  /// ما حُفظ في هذه الجلسة، فلا يُضغط مرّتين ولا يُقال «فشل» لما نجح.
  final _done = <String>{};

  @override
  Widget build(BuildContext context) {
    final async = ref.watch(employeeOutgoingSourceProvider);

    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'إضافة مستفيد جديد',
            subtitle: 'اختر حوالةً لتحفظ مستفيدَها',
            onBack: () => Navigator.of(context).pop(),
          ),
          Expanded(
            child: RefreshIndicator(
              onRefresh: () async =>
                  ref.invalidate(employeeOutgoingSourceProvider),
              color: R.primary,
              backgroundColor: Colors.white,
              child: async.when(
                loading: () => const Center(child: CircularProgressIndicator()),
                // ⚠ داخل قائمةٍ لا عارياً: `RefreshIndicator` يحتاج طفلاً
                // قابلاً للتمرير، وبدونه تموت السحبةُ في حالة الخطأ — وهي
                // الحالةُ الوحيدة التي يحتاجها المستخدم فعلاً.
                error: (e, _) => ListView(children: [
                  EmployeeEmpty(
                      icon: Icons.wifi_off_rounded,
                      text: 'تعذّر التحميل.\n$e'),
                ]),
                data: (rows) => rows.isEmpty
                    ? ListView(
                        physics: const AlwaysScrollableScrollPhysics(),
                        children: const [
                          EmployeeEmpty(
                            icon: Icons.person_off_outlined,
                            text: 'لا حوالات لك بعد.\n\n'
                                'المستفيدُ يُحفَظ من حوالةٍ نفّذتَها، '
                                'فأنشئ حوالةً أوّلاً.',
                          ),
                        ],
                      )
                    : ListView.separated(
                        padding: const EdgeInsets.fromLTRB(
                            R.padScreen, 14, R.padScreen, 30),
                        itemCount: rows.length,
                        separatorBuilder: (_, _) => const SizedBox(height: 8),
                        itemBuilder: (_, i) {
                          final m = rows[i];
                          final code = '${m['transfer_number'] ?? ''}';
                          return _AddFavRow(
                            code: code,
                            name: '${m['beneficiary'] ?? ''}'.trim(),
                            phone: '${m['beneficiary_phone'] ?? ''}'.trim(),
                            busy: _busy == code,
                            done: _done.contains(code),
                            onAdd: () => _add(m),
                          );
                        },
                      ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Future<void> _add(Map<String, dynamic> m) async {
    final code = '${m['transfer_number'] ?? ''}'.trim();
    final phone = '${m['beneficiary_phone'] ?? ''}'.trim();

    /*
     * ⚠ الخادمُ يشترط الهاتف ويرفض الطلب بدونه (‏required|string|max:50).
     * فيُقال ذلك هنا بجملةٍ يفهمها الموظف، بدل أن يُردّ بخطأِ تحقّقٍ لا
     * معنى له في يده.
     */
    if (phone.isEmpty) {
      _say('هذه الحوالة بلا رقم هاتفٍ للمستفيد، ولا يمكن حفظُها في المفضّلة.');
      return;
    }

    setState(() => _busy = code);
    try {
      await ref.read(apiClientProvider).post(
        '/device/employee/favorites/add',
        body: {
          'code_Favorite': code,
          // 1 = حوالة محلية. اصطلاحُ التطبيق نفسُه في `FavoriteKind`.
          'Type_Favorite': 1,
          'phone': phone,
        },
      );
      if (mounted) setState(() => _done.add(code));
    } on ApiFailure catch (e) {
      /*
       * ⚠ 409 تعني «محفوظٌ سلفاً» — والنتيجةُ التي أرادها متحقّقة، فلا
       * تُعرض خطأً: المستفيد في مفضّلته. وهي القاعدةُ نفسُها في
       * `FavoritesRepository.add` عند الوكيل.
       */
      if (e.statusCode == 409) {
        if (mounted) setState(() => _done.add(code));
      } else if (mounted) {
        _say(e.message);
      }
    } catch (_) {
      if (mounted) _say('تعذّر الحفظ — تحقّق من الاتصال.');
    } finally {
      if (mounted) setState(() => _busy = null);
    }
  }

  void _say(String m) {
    ScaffoldMessenger.of(context)
      ..hideCurrentSnackBar()
      ..showSnackBar(SnackBar(
        content:
            Text(m, style: T.plex(13, FontWeight.w500, color: Colors.white)),
        backgroundColor: R.inkA(.92),
        behavior: SnackBarBehavior.floating,
      ));
  }
}

/// مصدرُ الاختيار — حوالاتُ هذا الموظف الصادرة، بأسماء مستفيديها.
///
/// ⚠ النقطةُ نفسُها التي تُبنى عليها «صادرتي»، لا نقطةٌ ثانية: نقطتان
/// تعطيان قائمتين مختلفتين، فيرى الموظف في الإضافة ما لا يراه في حوالاته.
final employeeOutgoingSourceProvider =
    FutureProvider.autoDispose<List<Map<String, dynamic>>>((ref) async {
  final env = await ref
      .watch(apiClientProvider)
      .get('/device/employee/transfers/outgoing', query: {'limit': 100});
  final row = env.row ?? const {};
  return ((row['items'] as List?) ?? const [])
      .whereType<Map>()
      .map((e) => e.cast<String, dynamic>())
      .toList();
});

class _AddFavRow extends StatelessWidget {
  const _AddFavRow({
    required this.code,
    required this.name,
    required this.phone,
    required this.busy,
    required this.done,
    required this.onAdd,
  });

  final String code;
  final String name;
  final String phone;
  final bool busy;
  final bool done;
  final VoidCallback onAdd;

  @override
  Widget build(BuildContext context) => GlassCard(
        child: Row(
          children: [
            IconTile(
              size: 38,
              background: R.primaryA(.12),
              icon: Icon(Icons.person_outline_rounded,
                  size: 19, color: R.primaryDark),
            ),
            const SizedBox(width: 12),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(name.isEmpty ? 'بلا اسم' : name,
                      maxLines: 1,
                      overflow: TextOverflow.ellipsis,
                      style: T.kufi(14, FontWeight.w700)),
                  const SizedBox(height: 3),
                  Directionality(
                    // رقمٌ لاتينيّ في فقرةٍ عربية — يُفرض اتجاهه.
                    textDirection: TextDirection.ltr,
                    child: Text(
                        phone.isEmpty ? code : '${Fmt.phone(phone)} · $code',
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                        style:
                            T.plex(11, FontWeight.w400, color: R.inkA(.55))),
                  ),
                ],
              ),
            ),
            if (busy)
              const SizedBox(
                  width: 20,
                  height: 20,
                  child: CircularProgressIndicator(strokeWidth: 2))
            else if (done)
              Icon(Icons.check_circle_rounded, size: 22, color: R.primary)
            else
              IconButton(
                tooltip: 'حفظ في المفضّلة',
                onPressed: onAdd,
                icon: Icon(Icons.star_border_rounded,
                    size: 22, color: R.primaryDark),
                constraints:
                    const BoxConstraints(minWidth: 44, minHeight: 44),
              ),
          ],
        ),
      );
}
