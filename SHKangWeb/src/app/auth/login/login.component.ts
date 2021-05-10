import { Component, OnInit } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { Login } from 'src/app/admin-layout/_models/login.model';
import { loginService } from 'src/app/admin-layout/services/login.services';
import Swal from 'sweetalert2/dist/sweetalert2.js';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  model: any = {};
  loading = false;

  loginFormObj = {
    EmailAddress: null,
    Password: null,
    IsAdmin: 1,
  };
  hasError: boolean;
  returnUrl: any;
  isLoading$: Observable<boolean>;

  // private fields
  private unsubscribe: Subscription[] = []; // Read more: => https://brianflove.com/2016/12/11/anguar-2-unsubscribe-observables/
  user = null;
  LoginError: any;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private loginService: loginService
  ) {
    if (localStorage.UserSession) {
      this.router.navigate(['dashboard']);
    }
    /*    this.isLoading$ = this.authService.isLoading$;
    // redirect to home if already logged in
    if (this.authService.currentUserValue) {
      this.router.navigate(['/']);
    }*/
  }

  ngOnInit(): void {
    // get return url from route parameters or default to '/'
    // this.returnUrl =
      // this.route.snapshot.queryParams['returnUrl'] || '/dashboard';
  }

  submit() {
    this.loginService.login(this.loginFormObj).subscribe((res) => {
      this.user = res;
      if (this.user.result == true) {
        localStorage.setItem('token', this.user.data.token);
        localStorage.setItem('userdata', JSON.stringify(this.user.data));
        let user = localStorage.getItem('returnUrl'); 
        if(user){     
          this.returnUrl = JSON.parse(user);
        }
        this.router.navigate([this.returnUrl || '/dashboard']);
        // this.router.navigate(['dashboard']);
      } else {
        Swal.fire({
          title: 'Error',
          text: this.user.message,
          icon: 'error',
          showCancelButton: false,
          confirmButtonText: 'Ok'
        }).then((result) => { })
      }
    });
    /*
    this.hasError = false;
    const loginSubscr = this.authService
      .login(this.f.email.value, this.f.password.value)
      .pipe(first())
      .subscribe((user: UserModel) => {
        if (user) {
          this.router.navigate([this.  returnUrl]);
        } else {
          this.hasError = true;
        }
      });
    this.unsubscribe.push(loginSubscr);*/
  }

  ngOnDestroy() {
    this.unsubscribe.forEach((sb) => sb.unsubscribe());
  }
}
