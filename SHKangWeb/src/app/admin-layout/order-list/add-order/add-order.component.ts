import { AddProductComponent } from '../../add-product/add-product.component';
import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import Swal from 'sweetalert2/dist/sweetalert2.js';

import {
  GroupingState,
  ICreateAction,
  IDeleteAction,
  IEditAction,
  ISearchView,
  PaginatorState,
  SortState,
} from 'src/app/shared/crud-table';
import { FormGroup, NgForm } from '@angular/forms';
import { data, dresses, rompres } from '../orders.data';
import { orderService } from '../../services/order.services';
import { Address } from '../../_models/address.model';
import { AddressManagementComponent } from '../../customer/address-management/address-management.component';
import { EditAddressComponent } from '../../customer/address-management/edit-address/edit-address.component';
import { CustomersService } from '../../services/customers.service';

@Component({
  selector: 'app-add-order',
  templateUrl: './add-order.component.html',
  styleUrls: ['./add-order.component.scss'],
})
export class AddOrderComponent implements OnInit, ICreateAction, IEditAction {
  //#region Variable Declaration
  @Input() order: any;
  userselected: any;
  customerselected = [];
  singleSelectsettings;
  style = [];
  StyleSelected = [];
  uservals = [];
  selectedUserEmail: string;
  selectedUserName: string;
  selectedUserAddress: string;
  myValue = false;
  isSelecteds = [];
  paginator: PaginatorState;
  sorting: SortState;
  grouping: GroupingState;
  isLoading: boolean;
  private subscriptions: Subscription[] = [];
  rompers = rompres;
  dresses = dresses;
  data = data;
  categories: string;
  productList: any;
  total: number;
  finaltotal = 0;
  searchGroup: FormGroup;
  resultsORderProductLIst: any = {};
  resultUser: any = {};
  addressesResult: any;
  addressList: any;
  AddressSelected;
  user = JSON.parse(localStorage.getItem('userdata'));
  UserId = this.user.userId;
  selectedUserID: any;
  selectedUserAddressID: any;
  AddressSelectedList;
  searchString: string;
  //#endregion

  orderformobj = {
    OrderId: '0',
    UserId: '',
    CreatedBy: '',
    UserAddressId: '',
    IsMailSend: '1',
    ProductDetail: [],
  };
  productFinal: any = [];
  addorderRes: any;
  orderId: any;
  editRes: any = {};
  customerDetails: any;
  //#region Constcructor
  constructor(
    public modal: NgbActiveModal,
    private modalService: NgbModal,
    public orderService: orderService,
    public customerService:CustomersService
  ) {}

  //#endregion

  //#region Methods

  /**
   * NgOnInit
   */
  ngOnInit(): void {
    this.getOrderProductList('');
    this.getOrderCustomersList();

    if (this.order) {
      this.editOrder();
    }

    this.singleSelectsettings = {
      singleSelection: true,
      enableSearchFilter: true,
    };
    this.categories = 'all';
    this.orderService.paginator.page = 1;
    this.orderService.paginator.pageSize = 10;
    this.orderService.paginator.pageSizes = [];
    this.grouping = this.orderService.grouping;
    this.paginator = this.orderService.paginator;
    this.sorting = this.orderService.sorting;
    const sb = this.orderService.isLoading$.subscribe(
      (res) => (this.isLoading = res)
    );
    this.categories = 'all';
    // this.productList = rompres.concat(dresses);
    // if (this.order && this.order.idproducts) {
    //   var isSelecteds = this.order.idproducts.split(',');
    //   if (isSelecteds && isSelecteds.length > 0) {
    //     isSelecteds.forEach((isSelected) => {
    //       this.productList.forEach((product) => {
    //         if (product.id === +isSelected) {
    //           product.isSelected = true;
    //         }
    //       });
    //     });
    //   }
    // }
  }

  /**
   * cancel model
   * @param event
   */
  cancel(event) {}

  /**
   * user Select
   * @param event
   */
  OnAddrssSelect(event) {
    this.selectedUserAddress = event.itemName;
    this.selectedUserAddressID = event.id;
  }
  /**
   * Order product List
   * @param search
   */

  getOrderProductList(search) {
    this.orderService.addorderProductList(search).subscribe((res) => {
      this.resultsORderProductLIst = res;
      if (this.resultsORderProductLIst) {
        this.productList = this.resultsORderProductLIst.items;
        this.total = this.resultsORderProductLIst.total;
      }
    });
  }

  /**
   * Order customer List
   * @param search
   */
  getOrderCustomersList() {
    this.orderService.getcustomersListing().subscribe((res) => {
      this.resultUser = res;
      if (this.resultUser.result) {
        this.uservals = this.resultUser.data;
      }
    });
  }

  /**
   * Edit Order
   *
   */
  editOrder() {
    let editOrder = {
      OrderId: this.order.orderId,
      pageNo: 1,
      limit: 10,
      searchString: '',
      column: 'productCategoryTypeId',
      direction: 'desc',
    };

    this.orderService.editOrderGEtData(editOrder).subscribe((res) => {
      this.editRes = res;
      if (this.editRes.result) {
        var user = {
          id: this.editRes.data.customerId,
        };
        this.onUserSelect(user);
        var address = {
          id: this.editRes.data.customerAddressId,
        };
        this.OnAddrssSelect(address);
        this.productList = this.editRes.data.productList.items;
        this.selectedUserAddressID = this.editRes.data.customerAddressId;
        this.selectedUserID = this.editRes.data.customerId;
      }
    });
  }

/**
   * Add User Select
   * @param Userid
   */
 addadress(userselectedData:number){
   let data = {
    userId: this.selectedUserID.id, 
  }
  let modelref = this.modalService.open(AddressManagementComponent,{size:'xl'});
  modelref.componentInstance.customer = data;
  modelref.result.then(
    () => {
      var user = {
        id: this.selectedUserID.id,
      };
      this.onUserSelect(user);
    }
  );
  }
  /**
   * user Select
   * @param id
   */
  onUserSelect(id: any) {
    this.userselected;
    // this.AddressSelected = null;
    let result = this.uservals.find((res) => res.id === id.id);
    let data = {
      userId: result.id,
    };
    this.selectedUserName = result.itemName;
    this.selectedUserID = id;
    this.userselected = [
      { id: this.selectedUserID, itemName: this.selectedUserName },
    ];

    this.orderService.getAddressFromUserId(data).subscribe((Res) => {
      this.addressesResult = Res;
      if (this.addressesResult.result) {
        this.selectedUserAddress = this.AddressSelected;
        this.selectedUserEmail = this.addressesResult.data.emailNumber;
        this.addressList = this.addressesResult.data.addressList;
      }
      let addresses = this.addressList.filter(
        (data) => data.id == this.selectedUserAddressID
      );
      this.AddressSelected = addresses;
      this.selectedUserAddress = addresses.itemName;
    });
  }
  /**
   * searchTerm
   * @param searchTerm
   *
   */
  search(searchTerm: string) {
    this.orderService.paginator.page = 1;
    // var filter = {
    //   searchString: searchTerm,
    // };
    this.getOrderProductList(searchTerm);
  }

  /**
   * Product Select
   * @param product
   * @param total as (price * qty) for single product
   */
  changeValueEvent(Product, total) {
    this.productFinal.length = 0;
    if (Product.isSelected == true) {
      this.finaltotal = 0;
      this.productList.forEach((product, index) => {
        if (product.isSelected == true) {
          if (product.quantity == null) {
            product.quantity = 1;
          }
          this.finaltotal += product.quantity * product.price;
        }
      });
    } else {
      this.productList.forEach((product, index) => {
        if (
          product.isSelected == false &&
          product.productId == Product.productId
        ) {
          this.finaltotal -= product.price * product.quantity;
        }
      });
    }
    if(this.finaltotal < 0){
      this.finaltotal = 0;
    }

    this.productList.forEach((product, index) => {
      if (product.isSelected == true) {
        let datatopush = {
          ProductId: product.productId,
          Price: product.price,
          Quantity: product.quantity,
          ProductColorId: product.colorId,
        };
        this.productFinal.push(datatopush);
      }
    });
  }

  /**
   * Update Quantity
   * @param Product
   * @param total as (price * qty) for single product
   */
  updateQuantity(Product, total) {
    this.finaltotal = 0;
    this.productFinal.length = 0;
    this.productList.forEach((product) => {
      if (product.isSelected == true) {
        this.finaltotal += product.quantity * product.price;
        let datatopush = {
          ProductId: product.productId,
          Price: product.price,
          Quantity: product.quantity,
          ProductColorId: product.colorId,
        };
        this.productFinal.push(datatopush);
      }
    });
  }

  /**
   * Get List Product
   */

  getAllProduct() {
    // console.log('-------', this.categories);
    // if (this.categories == 'rompers') {
    //   this.productList = rompres;
    // } else if (this.categories == 'dresses') {
    //   this.productList = dresses;
    // } else {
    //   this.productList = rompres.concat(dresses);
    // }
  }

  /**
   * FOrm Action Submit
   */
  submit(orderForm: NgForm) {
    if (orderForm.form.valid) {
      this.orderformobj = {
        OrderId: this.order? this.order.orderId : '0',
        UserId: (typeof this.selectedUserID == "object") ? this.selectedUserID.id:  this.selectedUserID,
        CreatedBy: this.UserId.toString(),
        UserAddressId: this.selectedUserAddressID,
        IsMailSend: '1',
        ProductDetail: this.productFinal?this.productFinal:this.order.ProductDetail,
      };
      
      this.orderService.addNewOrder(this.orderformobj).subscribe((res) => {
        this.addorderRes = res;
        if (this.addorderRes.result) {
          this.modal.dismiss();
          this.orderService.fetch();
          Swal.fire({
            title: 'Success',
            text: this.addorderRes.message,
            icon: 'success',
            showCancelButton: false,
            confirmButtonText: 'Ok',
          });
        }
      });
    }
  }

  /**
   * Sorting product
   * @param column
   */
  sort(column: string) {}

  /**
   * Pagination
   * @param paginator
   */
  paginate(paginator: PaginatorState) {
    this.orderService.patchState({ paginator });
  }

  /**
   * Form action create
   *
   */
  create() {
    this.edit(undefined);
  }

  /**
   * Form action Edit
   *
   * @param Edit
   */

  edit(order: any) {}

   /**
   * Form action Invoice Create
   *
   * @param Edit
   */

    CreateInvoice(order) {
      var data = {
        orderId:1,
      }
      this.orderService.getInvoiceDataToCreate(data);

    }

  /**
   * Form action change sie ratio
   * @param order
   */
  changeRatio(order: any) {}
  //#endregion
}
