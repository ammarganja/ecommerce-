import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { HelperService } from 'src/app/core/helpers/helper-service.service';
import { UserService } from 'src/app/core/services/customer_services/user.service';
import { Base64Service } from 'src/app/core/utils/base64.service';

@Component({
  selector: 'app-user-personal-info',
  templateUrl: './user-personal-info.component.html',
  styleUrls: ['./user-personal-info.component.scss']
})
export class UserPersonalInfoComponent implements OnInit {

  //#region Variable Declaration
  @Input() userDetail: any;
  //#endregion

  //#region Constructor
  constructor(private userService: UserService,private ngbActiveModal : NgbActiveModal,private toaster : ToastrService,public helperService:HelperService) { }

  //#endregion

  //#region Functions
  ngOnInit(): void {
  }

  close() {
    this.ngbActiveModal.close();
  }

  submit() {
    var params = {
      "UserId": this.userDetail.userId,
      "FirstName": this.userDetail.firstName,
      "LastName": this.userDetail.lastName,
      "PhoneNumber": this.userDetail.phoneNumber,
      "EmailAddress": this.userDetail.emailAddress,
      "CompanyName": this.userDetail.companyName
    }
    this.userService.saveUserDetail(params).subscribe((response: any) => {
      if(response.result && response.status == 200){
        var userData : any = JSON.parse(Base64Service.decode(localStorage.getItem('userdata')));
        userData.firstName = this.userDetail.firstName;
        userData.lastName = this.userDetail.lastName;
        userData.phoneNumber = this.userDetail.phoneNumber;
        userData.emailAddress = this.userDetail.emailAddress;
        userData.companyName = this.userDetail.companyName;
        this.helperService.userName = userData.firstName;
        localStorage.setItem('userdata',Base64Service.encode(JSON.stringify(userData)));
        this.ngbActiveModal.close()
      }else{
        this.toaster.error(response.message);
        return;
      }
    })
  }

  //#endregion
}
