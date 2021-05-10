import { Component, OnDestroy, OnInit } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { FormGroup, FormBuilder } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { from, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import {
  IFetchSelectedAction,
  IFilterView,
  IGroupingView,
  PaginatorState,
  SortState,
  GroupingState,
  ICreateAction,
  IEditAction,
  IDeleteAction,
  IDeleteSelectedAction,
  IUpdateStatusForSelectedAction,
  ISortView,
  ISearchView,
} from 'src/app/shared/crud-table';
import { TypesService } from '../../services/type.service';
import { DeleteTypeComponent } from './delete-type/delete-type.component';
import { EditTypeComponent } from './edit-type/edit-type.component';

@Component({
  selector: 'app-type',
  templateUrl: './type.component.html',
  styleUrls: ['./type.component.scss'],
})
export class TypeComponent
  implements
    OnInit,
    OnDestroy,
    ICreateAction,
    IEditAction,
    IDeleteAction,
    IDeleteSelectedAction,
    IFetchSelectedAction,
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
  private subscriptions: Subscription[] = [];

  constructor(
    private fb: FormBuilder,
    private modalService: NgbModal,
    public typeService: TypesService
  ) {}

  typeList = [];
  // angular lifecircle hooks
  ngOnInit(): void {
    this.filterForm();
    //// this.typeService.fetch();
    this.typeList = [
      { id: 1, name: 'type1' },
      { id: 2, name: 'type2' },
      { id: 3, name: 'type3' },
      { id: 4, name: 'type4' },
      { id: 5, name: 'type5' },
      { id: 6, name: 'type6' },
    ];
    this.typeService.paginator.page = 1;
    this.typeService.paginator.pageSize = 10;
    this.typeService.paginator.pageSizes = [];
    this.grouping = this.typeService.grouping;
    this.paginator = this.typeService.paginator;
    this.sorting = this.typeService.sorting;
    const sb = this.typeService.isLoading$.subscribe(
      (res) => (this.isLoading = res)
    );
    this.subscriptions.push(sb);
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sb) => sb.unsubscribe());
  }

  // filtration
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
    this.typeService.patchState({ filter });
  }

  // search
  searchForm() {
    this.searchGroup = this.fb.group({
      searchTerm: [''],
    });
    const searchEvent = this.searchGroup.controls.searchTerm.valueChanges
      .pipe(debounceTime(150), distinctUntilChanged())
      .subscribe((val) => this.search(val));
    this.subscriptions.push(searchEvent);
  }

  search(searchTerm: string) {
    this.typeService.patchState({ searchTerm });
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
    this.typeService.patchState({ sorting });
  }

  // pagination
  paginate(paginator: PaginatorState) {
    this.typeService.patchState({ paginator });
  }

  // form actions
  create() {
    this.edit(undefined);
  }

  edit(customer: any) {
    const modalRef = this.modalService.open(EditTypeComponent, { size: 'sm' });
    modalRef.componentInstance.customer = customer;
    modalRef.result.then(
      () => this.typeService.fetch(),
      () => {}
    );
  }

  delete(id: number) {
    const modalRef = this.modalService.open(DeleteTypeComponent);
    modalRef.componentInstance.id = id;
    modalRef.result.then(
      () => this.typeService.fetch(),
      () => {}
    );
  }
}
