import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/theme/tokens.dart';
import '../../ui/widgets/controls.dart';
import '../../ui/widgets/glass.dart';
import '../security/security_mode_card.dart';

/// إعداداتُ أمان الموظف — زرٌّ افتراضيّ لا صلاحية: يختار الموظف حماية دخول
/// جهازه (بصمة · نمط الجهاز · بلا حماية) بنفسه، ولا يحكمها الوكيل.
class EmployeeSecurityScreen extends ConsumerWidget {
  const EmployeeSecurityScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return Screen(
      child: Column(
        children: [
          RhallaAppBar(
            title: 'الأمان',
            subtitle: 'حماية الدخول على جهازك',
            onBack: () => Navigator.of(context).maybePop(),
          ),
          const Expanded(
            child: SingleChildScrollView(
              padding: EdgeInsets.fromLTRB(
                  R.padScreen, 20, R.padScreen, 40),
              child: SecurityModeCard(),
            ),
          ),
        ],
      ),
    );
  }
}
