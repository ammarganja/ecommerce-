import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { resetpasswordService } from 'src/app/admin-layout/services/resetpassword.services';
import Swal from 'sweetalert2/dist/sweetalert2.js';


@Component({
  selector: 'app-resetpassword',
  templateUrl: './resetpassword.component.html',
  styleUrls: ['./resetpassword.component.scss'],
})
export class ResetpasswordComponent implements OnInit {
  resetFormObj = {
    password: null,
    confirmpassword: null,
    code: null,
  };
  users:any={}
  hasError: boolean;
  returnUrl: string;
  isLoading$: Observable<boolean>;

  // private fields
  private unsubscribe: Subscription[] = [];
  confirmPassError: string;
  errorMsg: any;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private resetpasswordservice: resetpasswordService
  ) {}

  ngOnInit(): void {
    this.resetFormObj.code =
      this.route.snapshot.queryParams['code'.toString()] || '/';
  }

  submit() {
    this.hasError = false;
    let formdata = {
      password:this.resetFormObj.password,
      code:this.resetFormObj.code
    };
    this.resetpasswordservice.resetPassword(formdata).subscribe((user) => {
      this.users = user;
      if(this.users.result == true){
        this.router.navigate(['login'])
      }else{
        
        Swal.fire({
          title: 'Error',
          text: this.users.message,
          icon: 'error',
          showCancelButton: false,
          confirmButtonText: 'Ok'
        }).then((result) => { })
        
      }
    });
  }

  ngOnDestroy() {
    this.unsubscribe.forEach((sb) => sb.unsubscribe());
  }

  checkpassword() {
    if (this.resetFormObj.password == this.resetFormObj.confirmpassword) {
      this.confirmPassError = '';
    } else {
      this.confirmPassError = 'CofirmPassword is not match with password';
    }
  }
}
