import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ResetPasswordComponent } from '../authmodule/reset-password/reset-password.component';
import { HomeComponent } from '../customer-module/home/home.component';
import { AboutUsComponent } from './components/about-us/about-us.component';
import { Forbidden403Component } from './components/forbidden403/forbidden403.component';
import { InternalServer500Component } from './components/internal-server500/internal-server500.component';
import { NotFound404Component } from './components/not-found404/not-found404.component';
import { PrivacyComponent } from './components/privacy/privacy.component';
import { TermsComponent } from './components/terms/terms.component';
import { UnAuhtorized401Component } from './components/un-auhtorized401/un-auhtorized401.component';

const routes: Routes = [
  { path: '403', component: Forbidden403Component },
  { path: '401', component: UnAuhtorized401Component },
  { path: '500', component: InternalServer500Component },
  { path: '404', component: NotFound404Component },
  { path: 'resetpassword/:code', component: HomeComponent },
  { path: 'aboutus', component: AboutUsComponent },
  { path: 'terms', component: TermsComponent },
  { path: 'privacy', component: PrivacyComponent },
  { path: '**', redirectTo: '/404' },
  { path: '500', redirectTo: '/500' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreRoutingModule { }
