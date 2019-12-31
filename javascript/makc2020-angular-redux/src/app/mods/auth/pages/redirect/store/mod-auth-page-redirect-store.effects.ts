// //Author Maxim Kuzmin//makc//

import {Injectable} from '@angular/core';
import {Actions, Effect, ofType} from '@ngrx/effects';
import {Action} from '@ngrx/store';
import {Observable} from 'rxjs';
import {map, switchMap} from 'rxjs/operators';
import {AppCoreLoggingService} from '@app/core/logging/core-logging.service';
import {AppHostAuthService} from '@app/host/auth/host-auth.service';
import {AppHostAuthStore} from '@app/host/auth/host-auth-store';
import {AppModAuthPageRedirectEnumActions} from '../enums/mod-auth-page-redirect-enum-actions';
import {AppModAuthPageRedirectStoreActions} from './mod-auth-page-redirect-store.actions';
import {AppModAuthPageRedirectStoreActionLoadSuccess} from './actions/mod-auth-page-redirect-store-action-load-success';

/** Мод "Auth". Страницы. Перенаправление. Хранилище состояния. Эффекты. */
@Injectable()
export class AppModAuthPageRedirectStoreEffects {

  /**
   * Загрузка.
   * @returns {Observable<Action>} Поток действий.
   */
  @Effect()
  load$: Observable<Action> = this.extActions$.pipe(
    ofType(AppModAuthPageRedirectEnumActions.Load),
    switchMap(
      action => {
        return this.appAuth.loadCurrentUser$(this.appLogger).pipe(
          map(
            result => {
              const {
                currentUser,
                isLoggedIn,
                redirectUrl
              } = this.appAuthStore.getState();

              return new AppModAuthPageRedirectStoreActionLoadSuccess(
                currentUser,
                isLoggedIn,
                result,
                redirectUrl
              );
            }
          )
        );
      }
    )
  );

  /**
   * Конструктор.
   * @param {AppHostAuthService} appAuth Аутентификация.
   * @param {AppHostAuthStore} appAuthStore Хранилище состояния аутентификации.
   * @param {AppCoreLoggingService} appLogger Регистратор.
   * @param {Actions<AppModAuthPageRedirectStoreActions>} extActions$ Поток действий.
   */
  constructor(
    private appAuth: AppHostAuthService,
    private appAuthStore: AppHostAuthStore,
    private appLogger: AppCoreLoggingService,
    private extActions$: Actions<AppModAuthPageRedirectStoreActions>
  ) {
  }
}
