import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';
import { NgxsReduxDevtoolsPluginModule } from '@ngxs/devtools-plugin';
import { NgxsModule } from '@ngxs/store';
import { NgxsStoragePluginModule } from '@ngxs/storage-plugin';

import { routes } from './app.routes';
import { GroceryListState } from './grocery-list/ngxs-store/grocery-list.state';
import { IngredientState } from './grocery-list/ngxs-store/ingredient.state';
import { provideHttpClient } from '@angular/common/http';
import { AuthState } from './auth/ngxs-store/auth.state';
import { JwtModule } from '@auth0/angular-jwt';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes), provideAnimations(), provideHttpClient(), importProvidersFrom(NgxsModule.forRoot([GroceryListState, IngredientState, AuthState]), NgxsReduxDevtoolsPluginModule.forRoot(), NgxsStoragePluginModule.forRoot({ key: AuthState }), JwtModule.forRoot({
    config: {
      tokenGetter: undefined
    },
  }))]
};

