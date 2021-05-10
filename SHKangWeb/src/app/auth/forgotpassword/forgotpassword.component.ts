import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { first } from 'rxjs/operators';
import { AuthService } from 'src/app/admin-layout/auth.service';
import { forgotpasswordService } from 'src/app/admin-layout/services/forgotpassword.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';

enum ErrorStates {
  NotSubmitted,
  HasError,
  NoError,
}
@Component({
  selector: 'app-forgotpassword',
  templateUrl: './forgotpassword.component.html',
  styleUrls: ['./forgotpassword.component.scss'],
})
export class ForgotpasswordComponent implements OnInit {
  forgotPasswordForm: FormGroup;
  errorState: ErrorStates = ErrorStates.NotSubmitted;
  errorStates = ErrorStates;
  isLoading$: Observable<boolean>;

  // private fields
  private unsubscribe: Subscription[] = []; // Read more: => https://brianflove.com/2016/12/11/anguar-2-unsubscribe-observables/

  forgotPasswordFormObj = {
    emailId: null,
  };
  res: any = {};
  forgotpasswordError: string;
  forgotpasswordErrorFlag = false;

  forgotpasswordErrorMessage: any = {};
  resultsSuccess: any = {};
  constructor(
    private fb: FormBuilder,
    private forgotpasswordservice: forgotpasswordService,
    private router: Router
  ) {
    // this.isLoading$ = this.authService.isLoading$;
  }

  ngOnInit(): void {}

  submit() {
    this.forgotpasswordservice
      .forgot_password(this.forgotPasswordFormObj)
      .subscribe((results) => {
        this.resultsSuccess = results;
        if (this.resultsSuccess.result == false) {
          Swal.fire({
            title: 'Error',
            text: this.resultsSuccess.message,
            icon: 'error',
            showCancelButton: false,
            confirmButtonText: 'Ok',
          }).then((result) => {});
        }
      });
  }
  redirectLogin() {
    this.router.navigate(['login']);
  }
}
