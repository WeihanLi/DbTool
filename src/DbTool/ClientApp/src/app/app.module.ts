import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CodeFirstComponent } from './code-first/code-first.component';
import { DbFirstComponent } from './db-first/db-first.component';
import { ModelFirstComponent } from './model-first/model-first.component';
import { AppSettingsComponent } from './app-settings/app-settings.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CodeFirstComponent,
    DbFirstComponent,
    ModelFirstComponent,
    AppSettingsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'home', component: HomeComponent },
      { path: 'codeFirst', component: CodeFirstComponent },
      { path: 'dbFirst', component: DbFirstComponent },
      { path: 'modelFirst', component: CodeFirstComponent },
      { path: 'settings', component: AppSettingsComponent },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
