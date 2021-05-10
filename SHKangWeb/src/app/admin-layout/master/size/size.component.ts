import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { ICreateAction, IDeleteAction, IEditAction, IFilterView, ISearchView, ISortView, PaginatorState, SortState } from 'src/app/shared/crud-table';
import { HelperService } from '../../services/helper.service';
import { SizeService } from '../../services/size.service';
import { Column } from '../../_models/columns.model';
import { EditSizeComponent } from './edit-size/edit-size.component';
import { sizeData } from "./size.data";
import { DeleteconfirmationComponent } from 'src/app/core/components/deleteconfirmation/deleteconfirmation.component';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { Router } from '@angular/router';
@Component({
  selector: 'app-size',
  templateUrl: './size.component.html',
  styleUrls: ['./size.component.scss'],
})
export class SizeComponent implements
  OnInit,
  OnDestroy,
  ICreateAction,
  IEditAction,
  IDeleteAction,
  ISortView,
  IFilterView,
  ISearchView,
  IFilterView {

  //#region Variable Declaration
  paginator: PaginatorState;
  sorting: SortState;
  isLoading: boolean;
  filterGroup: FormGroup;
  searchGroup: FormGroup;
  private subscriptions: Subscription[] = []; // Read more: => https://brianflove.com/2016/12/11/anguar-2-unsubscribe-observables/
  sizeData = [];
  Columns: Column[];
  //#endregion
  searchString = "";

  //#region Constcructor
  constructor(
    private fb: FormBuilder,
    private modalService: NgbModal,
    public sizeService: SizeService,
    private helperService: HelperService,
    private router: Router
  ) { }
  //#endregion

  //#region Methods

  /**
   * NgOnInit
   */
  ngOnInit(): void {
    ////this.filterForm();
    //// this.sizeService.fetch();
    var filter = {
      "searchString": ""
    }

    this.sizeService.paginator.page = 1;
    this.sizeService.paginator.pageSize = 10;
    this.sizeService.paginator.pageSizes = [];
    this.paginator = this.sizeService.paginator;
    this.sorting = this.sizeService.sorting;
    const sb = this.sizeService.isLoading$.subscribe(res => this.isLoading = res);
    this.subscriptions.push(sb);
    this.Columns = this.helperService.getSizeColumns();

    this.sizeService.patchState({ filter })
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
  filterForm() {
  }

  /**
   * filter data
  */
  filter() {
    const filter = {};
    this.sizeService.patchState({ filter });
  }

  /**
   * search form
   */
  searchForm() {
    this.searchGroup = this.fb.group({
      searchTerm: [''],
    });
    const searchEvent = this.searchGroup.controls.searchTerm.valueChanges
      .pipe(
        debounceTime(150),
        distinctUntilChanged()
      )
      .subscribe((val) => this.search(val));
    this.subscriptions.push(searchEvent);
  }

  /**
   * Search
   * @param searchTerm 
   */
  search(searchTerm: string) {
    this.sizeService.paginator.page = 1;
    var filter = {
      "searchString": searchTerm
    }
    this.sizeService.patchState({ filter });
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
    this.sizeService.patchState({ sorting });
  }

  /**
   * Paginate
   * @param paginator 
   */
  paginate(paginator: PaginatorState) {
    this.sizeService.patchState({ paginator });
  }

  /**
   * Create Form Action
   */
  create() {
    this.edit(undefined);
  }

  /**
   * Edit the form
   * @param sizeFormObj 
   */
  // edit(sizeFormObj: any) {
  //   const modalRef = this.modalService.open(EditSizeComponent, { size: 'lg' });
  //   if(sizeFormObj != undefined){
  //     modalRef.componentInstance.sizeFormData = sizeFormObj;
  //     modalRef.result.then(() =>
  //       this.sizeService.fetch(),
  //       () => { }
  //     );
  //   }
  // }

  edit(size: any) {
    if (size) {
      var selectedSize = {
        sizeId: size.sizeId,
        sizeName: size.sizeName,
        units: size.units
      }
    }

    const modalRef = this.modalService.open(EditSizeComponent, { size: 'lg' });
    modalRef.componentInstance.sizeFormData = selectedSize;
    modalRef.result.then((res) => {
      this.sizeService.fetch();
    });
  }

  /**
   * Delete
   * @param id 
   */

  delete(id: number) {
    const modalRef = this.modalService.open(DeleteconfirmationComponent);
    let data = {
      id: id,
      key: 'size',
    }
    modalRef.componentInstance.data = data;
    modalRef.result.then((res) => {
      if (res === 'yes') {
        this.sizeService.deletesize(id).subscribe((response: any) => {
          if (response.result) {
            this.sizeService.fetch();
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
  redirectProduct(size: any) {
    this.router.navigate(['/products'], { queryParams: { idSize: size.sizeId } })
  }

  //#endregion
}