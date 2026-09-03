/// توليد اسم الشركة بالإنجليزية من اسمها بالعربية، لحظةَ الكتابة.
///
/// **لماذا محليّاً لا عبر ترجمة جوجل؟** ثلاثة أسباب، والثالث هو الحاسم:
/// الوكيل قد يعمل في فرعٍ بلا إنترنت وحقلٌ لا يُملأ إلا متّصلاً يبدو معطّلاً؛
/// وطلبُ شبكةٍ مع كل حرف يُكتب بطيء ومكلف؛ و**اسم شركةٍ لا يُترجم، بل
/// يُنقَل حرفياً** — «الأمانة» ليست "The Trust"، بل "Al Amana". مترجمٌ عام
/// يفعل الأولى، وهو خطأ في اسمٍ يُطبع على الفواتير.
///
/// فالمعالجة هنا هجينة: الكلمات الوصفية المعروفة في أسماء شركات الصرافة
/// تُترجم («شركة» ⇦ Company، «للحوالات المالية» ⇦ for Financial Transfers)،
/// وما عداها — وهو الاسم العَلَم — يُنقل حرفياً.
///
/// والنتيجة اقتراح لا حكم: الحقل الإنجليزي يبقى قابلاً للتعديل، وأول تعديل
/// يدويّ يوقف التوليد التلقائي فلا يُمحى ما كتبه المستخدم.
library;

/// أنواع الكيانات — تُنقل إلى آخر الاسم العَلَم كما في الإنجليزية:
/// «شركة الأمانة» ⇦ "Al Amana Company"، لا "Company Al Amana".
const _kinds = <String, String>{
  'شركة': 'Company',
  'مؤسسة': 'Establishment',
  'وكالة': 'Agency',
  'مكتب': 'Office',
  'مصرف': 'Bank',
  'بنك': 'Bank',
  'مجموعة': 'Group',
};

/// صفات — تسبق موصوفها في الإنجليزية وتتبعه في العربية، فتُعكس.
const _adjectives = <String, String>{
  'المالية': 'Financial',
  'مالية': 'Financial',
  'الليبية': 'Libyan',
  'ليبية': 'Libyan',
  'الدولية': 'International',
  'دولية': 'International',
  'الإسلامية': 'Islamic',
  'الاسلامية': 'Islamic',
  'العامة': 'General',
  'الوطنية': 'National',
  'السريعة': 'Express',
  'المحدودة': 'Limited',
  'الحديثة': 'Modern',
  'المتحدة': 'United',
};

/// أسماء الأعمال المعروفة — تُترجم لأنها وصفٌ للنشاط لا اسمٌ عَلَم.
const _nouns = <String, String>{
  'حوالات': 'Transfers',
  'الحوالات': 'Transfers',
  'تحويلات': 'Transfers',
  'التحويلات': 'Transfers',
  'صرافة': 'Exchange',
  'الصرافة': 'Exchange',
  'خدمات': 'Services',
  'الخدمات': 'Services',
  'تجارة': 'Trading',
  'التجارة': 'Trading',
  'استثمار': 'Investment',
  'الاستثمار': 'Investment',
  'فرع': 'Branch',
  'ليبيا': 'Libya',
  'طرابلس': 'Tripoli',
  'بنغازي': 'Benghazi',
  'مصراتة': 'Misrata',
  'سبها': 'Sabha',
};

/// جدول النقل الحرفي. اختيارات مقصودة: `خ` ⇦ kh و`ش` ⇦ sh و`ث` ⇦ th، وهي
/// الصيغ الشائعة في أسماء الشركات الليبية على اللافتات والأوراق الرسمية.
const _letters = <String, String>{
  'ا': 'a', 'أ': 'a', 'إ': 'i', 'آ': 'aa', 'ٱ': 'a',
  'ب': 'b', 'ت': 't', 'ث': 'th', 'ج': 'j', 'ح': 'h', 'خ': 'kh',
  'د': 'd', 'ذ': 'dh', 'ر': 'r', 'ز': 'z', 'س': 's', 'ش': 'sh',
  'ص': 's', 'ض': 'd', 'ط': 't', 'ظ': 'z', 'ع': 'a', 'غ': 'gh',
  'ف': 'f', 'ق': 'q', 'ك': 'k', 'ل': 'l', 'م': 'm', 'ن': 'n',
  'ه': 'h', 'و': 'w', 'ي': 'y', 'ى': 'a', 'ة': 'a',
  'ؤ': 'u', 'ئ': 'i', 'ء': '', 'ﻻ': 'la', 'لا': 'la',
};

/// التشكيل والتطويل — تُحذف قبل أي معالجة، وإلا صار «الأمانةُ» كلمةً أخرى
/// لا يجدها القاموس.
final _marks = RegExp('[ً-ْٰـ]');

class ArabicToLatin {
  ArabicToLatin._();

  /// الاسم الإنجليزي المقترح، أو سلسلة فارغة إن لم يكن هناك ما يُقترح.
  static String suggest(String arabic) {
    final cleaned = arabic.replaceAll(_marks, '').trim();
    if (cleaned.isEmpty) return '';

    final words = cleaned.split(RegExp(r'\s+')).where((w) => w.isNotEmpty).toList();
    if (words.isEmpty) return '';

    String? kind;
    final proper = <String>[];
    final tail = <_Token>[];

    // «لـ» و«لل» تعني «for» — وهي الفاصل بين الاسم العَلَم ووصف النشاط.
    var seenFor = false;

    for (final w in words) {
      if (kind == null && _kinds.containsKey(w)) {
        kind = _kinds[w];
        continue;
      }

      final stripped = _stripFor(w);
      if (stripped != null) {
        seenFor = true;
        tail.add(_classify(stripped));
        continue;
      }

      if (seenFor) {
        tail.add(_classify(w));
      } else if (_adjectives.containsKey(w) || _nouns.containsKey(w)) {
        // وصفٌ بلا «لـ» — «شركة الصرافة الليبية». يبقى في الذيل.
        seenFor = true;
        tail.add(_classify(w));
      } else {
        proper.add(_translit(w));
      }
    }

    final parts = <String>[];
    if (proper.isNotEmpty) parts.add(proper.join(' '));
    if (kind != null) parts.add(kind);

    final descriptor = _joinTail(tail);
    if (descriptor.isNotEmpty) {
      parts.add(seenFor && proper.isNotEmpty ? 'for $descriptor' : descriptor);
    }

    return parts.join(' ').replaceAll(RegExp(r'\s+'), ' ').trim();
  }

  /// يزيل «لـ» أو «لل» من أول الكلمة ويعيد الأصل، أو null إن لم تكن كذلك.
  ///
  /// «للحوالات» ⇦ «الحوالات»: اللام الأولى للجرّ والثانية لام التعريف،
  /// فتُعاد الألف كي تُطابق القاموس.
  static String? _stripFor(String w) {
    if (w.startsWith('لل') && w.length > 3) return 'ا${w.substring(1)}';
    if (w.startsWith('ل') && w.length > 3 && _nouns.containsKey(w.substring(1))) {
      return w.substring(1);
    }
    return null;
  }

  static _Token _classify(String w) {
    if (_adjectives.containsKey(w)) return _Token(_adjectives[w]!, adjective: true);
    if (_nouns.containsKey(w)) return _Token(_nouns[w]!);
    return _Token(_translit(w));
  }

  /// يعكس كل صفةٍ لتسبق موصوفها: «حوالات مالية» ⇦ "Financial Transfers".
  static String _joinTail(List<_Token> tail) {
    final out = <String>[];
    for (var i = 0; i < tail.length; i++) {
      final t = tail[i];
      if (t.adjective && out.isNotEmpty) {
        out.insert(out.length - 1, t.text);
      } else {
        out.add(t.text);
      }
    }
    return out.where((s) => s.isNotEmpty).join(' ');
  }

  /// نقلٌ حرفيّ لكلمة واحدة، مع «ال» التعريف بادئةً منفصلة.
  static String _translit(String word) {
    var w = word;
    var prefix = '';

    // «ال» التعريف تُكتب "Al" منفصلةً — وهي الصيغة المعتادة في أسماء
    // الشركات: "Al Amana" لا "Alamana".
    if (w.length > 3 && (w.startsWith('ال') || w.startsWith('أل'))) {
      prefix = 'Al ';
      w = w.substring(2);
    }

    final chars = w.split('');
    final sb = StringBuffer();
    for (var i = 0; i < chars.length; i++) {
      final ch = chars[i];

      // «و» و«ي» ساكنان في أول الكلمة (w / y) وحرفا مدٍّ في وسطها (u / i).
      // بدون هذا التفريق يخرج «النور» ⇦ "Al Nwr" — وهي كتابةٌ لا تُقرأ.
      if (i > 0 && (ch == 'و' || ch == 'ي')) {
        sb.write(ch == 'و' ? 'u' : 'i');
        continue;
      }

      final mapped = _letters[ch];
      if (mapped != null) {
        sb.write(mapped);
      } else if (RegExp(r'[0-9A-Za-z\-.]').hasMatch(ch)) {
        // أرقامٌ أو حروف لاتينية في الاسم تمرّ كما هي.
        sb.write(ch);
      }
    }

    final body = sb.toString();
    if (body.isEmpty) return prefix.trim();
    return prefix + body[0].toUpperCase() + body.substring(1);
  }
}

class _Token {
  const _Token(this.text, {this.adjective = false});
  final String text;
  final bool adjective;
}
