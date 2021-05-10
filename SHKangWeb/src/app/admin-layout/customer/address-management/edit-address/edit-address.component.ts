import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AddressService } from 'src/app/admin-layout/services/address.services';
import { CommonService } from 'src/app/admin-layout/services/common.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';

@Component({
  selector: 'app-edit-address',
  templateUrl: './edit-address.component.html',
  styleUrls: ['./edit-address.component.scss'],
})
export class EditAddressComponent implements OnInit {
  @Input() address;
  @Input() selectedUser;
  addressFormObj  : any = {};
  country: any;
  state: any;
  singleSelectsettings={
    enableSearchFilter:true,
    singleSelection:true,
  }
  stateselected=[];
  countryselected=[];
 
  constructor(private modal: NgbActiveModal,private addressService : AddressService,private commonService: CommonService) {}
  ngOnInit(): void {
    if (this.address) {
      this.addressFormObj = this.address;
    }

    this.getCountries();
  }
  cancel() {
    this.modal.close(EditAddressComponent);
  }

  getCountries(){
    this.commonService.getCountries().subscribe((response: any) => {
      if (response.result) {
        this.country = response.data;
        if(this.addressFormObj && this.addressFormObj.countryId){
          this.countryselected.push({id:this.addressFormObj.countryId,itemName:this.addressFormObj.country});
          this.getStates();
        }
        
      }else{
        Swal.fire({
          title: 'Error',
          text: response.message,
          icon: 'error',
          showCancelButton: false,
          confirmButtonText: 'Ok'
        }).then((result) => { })
      }
    });
  }

  getStates(){
    if(this.countryselected && this.countryselected.length > 0){
      this.commonService.getStates(this.countryselected[0].id).subscribe((response: any) => {
        if (response.result) {
          this.state = response.data;
          if(this.addressFormObj && this.addressFormObj.stateId){
            this.stateselected.push({id:this.addressFormObj.stateId,itemName:this.addressFormObj.state})
          }
        }else{
          Swal.fire({
            title: 'Error',
            text: response.message,
            icon: 'error',
            showCancelButton: false,
            confirmButtonText: 'Ok'
          }).then((result) => { })
        }
      });
    }else{
      this.state = [];
      this.stateselected = [];
    }
    
  }

  submit() {
    let params = {
      UserAddressId : this.addressFormObj.userAddressId ? this.addressFormObj.userAddressId : 0,
      Address1 : this.addressFormObj.address1,
      Address2 : this.addressFormObj.address2,
      City : this.addressFormObj.city,
      CountryId : this.countryselected[0].id,
      StateId : this.stateselected[0].id,
      Zipcode : this.addressFormObj.zipcode,
      UserId : this.selectedUser,
      FullName : this.addressFormObj.fullName,
      CompanyName : this.addressFormObj.companyName,
      Email : this.addressFormObj.emailId,
      PhoneNumber : this.addressFormObj.phoneNumber,
    }
   this.addressService.createItem(params).subscribe((response: any) => {
     if (response.result) {
       this.modal.close();
     }else{
       Swal.fire({
         title: 'Error',
         text: response.message,
         icon: 'error',
         showCancelButton: false,
         confirmButtonText: 'Ok'
       }).then((result) => { })
     }
   });
  }

  onStateSelect(event){

  }

  onCountrySelect(event){
    this.getStates();
  }

  onCountryDeSelect(event){
    this.state = [];
    this.stateselected = [];
  }


}
