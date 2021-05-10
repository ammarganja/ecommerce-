import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { EditContentManagementComponent } from './edit-content-management/edit-content-management.component';
import { ViewContentManagementComponent } from './view-content-management/view-content-management.component';
import { contentData } from "./content.data";
import { FormGroup, FormBuilder } from '@angular/forms';
import { Subscription } from 'rxjs';
import { PaginatorState, SortState, GroupingState } from 'src/app/shared/crud-table';
import { contentService } from '../services/content.services';
import { HelperService } from '../services/helper.service';

@Component({
  selector: 'app-content-management',
  templateUrl: './content-management.component.html',
  styleUrls: ['./content-management.component.scss']
})

export class ContentManagementComponent implements
  OnInit,
  OnDestroy{

  //#region Variable Declaration
  paginator: PaginatorState;
  sorting: SortState;
  grouping: GroupingState;
  isLoading: boolean;
  filterGroup: FormGroup;
  searchGroup: FormGroup;
  private subscriptions: Subscription[] = []; 
  Columns:any[];
  rows = [];
  searchString = "";
  //#endregion
  
  //#region Constcructor
  constructor(
    private modalService: NgbModal,
    public contentService: contentService,
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

    this.rows = contentData;
    this.contentService.paginator.page = 1;
    this.contentService.paginator.pageSize = 10;
    this.contentService.paginator.pageSizes = [];
    this.grouping = this.contentService.grouping;
    this.paginator = this.contentService.paginator;
    this.sorting = this.contentService.sorting;
    const sb = this.contentService.isLoading$.subscribe(res => this.isLoading = res);
    this.subscriptions.push(sb);
    this.Columns = this.helperService.getContentColumns();

    this.contentService.patchState({ filter })
  }
  
  /**
   * NgOnDestroy
  */
  ngOnDestroy() {
    this.subscriptions.forEach((sb) => sb.unsubscribe());
  }  

  /**
   * search data
  */
  search(searchTerm: string) {
    this.contentService.paginator.page = 1;
    var filter = {
      "searchString": searchTerm
    }
    this.contentService.patchState({ filter });
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
    this.contentService.patchState({ sorting });
  }

  /**
   * paginate
  */
  paginate(paginator: PaginatorState) {
    this.contentService.patchState({ paginator });
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
  edit(content: any) {
    const modalRef = this.modalService.open(EditContentManagementComponent, { size: 'xl' });
    modalRef.componentInstance.selectData = content;
    modalRef.result.then((res) => {
      this.contentService.fetch();
    });
  }

  /**
   * view discription
  */
  viewDescription(description: string) {
    var viewModal = this.modalService.open(ViewContentManagementComponent, { backdrop: false, size: 'lg' });
    viewModal.componentInstance.description = description;
  }
  //#endregion
}
