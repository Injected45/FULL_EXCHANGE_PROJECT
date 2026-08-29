import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/net/api_client.dart';
import '../../core/storage/secure_store.dart';
import 'auth_repository.dart';

enum AuthStatus { unknown, signedOut, signedIn }

class AuthState {
  const AuthState({required this.status, this.user, this.onboarded = false});

  final AuthStatus status;
  final AgentUser? user;
  final bool onboarded;

  AuthState copyWith({AuthStatus? status, AgentUser? user, bool? onboarded}) =>
      AuthState(
        status: status ?? this.status,
        user: user ?? this.user,
        onboarded: onboarded ?? this.onboarded,
      );

  static const initial = AuthState(status: AuthStatus.unknown);
}

class AuthController extends StateNotifier<AuthState> {
  AuthController(this._repo, this._store) : super(AuthState.initial) {
    _bootstrap();
  }

  final AuthRepository _repo;
  final SecureStore _store;

  Future<void> _bootstrap() async {
    final onboarded = await _store.readOnboarded();
    final user = await _repo.restore();
    if (!mounted) return;
    state = AuthState(
      status: user == null ? AuthStatus.signedOut : AuthStatus.signedIn,
      user: user,
      onboarded: onboarded,
    );
  }

  Future<void> markOnboarded() async {
    await _store.setOnboarded();
    if (!mounted) return;
    state = state.copyWith(onboarded: true);
  }

  void adopt(AuthSession session) {
    state = AuthState(
      status: AuthStatus.signedIn,
      user: session.user,
      onboarded: true,
    );
  }

  Future<void> signOut() async {
    await _repo.signOut();
    if (!mounted) return;
    state = AuthState(status: AuthStatus.signedOut, onboarded: state.onboarded);
  }
}

final authControllerProvider =
    StateNotifierProvider<AuthController, AuthState>((ref) {
  final controller = AuthController(
    ref.watch(authRepositoryProvider),
    ref.watch(secureStoreProvider),
  );

  // جلسة منتهية (401) ⇒ خروج فوري. بدون هذا يبقى الرمز الميت في التخزين،
  // ويظل الراوتر يعدّ الوكيل داخل التطبيق فيرى خطأً على كل شاشة بلا مخرج.
  // الوصل هنا لا في core/net: الاتجاه features ← core، والراوتر يراقب هذا
  // المزوّد دائماً فلا يمكن أن يُنسى الوصل.
  ref.watch(apiClientProvider).onUnauthorized = () {
    controller.signOut();
  };

  return controller;
});
