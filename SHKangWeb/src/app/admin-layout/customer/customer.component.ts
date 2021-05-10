// tslint:disable:no-string-literal
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DeleteconfirmationComponent } from 'src/app/core/components/deleteconfirmation/deleteconfirmation.component';
import {
  GroupingState,
  PaginatorState,
  SortState,
  ICreateAction,
  IEditAction,
  IDeleteAction,
  ISortView,
  IFilterView,
  IGroupingView,
  ISearchView,
} from '../../shared/crud-table';
import { EditCustomerComponent } from './edit-customer/edit-customer.component';
import { CustomersService } from '../services/customers.service';
import { AddressManagementComponent } from './address-management/address-management.component';
import { customerData } from './customer.data';
import { ConfirmationpopupComponent } from 'src/app/core/components/confirmationpopup/confirmationpopup.component';
import { HelperService } from '../services/helper.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';
@Component({
  selector: 'app-customers',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.scss'],
})
export class CustomerComponent
  implements
    OnInit,
    OnDestroy,
    ICreateAction,
    IEditAction,
    IDeleteAction,
    ISortView,
    IFilterView,
    IGroupingView,
    ISearchView,
    IFilterView {
  //#region Variable Declaration
  paginator: PaginatorState;
  sorting: SortState;
  grouping: GroupingState;
  isLoading: boolean;
  filterGroup: FormGroup;
  searchGroup: FormGroup;
  private subscriptions: Subscription[] = [];
  Columns: any[];
  singleSelectsettings: {};
  customerStatus: any[];
  customerModel: any[];
  userStatusName = 'User';
  userList = [];
  isLogin = false;
  searchString: any;
  isLoginUserId: any;
  constructor(
    private fb: FormBuilder,
    private modalService: NgbModal,
    public customerService: CustomersService,
    public helperService: HelperService
  ) {}

  //#endregion

  //#region Methods

  /**
   * NgOnInit
   */
  ngOnInit(): void {
   this.customerStatus = [
      { id: 1, itemName: 'Active' },
      { id: 2, itemName: 'Pending' },
      { id: 3, itemName: 'Suspended' },
    ];
    this.singleSelectsettings = {
      singleSelection: true,
      enableSearchFilter: true,
      classes : 'hide_x_button'
    };
    
    var filter = {
      "searchString": "",
      "Status": 0
    }

    this.customerService.paginator.page = 1;
    this.customerService.paginator.pageSize = 10;
    this.customerService.paginator.pageSizes = [];
    this.paginator = this.customerService.paginator;
    this.sorting = this.customerService.sorting;
    const sb = this.customerService.isLoading$.subscribe(
      (res) => (this.isLoading = res)
    );
    this.subscriptions.push(sb);

    this.Columns = this.helperService.getCustomerColumns();
    this.customerService.patchState({ filter })
  }
  /**
   * NgOnDestroy
   */
  ngOnDestroy() {
    this.subscriptions.forEach((sb) => sb.unsubscribe());
  }
  

  /**
   * FilterForm
   */
  filterForm() {}
  /**
   * filter data
   */
  filter() {}

  /**
   * search form
   */
  searchForm() {}
  /**
   * Search
   * @param searchTerm
   */
  search(searchTerm: string) {
    this.customerService.paginator.page = 1;
    var filter = {
      "searchString": searchTerm,
      "Status": this.customerModel && this.customerModel.length > 0 ? this.customerModel[0].id : 0
    }
    this.customerService.patchState({ filter });
  }

  onItemSelect($event){
    this.customerService.paginator.page = 1;
    var filter = {
      "searchString": this.searchString,
      "Status": this.customerModel && this.customerModel.length > 0 ? this.customerModel[0].id : 0
    }
    this.customerService.patchState({ filter });
  }

  OnItemDeSelect($event){
    this.customerService.paginator.page = 1;
    var filter = {
      "searchString": this.searchString,
      "Status": this.customerModel && this.customerModel.length > 0 ? this.customerModel[0].id : 0
    }
    this.customerService.patchState({ filter });
  }

  /**
   * Sort
   * @param column
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
    this.customerService.patchState({ sorting });
  }

  /**
   * Paginate
   * @param paginator
   */
  paginate(paginator: PaginatorState) {
    this.customerService.patchState({ paginator });
  }

  /**
   * Create Form Action
   */
  create() {
    this.edit(undefined);
  }
  address(customer: any) {
    let modelref = this.modalService.open(AddressManagementComponent, {
      size: 'xl',
    });
    modelref.componentInstance.customer = customer;
  }
  /**
   * Edit the form
   * @param sizeFormObj
   */

  edit(customer: any) {
    const modalRef = this.modalService.open(EditCustomerComponent, {
      size: 'xl',
    });
    modalRef.componentInstance.customer = customer;
    modalRef.result.then(
      () => this.customerService.fetch(),
      () => {}
    );
  }
  /**
   * Delete
   * @param id
   */

  delete(id: number) {
    if(id == this.isLoginUserId){
      Swal.fire({
        title: 'Error',
        text: "You can not delete logged in user.",
        icon: 'error',
        showCancelButton: false,
        confirmButtonText: 'Ok',
      });
    }else{
      const modalRef = this.modalService.open(DeleteconfirmationComponent);
      let data = {
        id: id,
        key: 'Customer',
      };
      modalRef.componentInstance.data = data;
      modalRef.result.then((res) => {
        if (res === 'yes') {
          this.customerService.deleteCustomer(id).subscribe((response: any) => {
            if (response.result) {
              this.customerService.fetch();
            } else {
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
    
  }
  /**
   * Status Change
   * @param id
   * @param oldstatus
   */

  customerstatus(id: number, status: any, oldstatus) {
    if (status != oldstatus) {
      let modelref = this.modalService.open(ConfirmationpopupComponent);
      modelref.componentInstance.title = modelref.result.then(
        (result) => {
          if (result == 'yes') {
            this.customerService.changeCustomerStatus(id,status).subscribe((response: any) => {
              if (response.result) {
                this.customerService.fetch();
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
        (reason) => {}
      );
    }
  }

  isLoginUser() {
    var user = JSON.parse(localStorage.getItem('userdata'));
    if (user.userId) {
      this.isLoginUserId = user.userId;
      if (this.isLoginUserId === user.userId) {
  
      }
    }
  }
  //#endregion
}
