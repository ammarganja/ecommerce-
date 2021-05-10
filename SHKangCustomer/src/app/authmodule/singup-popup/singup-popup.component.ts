import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/core/services/auth/auth.service';
import { LoginpopupComponent } from '../loginpopup/loginpopup.component';

@Component({
  selector: 'app-singup-popup',
  templateUrl: './singup-popup.component.html',
  styleUrls: ['./singup-popup.component.scss'],
})
export class SingupPopupComponent implements OnInit {
  passError:any;
  constructor(private model: NgbActiveModal, private modelservice: NgbModal,public authservice:AuthService,public toastrservuce:ToastrService) {}

  signupformobj = {
    email: '',
    password: '',
    terms: false,
    company_name: '',
    confirm_password: '',
    contact: '',
    fullname:""
  };
  ngOnInit(): void {}
  backtologin() {
    this.model.dismiss();
    this.modelservice.open(LoginpopupComponent, {
      windowClass: 'login-modal in',
      centered: true,
    });
  }
  close() {
    this.model.dismiss();
  }
  submit() {
    let data = {
      UserId: '0',
      FirstName: this.signupformobj.fullname,
      LastName: '',
      PhoneNumber: this.signupformobj.contact,
      EmailAddress: this.signupformobj.email,
      Password: this.signupformobj.password,
      CompanyName: this.signupformobj.company_name,
      RoleType: '2',
    };
    this.authservice.register(data).subscribe((res:any)=>{
      if(res.result){
        this.toastrservuce.success(res.data)
        this.model.close();
      }else{
        this.toastrservuce.error(res.message)
      }
    })
  }

  cofirmPassword(){
    if(this.signupformobj.password != this.signupformobj.confirm_password){
      this.passError = "Confirm Password is Not matched with Passwoed";
    }else{
      this.passError = "";
    }
  }
}
