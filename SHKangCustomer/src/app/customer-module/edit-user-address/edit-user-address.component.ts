import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { HelperService } from 'src/app/core/helpers/helper-service.service';
import { UserService } from 'src/app/core/services/customer_services/user.service';
import { Base64Service } from 'src/app/core/utils/base64.service';

@Component({
  selector: 'app-edit-user-address',
  templateUrl: './edit-user-address.component.html',
  styleUrls: ['./edit-user-address.component.scss']
})
export class EditUserAddressComponent implements OnInit {

  //#region Variable Declaration
  @Input() address: any;
  countryList: any = [];
  stateList: any = [];
  selectedState: any = [];
  selectedCountry: any = [];
  dropdownSettings = {};
  //#endregion

  //#region Constructor
  constructor(private userService: UserService, private ngbActiveModal: NgbActiveModal, private toaster: ToastrService, public helperService: HelperService, private changeDetectionRef: ChangeDetectorRef) { }

  //#endregion

  //#region Functions
  ngOnInit(): void {
    this.dropdownSettings = {
      singleSelection: true,
      idField: 'id',
      textField: 'itemName',
      itemsShowLimit: 5,
      allowSearchFilter: true
    };
    this.getCountryList()
  }

  close() {
    this.ngbActiveModal.close();
  }

  getCountryList() {
    this.userService.getCountries().subscribe((response: any) => {
      if (response.result) {
        this.countryList = response.data;
        if (this.address && this.address.countryId) {
          let arr: any[] = [{ id: this.address.countryId.toString(), itemName: this.address.country }];
          this.selectedCountry = arr;
          this.changeDetectionRef.markForCheck();

          this.getStates();
        }

      } else {
        this.toaster.error(response.message);
        return
      }
    });
  }

  getStates() {
    
    if (this.selectedCountry && this.selectedCountry.length > 0) {
      this.userService.getStates(this.selectedCountry[0].id).subscribe((response: any) => {
        if (response.result) {
          this.stateList = response.data;
          if (this.address && this.address.stateId) {
            let arr: any[] = [{ id: this.address.stateId.toString(), itemName: this.address.state }];
            this.selectedState = arr;
          }
        } else {
          this.toaster.error(response.message)
        }
      });
    } else {
      this.stateList = [];
      this.selectedState = [];
    }
  }

  onStateSelect(event: any) {

  }

  onCountrySelect(event: any) {
    this.stateList = [];
    this.selectedState = [];
    this.getStates();
  }

  onCountryDeSelect(event: any) {
    this.stateList = [];
    this.selectedState = [];
  }

  isPrimary(event: any) {
    if (event && event.target.checked) {
      this.address.isPrimary = true;
    } else {
      this.address.isPrimary = false;
    }
  }

  submit() {

    if(this.selectedCountry.length == 0){
      this.toaster.error("Please select Country");
      return;
    }

    if(this.selectedState.length == 0){
      this.toaster.error("Please select State");
      return;
    }

    var userData = JSON.parse(Base64Service.decode(localStorage.getItem('userdata')));
    var params = {
      "UserAddressId":  this.address.userAddressId ? this.address.userAddressId : 0,
      "Address1": this.address.address1,
      "Address2":this.address.address2,
      "CountryId": this.selectedCountry[0].id,
      "City": this.address.city,
      "StateId": this.selectedState[0].id,
      "ZipCode": this.address.zipcode,
      "IsPrimary" : this.address.isPrimary,
      "FullName" : this.address.fullName ? this.address.fullName : '',
      "PhoneNumber" : this.address.phoneNumber ? this.address.phoneNumber : '',
      "CompanyName" : this.address.companyName ? this.address.companyName : '',
      "Email": this.address.emailId ? this.address.emailId : '',
      "UserId" : userData.userId
    }
    this.userService.saveUserAddress(params).subscribe((response: any) => {
      if (response.result && response.status == 200) {
        this.ngbActiveModal.close()
      } else {
        this.toaster.error(response.message);
        return;
      }
    })
  }

  //#endregion

}
