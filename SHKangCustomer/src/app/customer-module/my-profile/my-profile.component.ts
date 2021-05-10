import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/core/services/auth/auth.service';
import { UserService } from 'src/app/core/services/customer_services/user.service';
import { Base64Service } from 'src/app/core/utils/base64.service';
import { EditUserAddressComponent } from '../edit-user-address/edit-user-address.component';
import { UserPersonalInfoComponent } from '../user-personal-info/user-personal-info.component';

@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.scss']
})
export class MyProfileComponent implements OnInit {

  //#region  Variable Declaration
  userDetail: any;
  userAddressList : any = [];
  //#endregion

  //#region Constructor
  constructor(private authService: AuthService, private toastr: ToastrService, private Base64Service: Base64Service, private ngbModal: NgbModal,private userService : UserService) { }
  //#endregion

  //#region Functions
  ngOnInit(): void {
    this.getUserProfileDetail();
    this.getUserAddresses();
  }

  /**
   * get user profile detail
   */
  getUserProfileDetail() {
    var userData: any = JSON.parse(Base64Service.decode(localStorage.getItem('userdata')));
    let requestParam: any;
    requestParam = {
      UserId: userData.userId
    }
    this.authService
      .getUserDetail(requestParam)
      .subscribe((response: any) => {
        if (response.result && response.status == 200) {
          this.userDetail = response.data;
          this.userDetail.firstName = this.userDetail.firstName + (this.userDetail.lastName ? ' ' + this.userDetail.lastName : '');
          this.userDetail.lastName = '';

        } else {
          this.toastr.error(response.message);
          return;
        }
      });
  }

  editUserDetail() {
    var modalRef = this.ngbModal.open(UserPersonalInfoComponent, {
      windowClass: 'login-modal in',
      centered: true,
    });
    modalRef.componentInstance.userDetail = this.userDetail;
    modalRef.result.then((res) => {
      this.getUserProfileDetail();
    });
  }

  getUserAddresses() {
    var userData: any = JSON.parse(Base64Service.decode(localStorage.getItem('userdata')));
    var params = {
      "userId": userData.userId,
      "pageNo": 1,
      "limit": 1000,
      "searchString": "",
      "column": "",
      "direction": ""
    }

    this.userService.getUserAddresses(params).subscribe((response:any)=>{
      if(response.status = 200 && response.result){
        this.userAddressList = response.data.items;
      }else{
        this.toastr.error(response.message);
        return;
      }
    })
  }

  editAddress(address:any= {}){
    var modalRef = this.ngbModal.open(EditUserAddressComponent, {
      windowClass: 'login-modal in',
      centered: true,
    });
    modalRef.componentInstance.address = address;
    modalRef.result.then((res) => {
      this.getUserAddresses();
    });
  }

  //#endregion
}
