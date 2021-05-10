import { Component, Input, NO_ERRORS_SCHEMA, OnDestroy, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ICreateAction, IEditAction, IDeleteAction, IDeleteSelectedAction, IFetchSelectedAction, IUpdateStatusForSelectedAction, ISortView, IFilterView, IGroupingView, ISearchView, PaginatorState, SortState, GroupingState } from 'src/app/shared/crud-table';
import { CustomersService } from '../../services/customers.service';
import { AddressService } from '../../services/address.services';
import { DeleteAddressComponent } from './delete-address/delete-address.component';
import { EditAddressComponent } from './edit-address/edit-address.component';
import { HelperService } from '../../services/helper.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { DeleteconfirmationComponent } from 'src/app/core/components/deleteconfirmation/deleteconfirmation.component';

@Component({
  selector: 'app-address-management',
  templateUrl: './address-management.component.html',
  styleUrls: ['./address-management.component.scss'],
})
export class AddressManagementComponent implements
OnInit,
OnDestroy,
ICreateAction,
IEditAction,
IDeleteAction,
IDeleteSelectedAction,
IFetchSelectedAction,
IUpdateStatusForSelectedAction,
ISortView,
IFilterView,
IGroupingView,
ISearchView,
IFilterView {
paginator: PaginatorState;
sorting: SortState;
grouping: GroupingState;
isLoading: boolean;
filterGroup: FormGroup;
searchGroup: FormGroup;
Columns: any[];
private subscriptions: Subscription[] = [];
  @Input() customer;
  addressformObj=[];
  addresses = [];
   constructor(private modal: NgbActiveModal,
    private modelservice:NgbModal,
    public addressService: AddressService, 
    private helperService : HelperService) {}
   
  ngOnInit(): void {
    var filter = {
      "searchString": "",
      "userId" : this.customer.userId
    }
    this.addressService.patchState({ filter })
    console.log(this.customer)
    // if (this.address) {
    //   this.addresses = this.address;
    // }
    this.addressService.paginator.page = 1;
    this.addressService.paginator.pageSize = 10;
    this.addressService.paginator.pageSizes = [];
    this.paginator = this.addressService.paginator;
    this.sorting = this.addressService.sorting;
    const sb = this.addressService.isLoading$.subscribe(res => this.isLoading = res);
    this.subscriptions.push(sb);

    this.Columns = this.helperService.getAddressColumns();

  }
  cancel() {
    this.modal.close(AddressManagementComponent);
  }
  ngOnDestroy() {
    this.subscriptions.forEach((sb) => sb.unsubscribe());
  }
  
  // filtration
  filterForm() {
  }
  
  filter() {
  }
  
  // search
  searchForm() {
  }
  
  search(searchTerm: string) {
    this.addressService.patchState({ searchTerm });
  }
  
  // sorting
  sort(column: string) {
    const sorting = this.sorting;
    const isActiveColumn = sorting.column === column;
    if (!isActiveColumn) {
      sorting.column = column;
      sorting.direction = 'asc';
    } else {
      sorting.direction = sorting.direction === 'asc' ? 'desc' : 'asc';
    }
    this.addressService.patchState({ sorting });
  }
  
  // pagination
  paginate(paginator: PaginatorState) {
    this.addressService.patchState({ paginator });
  }
  
  // form actions
  create() {
    this.edit(undefined);
  }
  
  edit(address: any) {
    if(address){
      address.userId = this.customer.userId;
    }
    
    let  modalref = this.modelservice.open(EditAddressComponent, { size: 'xl' });
    modalref.componentInstance.address = address;
    modalref.componentInstance.selectedUser = this.customer.userId;
    modalref.result.then(
      () => this.addressService.fetch(),
      () => {}
    );
  }

  delete(id: number) {
    const modalRef = this.modelservice.open(DeleteconfirmationComponent);
    let data = {
      id: id,
      key: 'Address',
    };
    modalRef.componentInstance.data = data;
    modalRef.result.then((res) => {
      if(res === 'yes'){
        this.addressService.deleteUserAddress(id).subscribe((response: any) => {
          if (response.result) {
            this.addressService.fetch();
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
  
  deleteSelected() {
  }
  
  updateStatusForSelected() {
  }
  
  fetchSelected() {
  }
}
