import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal, NgbDatepicker, NgbDateStruct, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CouponsService } from '../../../services/coupon.services';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { HelperService } from 'src/app/admin-layout/services/helper.service';

@Component({
  selector: 'app-edit-coupon',
  templateUrl: './edit-coupon.component.html',
  styleUrls: ['./edit-coupon.component.scss']
})
export class EditCouponComponent implements OnInit {
  
  //#region Variable Declaration
  @Input() coupon: any;
  coupontype: { id: number; itemName: string; }[];
  coupontypeModel: any[];
  mindate: any;
  isSelectd: boolean = false;
  isAmount: boolean = false;
  isPercentage: boolean = false;
  isSelectdItem:any;
  singleSelectsettings:any;
  start_date: NgbDateStruct;
  expiry_date: NgbDateStruct;
  
  couponFormObj = {
    PromoCodeId : 0,
    Name:null,
    Code:null,
    ExpiryDate : null,
    StartDate : null,
    Amount : '0',
    Percentage : '0.00',
    Description: null,
    CreatedBy : null,
    UpdatedBy : null,
    discount : null,
    Status : 0
  }
  //#endregion

  //#region Constcructor
  constructor(private modal: NgbActiveModal,private couponService : CouponsService,private helperService : HelperService) { }
  //#endregion
  

  //#region Methods

  /**
   * NgOnInit
  */
  ngOnInit() {
    if (this.coupon) {
      this.couponFormObj = this.coupon;
      if(this.couponFormObj.Percentage !== '0.00'){
        this.isPercentage = true;
        this.isAmount = false;
        this.coupontypeModel = [{id: 2, itemName: 'Percentage'}];
        this.couponFormObj.discount = this.couponFormObj.Percentage;
      }
      else if(this.couponFormObj.Amount !== '0'){
        this.isPercentage = false;
        this.isAmount = true;
        this.coupontypeModel = [{id: 1, itemName: 'Amount'}];
        this.couponFormObj.discount = this.couponFormObj.Amount;
      }else{
        this.isPercentage = false;
        this.isAmount = false;
        this.coupontypeModel = [];
        this.couponFormObj.discount = null;
      }
    }

    this.coupontype = [
      { id: 1, itemName: 'Amount' },
      { id: 2, itemName: 'Percentage' },
    ]

    this.singleSelectsettings = {
      singleSelection: true,
      enableSearchFilter: false,
    };
  }

  /**
   * cancel
  */
  cancel() {
    this.modal.close(EditCouponComponent);
  }

  /**
   * submit
  */
  submit() {
    var obj = this.couponFormObj;

    if(this.isAmount){
      obj.Amount = this.couponFormObj.discount;
      obj.Percentage = '0.00';
    }

    else if(this.isPercentage){
      obj.Percentage = this.couponFormObj.discount;
      obj.Amount = '0';
    }

    if(this.couponFormObj.PromoCodeId == 0){
      obj.Status = 1;
    }

    if(this.couponFormObj.StartDate){
      var year = this.couponFormObj.StartDate.year;
      var month = this.couponFormObj.StartDate.month;
      var day = this.couponFormObj.StartDate.day;
      obj.StartDate = year + '-' + month + '-' + day;
    }

    if(this.couponFormObj.ExpiryDate){
      var year = this.couponFormObj.ExpiryDate.year;
      var month = this.couponFormObj.ExpiryDate.month;
      var day = this.couponFormObj.ExpiryDate.day;
      obj.ExpiryDate = year + '-' + month + '-' + day;
    }

    var user = this.helperService.getUserDetail();

    obj.CreatedBy = user.userId;
    obj.UpdatedBy = user.userId;

    this.couponService.createItem(obj).subscribe((response: any) => {
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

  /**
   * coupon item select
  */
  oncouponsItemSelect(e) {
    if (e && e.id) {
      this.isSelectd = true;
      this.isSelectdItem = e.id;
      if (e.itemName == 'Amount') {
        this.isAmount = true;
        this.isPercentage = false;
      }
      if (e.itemName == 'Percentage') {
        this.isPercentage = true;
        this.isAmount = false;
      }
      this.coupontype = [
        { id: 1, itemName: 'Amount' },
        { id: 2, itemName: 'Percentage' },
      ]
    } else {
      this.isSelectd = false;
      this.isSelectdItem = '';
      this.coupontype = [];
    }
  }
  //#endregion
}
