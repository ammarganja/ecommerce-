import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/core/services/auth/auth.service';
import { Base64Service } from 'src/app/core/utils/base64.service';

@Component({
  selector: 'app-user-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {

  constructor(public authservice: AuthService, public toastrservice: ToastrService) { }
  confirmpassword: any;

  changepasswordForm = {
    oldPassword: '',
    newPassword: '',
    confirmPassword: ''

  }
  ngOnInit(): void {
  }


  submit() {
    var userData = JSON.parse(Base64Service.decode(localStorage.getItem('userdata')));
    let data = {
      userId : userData.userId,
      oldPassword: this.changepasswordForm.oldPassword,
      newPassword: this.changepasswordForm.newPassword
    }
    this.authservice.changepassword(data).subscribe((res: any) => {
      if (res.result) {
        this.toastrservice.success(res.message)
      } else {
        this.toastrservice.error(res.message)
      }
    })
  }
}
