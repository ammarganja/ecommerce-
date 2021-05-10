import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {LoginComponent} from './login/login.component';
import {AuthComponent} from './auth.component';
import { ForgotpasswordComponent } from './forgotpassword/forgotpassword.component';
import { ResetpasswordComponent } from './resetpassword/resetpassword.component';
import { EmailmessageComponent } from './emailmessage/emailmessage.component';

const routes: Routes = [
  {
    path: '',
    component: AuthComponent,
    children: [
      {
        path: 'login',
        component: LoginComponent,
      },
      {
        path: 'forgotpassword',
        component: ForgotpasswordComponent,
      },
      {
        path: 'resetpassword',
        component: ResetpasswordComponent,
      },
      {
        path: 'emailmessage',
        component: EmailmessageComponent,
      },
      {
        path: '', redirectTo: 'login', pathMatch: 'full'
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
