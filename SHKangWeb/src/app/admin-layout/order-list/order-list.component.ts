import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AddOrderComponent } from './add-order/add-order.component';
import { PaymentStatusComponent } from './payment-status/payment-status.component';

import { FormBuilder, FormGroup } from '@angular/forms';
import { from, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import {
  GroupingState,
  PaginatorState,
  SortState,
  ICreateAction,
  IEditAction,
  ISortView,
  IFilterView,
  IGroupingView,
  ISearchView,
} from '../../shared/crud-table';

import { DeleteOrderComponent } from './delete-order/delete-order.component';
import { OrderSizeRatioComponent } from '../order-size-ratio/order-size-ratio.component';
import { CreateInvoiceComponent } from '../create-invoice/create-invoice.component';
import { orderData } from './orders.data';
import { Columns } from '../_models/columns';
import { ConfirmationpopupComponent } from 'src/app/core/components/confirmationpopup/confirmationpopup.component';
import { HelperService } from '../services/helper.service';
import { orderService } from '../services/order.services';
import Swal from 'sweetalert2/dist/sweetalert2.js';

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.scss'],
})
export class OrderListComponent
  implements
  OnInit,
  OnDestroy,
  ICreateAction,
  IEditAction,
  ISortView,
  IFilterView,
  IGroupingView,
  ISearchView,
  IFilterView {
  //#region Variable Declaration
  private subscriptions: Subscription[] = [];
  paginator: PaginatorState;
  sorting: SortState;
  grouping: GroupingState;
  isLoading: boolean;
  filterGroup: FormGroup;
  searchGroup: FormGroup;
  searchString = '';
  @ViewChild('ngbDropdown') d: NgbDropdownModule;
  colorModel: any[];
  color: any[];
  ordersList: any[];
  Columns: Columns[];
  OrderStatusName = 'Order';

  singleSelectsettings: {
    singleSelection: boolean;
    enableSearchFilter: boolean;
  };
  resChangeSTatus: any;
  deletestatus: any;
  invoiceData: any;

  //#endregion

  //#region Constcructor
  constructor(
    private fb: FormBuilder,
    private modalService: NgbModal,
    public orderService: orderService,
    public helperservice: HelperService
  ) { }
  //#endregion

  //#region Methods

  /**
   * NgOnInit
   */
  ngOnInit(): void {
    this.filterForm();
    this.searchForm();
    // this.ordersList = orderData;
    this.color = [
      { id: 1, itemName: 'Red' },
      { id: 2, itemName: 'Green' },
      { id: 3, itemName: 'Blue' },
      { id: 4, itemName: 'yellow' },
      { id: 5, itemName: 'Oraned' },
      { id: 6, itemName: 'Black' },
    ];
    this.singleSelectsettings = {
      singleSelection: true,
      enableSearchFilter: true,
    };
    this.colorModel = [];

    var filter = {
      searchString: '',
    };

    this.orderService.paginator.page = 1;
    this.orderService.paginator.pageSize = 10;
    this.orderService.paginator.pageSizes = [];
    this.grouping = this.orderService.grouping;
    this.paginator = this.orderService.paginator;
    this.sorting = this.orderService.sorting;
    const sb = this.orderService.isLoading$.subscribe(
      (res) => (this.isLoading = res)
    );
    this.subscriptions.push(sb);
    this.Columns = this.helperservice.getOrderColumns();

    this.orderService.patchState({ filter });
  }
  /**
   * NgOnDestroy
   */

  ngOnDestroy() {
    this.subscriptions.forEach((sb) => sb.unsubscribe());
  }

  /**
   *FilterForm
   */
  filterForm() {
    this.filterGroup = this.fb.group({
      status: [''],
      type: [''],
      searchTerm: [''],
    });
    this.subscriptions.push(
      this.filterGroup.controls.status.valueChanges.subscribe(() =>
        this.filter()
      )
    );
    this.subscriptions.push(
      this.filterGroup.controls.type.valueChanges.subscribe(() => this.filter())
    );
  }

  /**
   *Change Status
   @param status
   @param order
   */
  changestatus(status, order) {
    // order.orderstatus = status;
    this.ordersList.forEach((Order) => {
      if (order.id === Order.id) {
        Order.paymentStatus = status;
      }
    });
  }

  /**
   *Filter
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
    this.orderService.patchState({ filter });
  }

  /**
   *SearchForm
   */
  searchForm() {
    this.searchGroup = this.fb.group({
      searchTerm: [''],
    });
    const searchEvent = this.searchGroup.controls.searchTerm.valueChanges
      .pipe(
        /*
      The user can type quite quickly in the input box, and that could trigger a lot of server requests. With this operator,
      we are limiting the amount of server requests emitted to a maximum of one every 150ms
      */
        debounceTime(150),
        distinctUntilChanged()
      )
      .subscribe((val) => this.search(val));
    this.subscriptions.push(searchEvent);
  }

  /**
   *Search
   @param searchTerm
   */
  search(searchTerm: string) {
    this.orderService.paginator.page = 1;
    var filter = {
      searchString: searchTerm,
    };
    this.orderService.patchState({ filter });
  }

  /**
   *Sorting column
   @param column
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
    this.orderService.patchState({ sorting });
  }

  /**
   *Pagination
   @param paginator
   */
  paginate(paginator: PaginatorState) {
    this.orderService.patchState({ paginator });
  }

  /**
   *FormAction Create
   */
  create() {
    this.edit(undefined);
  }

  /**
   *FormAction Edit
   */
  edit(order: any) {
    const modalRef = this.modalService.open(AddOrderComponent, { size: 'xl' });
    modalRef.componentInstance.order = order;
    modalRef.result.then(
      () => this.orderService.fetch(),
      () => { }
    );
  }

  /**
   *FormAction Change Ratio
   @param Order
   */
  changeRatio(order: any) {
    const modalRef = this.modalService.open(OrderSizeRatioComponent, {
      size: 'sm',
    });
    modalRef.componentInstance.order = order;
    modalRef.result.then(
      () => this.orderService.fetch(),
      () => { }
    );
  }

  /**
   *FormAction Delete
   @param id
   */
  delete(id: number) {
    const modalRef = this.modalService.open(DeleteOrderComponent);
    modalRef.componentInstance.id = id;
    modalRef.result.then(
      (result) => {
        if (result == 'yes') {
          var data = {
            OrderId: id,
          };
          this.orderService.deleteOrder(data).subscribe((res) => {
            this.deletestatus = res;
            if (this.deletestatus.result == true) {

              Swal.fire({
                title: 'Success',
                text: this.deletestatus.message,
                icon: 'success',
                showCancelButton: false,
                confirmButtonText: 'Ok',
              }).then((result) => {
                this.ngOnInit();
              });
            }
          });
        }
      },
      (reason) => { }
    );
  }
  /**
   *Chnage Order
   @param id
   */
  changeOrder(id: number, status: any, oldstatus) {
    if (status != oldstatus) {
      let modelref = this.modalService.open(ConfirmationpopupComponent);
      // modelref.componentInstance.title = this.OrderStatusName;
      var jsonparserdata = JSON.parse(localStorage.getItem('userdata'));
      modelref.result.then(
        (result) => {
          if (result == 'yes') {
            var data = {
              orderInvoiceId: id,
              orderStatusId: status,
              updatedBy: jsonparserdata.userId,
            };
            this.orderService.orderstatusChange(data).subscribe((res) => {
              this.resChangeSTatus = res;
              if (this.resChangeSTatus.result == true) {
                Swal.fire({
                  title: 'Success',
                  text: this.resChangeSTatus.message,
                  icon: 'success',
                  showCancelButton: false,
                  confirmButtonText: 'Ok',
                }).then((result) => {
                  this.ngOnInit();
                });
              }
            });
          }
        },
        (reason) => { }
      );
    }
  }

  /**
   *Create Invice
   @param order
   */
  createInvoice(order: any) {
    const modalRef = this.modalService.open(CreateInvoiceComponent, {
      size: 'xl',
    });

    modalRef.componentInstance.order = order;
    modalRef.result.then(
      () => this.orderService.fetch(),
      () => { }
    );
  }

  /**
   *Open Invoice Payment
   */
  openInvoice(id) {
    let modalRef = this.modalService.open(PaymentStatusComponent, {
      size: 'lg',
    });
    modalRef.componentInstance.invoiceDataId = id
    modalRef.result.then(
      () => this.orderService.fetch(),
      () => { }
    );
  }
  //#endregion
}
