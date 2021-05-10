import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/core/services/auth/auth.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
})
export class ResetPasswordComponent implements OnInit {
  @Input() code:any;
  passError: any;
  codes: any;

  constructor(
    public route: ActivatedRoute,
    public router: Router,
    public authservice: AuthService,
    public toastrservice: ToastrService
  ) {}

  resetFormObj = {
    password: '',
    confirm_password: '',
  };
  ngOnInit(): void {
    this.codes = this.code;
  }


  confirmPass() {
    if (this.resetFormObj.password != this.resetFormObj.confirm_password) {
      this.passError = 'ConfirmPassword is not matched with password';
    } else {
      this.passError = '';
    }
  }
  submit(){
    let formdata = {
      password: this.resetFormObj.password,
      code: this.codes,
    };
    this.authservice.resetpassword(formdata).subscribe((res: any) => {
      if (res.result) {
        this.toastrservice.success(res.data);
        this.router.navigate(['/customer/home'])
      } else {
        this.toastrservice.error(res.message);
      }
    });
  }
}
