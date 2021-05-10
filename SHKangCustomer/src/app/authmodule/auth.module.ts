import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthRoutingModule } from './auth-routing.module';
import { AuthComponent } from './auth.component';
import { LoginComponent } from './login/login.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { LoginpopupComponent } from './loginpopup/loginpopup.component';
import { SingupPopupComponent } from './singup-popup/singup-popup.component';
import { ForgotpasswordComponent } from './forgotpassword/forgotpassword.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { ToastContainerModule, ToastrModule, ToastrService } from 'ngx-toastr';


@NgModule({
  declarations: [
    AuthComponent,
    LoginComponent,
    ResetPasswordComponent,
    LoginpopupComponent,
    SingupPopupComponent,
    ForgotpasswordComponent,
  ],
  imports: [
    CommonModule,
    AuthRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    NgbDropdownModule,
    ToastrModule,
    ToastContainerModule,
  ]
})
export class AuthModule { }
