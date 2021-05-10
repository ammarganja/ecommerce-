import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/core/services/auth/auth.service';
import { LoginpopupComponent } from '../loginpopup/loginpopup.component';

@Component({
  selector: 'app-forgotpassword',
  templateUrl: './forgotpassword.component.html',
  styleUrls: ['./forgotpassword.component.scss']
})
export class ForgotpasswordComponent implements OnInit {

  constructor(private model:NgbModal,private modelservice:NgbActiveModal,public authservice:AuthService,public toastrservice:ToastrService) { }

  forgotpasswordForm ={
    emailId:''
  }
  ngOnInit(): void {
  }
  login(){
    this.modelservice.dismiss(LoginpopupComponent)
    this.model.open(LoginpopupComponent,{windowClass:'login-modal in',centered:true})
  }
  close(){
    this.modelservice.dismiss(LoginpopupComponent)
  }
  submit(){
    if(this.forgotpasswordForm.emailId){
      let data = {
        emailId:this.forgotpasswordForm.emailId
      }
      this.authservice.forgotPassword(data).subscribe((res:any)=>{
          if(res.result){
           this.toastrservice.success(res.message)
           this.modelservice.dismiss();
          }else{
            this.toastrservice.error(res.message)
          }
      })
    }

  }

}
