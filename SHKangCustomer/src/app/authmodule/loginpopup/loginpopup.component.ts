import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from 'src/app/core/services/auth/auth.service';
import { FrontendService } from 'src/app/core/services/frontend.service';
import { ForgotpasswordComponent } from '../forgotpassword/forgotpassword.component';
import { SingupPopupComponent } from '../singup-popup/singup-popup.component';
import { ToastrService } from 'ngx-toastr';
import { Base64Service } from 'src/app/core/utils/base64.service';
import { HelperService } from 'src/app/core/helpers/helper-service.service';


@Component({
  selector: 'app-loginpopup',
  templateUrl: './loginpopup.component.html',
  styleUrls: ['./loginpopup.component.scss'],
})
export class LoginpopupComponent implements OnInit {
  isSessionTrue= false;
  
  isLogIn=false;
  returnUrl: any;
  constructor(
    private model: NgbActiveModal,
    private modelservice: NgbModal,
    private router: Router,
    private authservice: AuthService,
    private toastr: ToastrService,
    private helperService: HelperService
  ) {}

  loginFormObj = {
    email: null,
    password: null,
  };
  ngOnInit(): void {}

  close() {
    this.model.dismiss(LoginpopupComponent);
  }
  signup() {
    this.model.dismiss(LoginpopupComponent);
    this.modelservice.open(SingupPopupComponent, {
      centered: true,
      windowClass: 'login-modal in large_pop',
      modalDialogClass: 'signup-modal',
    });
  }
  forgotpassword() {
    this.model.dismiss(LoginpopupComponent);
    this.modelservice.open(ForgotpasswordComponent, {
      windowClass: 'login-modal in',
      centered: true,
    });
  }
  submit() {
    let data = {
      EmailAddress: this.loginFormObj.email,
      Password: this.loginFormObj.password,
      IsAdmin: 0,
    };
    this.authservice.login(data).subscribe((res: any) => {
      if (res.result) {
        let data = localStorage.getItem('returnUrl');
        if(data){
          this.returnUrl = JSON.parse(data);
          this.router.navigate([this.returnUrl || '/']);
        }
        
        this.isSessionTrue = res.results;
        this.toastr.success(res.message)
        localStorage.setItem('token',res.data.token)
        localStorage.setItem('userdata',Base64Service.encode(JSON.stringify(res.data)));
        this.helperService.userName = res.data.firstName + (res.data.lastName ? ' '+ res.data.lastName : '')
        this.helperService.isLogIn = true;
        this.model.dismiss();
      } else {
        this.helperService.isLogIn = false;
        this.toastr.error(res.message)
      }
    });
  }
}
