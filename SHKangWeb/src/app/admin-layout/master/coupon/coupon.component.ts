import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { NgbDropdownModule, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { CouponsService } from '../../services/coupon.services';
import { Column } from '../../_models/columns.model';
import { EditCouponComponent } from './edit-coupon/edit-coupon.component';
import { couponData } from "./coupon.data";
import { ConfirmationpopupComponent } from 'src/app/core/components/confirmationpopup/confirmationpopup.component';
import { DeleteconfirmationComponent } from 'src/app/core/components/deleteconfirmation/deleteconfirmation.component';
import { HelperService } from '../../services/helper.service';
import { PaginatorState, SortState, GroupingState } from 'src/app/shared/crud-table';
import Swal from 'sweetalert2/dist/sweetalert2.js';

@Component({
  selector: 'app-coupon',
  templateUrl: './coupon.component.html',
  styleUrls: ['./coupon.component.scss']
})

export class CouponComponent implements
  OnInit,
  OnDestroy {
  
  //#region Variable Declaration
  paginator: PaginatorState;
  sorting: SortState;
  grouping: GroupingState;
  isLoading: boolean;
  filterGroup: FormGroup;
  searchGroup: FormGroup;
  private subscriptions: Subscription[] = [];
  @ViewChild('ngbDropdown') d: NgbDropdownModule;
  couponList:any[];
  Columns:Column[];
  changecouponStatusName: "Promo Code";
  searchString = '';
  //#endregion
  
  //#region Constcructor
  constructor(
    private fb: FormBuilder,
    private modalService: NgbModal,
    public couponService: CouponsService,
    public helperService:HelperService
  ) { }
  //#endregion
  
  //#region Methods

  /**
   * NgOnInit
  */
  ngOnInit(): void {
    var filter = {
      "searchString": ""
    }
    
    this.couponList = couponData;
    this.couponService.paginator.page = 1;
    this.couponService.paginator.pageSize = 10;
    this.couponService.paginator.pageSizes = [];
    this.grouping = this.couponService.grouping;
    this.paginator = this.couponService.paginator;
    this.sorting = this.couponService.sorting;
    const sb = this.couponService.isLoading$.subscribe(
      (res) => (this.isLoading = res)
    );
    this.subscriptions.push(sb);
    this.Columns = this.helperService.getPromoCodeColumns();

    this.couponService.patchState({ filter })
  }

  /**
   * NgOnDestroy
  */
  ngOnDestroy() {
    this.subscriptions.forEach((sb) => sb.unsubscribe());
  }

  /**
   * filter
   */
  filter() {
    const filter = {};
    const status = this.filterGroup.get('status').value;
    if (status) {
      filter['status'] = status;
    }

    const type = this.filterGroup.get('type').value;
    if (type) {
      filter['type'] = type;
    }
    this.couponService.patchState({ filter });
  }

  /**
   * search
  */
  search(searchTerm: string) {
    this.couponService.paginator.page = 1;
    var filter = {
      "searchString": searchTerm
    }
    this.couponService.patchState({ filter });
  }

  /**
   * sort
  */
  sort(column: string) {
    const sorting = this.sorting;
    const isActiveColumn = sorting.column === column;
    if (!isActiveColumn) {
      sorting.column = column;
      sorting.direction = 'asc';
    } else {
      sorting.direction = sorting.direction === 'asc' ? 'desc' : 'asc';
    }
    this.couponService.patchState({ sorting });
  }

  /**
   * pagination
  */
  paginate(paginator: PaginatorState) {
    this.couponService.patchState({ paginator });
  }

  /**
   * create
  */
  create() {
    this.edit(undefined);
  }

  /**
   * edit
  */
  edit(coupon: any) {
    let user = this.helperService.getUserDetail();
    if(coupon){

      if(coupon.expiryDate){
        var year = new Date(coupon.expiryDate).getFullYear();
        var month = new Date(coupon.expiryDate).getMonth() + 1;
        var day = new Date(coupon.expiryDate).getDate();
        coupon.expiryDate = {
          year: year,
          month: month,
          day: day
        }
      }

      if(coupon.startDate){
        var year = new Date(coupon.startDate).getFullYear();
        var month = new Date(coupon.startDate).getMonth() + 1;
        var day = new Date(coupon.startDate).getDate();
        coupon.startDate = {
          year: year,
          month: month,
          day: day
        }
      }

      var promoCode = {
        PromoCodeId : coupon.promoCodeId,
        Name:coupon.name,
        Code:coupon.code,
        ExpiryDate : coupon.expiryDate,
        StartDate : coupon.startDate,
        Amount : coupon.amount,
        Percentage : coupon.percentage,
        Description: coupon.description,
        CreatedBy : user.userId,
        UpdatedBy : user.userId,
        Status : coupon.status
      }
    }

    const modalRef = this.modalService.open(EditCouponComponent, {
      size: 'xl',
    });

    modalRef.componentInstance.coupon = promoCode;
    modalRef.result.then((res) => {
      this.couponService.fetch();
    });
  }
  
  /**
   * delete
  */
  delete(id: number) {
    var user = this.helperService.getUserDetail();
    const modalRef = this.modalService.open(DeleteconfirmationComponent);
    let data = {
      id: id,
      key: 'Promo Code',
    }
    modalRef.componentInstance.data = data;
    modalRef.result.then((res) => {
      if(res === 'yes'){
        this.couponService.deleteCoupan({promoCodeId:id,updatedBy:user.userId}).subscribe((response: any) => {
          if (response.result) {
            this.couponService.fetch();
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
    });
  }

  /**
   * change coupon
  */
  changecoupon(id: number, status: any, oldstatus) {
    if (status != oldstatus) {
      let modelref = this.modalService.open(ConfirmationpopupComponent)
      modelref.componentInstance.title = this.changecouponStatusName
      modelref.result.then(
        (result) => {
          if (result == 'yes') {
            this.couponService.changeCoupanStatus({promoCodeId:id,Status:status}).subscribe((response: any) => {
              if (response.result) {
                this.couponService.fetch();
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
        },
        (reason) => { }
      );
    }
  }

  /**
   * change status
  */
  changestatus(status, coupon) {
    this.couponList.forEach((coupon) => {
      if (coupon.id === coupon.id) {
        coupon.status = status;
      }
    });
  }
  //#endregion
}
