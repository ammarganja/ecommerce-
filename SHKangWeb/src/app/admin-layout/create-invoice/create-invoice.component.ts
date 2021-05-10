import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, NgForm } from '@angular/forms';
import { MAT_PROGRESS_SPINNER_DEFAULT_OPTIONS_FACTORY } from '@angular/material/progress-spinner';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import {
  GroupingState,
  PaginatorState,
  SortState,
} from 'src/app/shared/crud-table';
import { CreateinvoiceService } from '../services/createinvoice.service';
import { orderService } from '../services/order.services';
import Swal from 'sweetalert2/dist/sweetalert2.js';

@Component({
  selector: 'app-create-invoice',
  templateUrl: './create-invoice.component.html',
  styleUrls: ['./create-invoice.component.scss'],
})
export class CreateInvoiceComponent implements OnInit {
  @Input() order: any;

  orderDetail;
  productVat: number;
  shippingCharge: number;
  tracking: string;
  ShippingCharge: number;
  listTotal = 0;

  categories: string;
  productList: any;
  selectedProducts = [];
  tabs = {
    Step1: 0,
    Step2: 1,
  };
  numbervalidationError = false;
  numbervalidationErrorVat = false;
  trckvalError = false;
  invoiceres: any;
  finalProducts: any = [];
  sizeratio: any;
  orderFormObj = {
    OrderInvoiceId: null,
    OrderId: null,
    UserId: null,
    CreatedBy: null,
    UpdatedBy: null,
    ShippingCharges: null,
    VatCharges: null,
    TrackingNumber: null,
    ProductDetail: [],
  };
  ratioError: string;
  ratios: any;
  UsrData = JSON.parse(localStorage.getItem('userdata'));
  UserId = this.UsrData.userId;
  addInvoiceRes: any;

  paginator: PaginatorState;
  sorting: SortState;
  grouping: GroupingState;
  isLoading: boolean;
  filterGroup: FormGroup;
  searchGroup: FormGroup;
  private subscriptions: Subscription[] = [];

  activeTabId = this.tabs.Step1;
  finalTotal = 0;
  vatWithTotal: number;
  GRandTotal: any;
  poductSendArray: any;

  constructor(
    public createinvoiceService: CreateinvoiceService,
    public modal: NgbActiveModal,
    private orderservice: orderService
  ) {}

  ngOnInit(): void {
    let orderdata = {
      OrderId: this.order,
    };
    this.orderservice.getInvoiceDataToCreate(orderdata).subscribe((res) => {
      this.invoiceres = res;
      if (this.invoiceres.result) {
        this.orderDetail = this.invoiceres.data;
      }
    });

    this.categories = 'all';
    this.createinvoiceService.paginator.page = 1;
    this.createinvoiceService.paginator.pageSize = 10;
    this.createinvoiceService.paginator.pageSizes = [];
    this.grouping = this.createinvoiceService.grouping;
    this.paginator = this.createinvoiceService.paginator;
    this.sorting = this.createinvoiceService.sorting;
  }

  changeTab(tabId: number) {
    this.activeTabId = tabId;
  }

  changeValueEvent() {
    console.log('*********', this.productList);
  }

  submit() {
    if(this.finalProducts.length == 0){
      Swal.fire({
        title: 'Error',
        text: "Please select Product",
        icon: 'error',
        showCancelButton: false,
        confirmButtonText: 'Ok',
      });
    }
    this.orderFormObj = {
      OrderInvoiceId: '0',
      OrderId: this.orderDetail.orderId,
      UserId: this.orderDetail.userId,
      CreatedBy: this.UserId,
      UpdatedBy: this.UserId,
      ShippingCharges: this.shippingCharge,
      VatCharges: this.productVat,
      TrackingNumber: this.tracking,
      ProductDetail: this.poductSendArray,
    };
    this.orderservice.CreateNewInvoice(this.orderFormObj).subscribe((res) => {
      this.addInvoiceRes = res;
      if (this.addInvoiceRes.result == true) {
        this.modal.dismiss();
        this.orderservice.fetch();
        Swal.fire({
          title: 'Success',
          text: this.addInvoiceRes.message,
          icon: 'success',
          showCancelButton: false,
          confirmButtonText: 'Ok',
        });
      }else{
        Swal.fire({
          title: 'Error',
          text: this.addInvoiceRes.message,
          icon: 'error',
          showCancelButton: false,
          confirmButtonText: 'Ok',
        });
      }
    });
  }
  calculateShipping(val){
    if(isNaN(val) || val==null || val==""){
      val = 0
   }
    this.GRandTotal = this.finalTotal + parseFloat(val);
  }
  calculateVat(val){
    this.finalTotal = 0;
    this.orderDetail.productDetails.forEach((product) => {
      if(isNaN(product.subtotal) || product.subtotal==null || product.subtotal==""){
         product.subtotal = 0
      }
      this.finalTotal += parseFloat(product.subtotal)
    });
    let vatcalc = this.finalTotal*(parseFloat(val)/100)
    this.finalTotal = vatcalc + this.finalTotal;
  }

  changecheckBoxEvent(Product) {
    
    this.ratios = Product.remainingRatio.split('/');
    this.finalProducts.length = 0;
    this.orderDetail.productDetails.forEach((product) => {
      let sizes = [];

      product.sizeGroupList.sizeGroupList.forEach((size, index) => {
        sizes.push(size.value);
        product.remainingSizeLists.forEach((element) => {
          if (element.value < size.value) {
            size.ratioError = 'Size is Not Grater Then remaining Sizes to ship';
          } else {
            size.ratioError = '';
          }
        });
      });
      if (product.isSelected == true) {
        let quantity = 0;
        sizes.forEach((element) => {
          quantity = quantity + element;
        });

        debugger
        let finalqty = (quantity / product.requiredTotalSize);

        product.sizeGroupList.qtySubTotal = (finalqty * product.remainingQty)* product.price;

        
        let data = {
          ProductId: product.productId,
          Price: product.price,
          Quantity: (product.requiredTotalSize == quantity)? product.totalQty : ((finalqty*product.remainingQty)).toFixed(2),
          ProductColorId: product.productColorId,
          SizeRatio: sizes.toString(),
          subTotal: product.sizeGroupList.qtySubTotal,
        };
        this.finalProducts.push(data);
      }
      product.subtotal = product.sizeGroupList.qtySubTotal 
    });
    this.poductSendArray = this.finalProducts;
  }

  trackingRequired(trckval: string) {
    if (trckval == '' || trckval == null || trckval == undefined) {
      this.trckvalError = true;
    } else {
      this.trckvalError = false;
    }
  }
  email(id:any){
    let data = {
      orderInvoiceId:id,
    }
    this.orderservice.sendEmailInvoice(data).subscribe((data:any)=>{
      if(data.result){
        Swal.fire({
          title: 'Success',
          text: "Success fully sent email",
          icon: 'success',
          showCancelButton: false,
          confirmButtonText: 'Ok',
        });      
      }
    })    
  }
}
