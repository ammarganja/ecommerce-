import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { from, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { DeleteconfirmationComponent } from 'src/app/core/components/deleteconfirmation/deleteconfirmation.component';
import { HelperService } from '../../services/helper.service';
import { Column } from '../../_models/columns.model';
import { PaginatorState, SortState, GroupingState } from 'src/app/shared/crud-table';
import { ColorService } from '../../services/color.service';
import { EditColorComponent } from './edit-color/edit-color.component';
import { colorData } from "./color.data";
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { Router, Routes } from '@angular/router';

@Component({
  selector: 'app-color',
  templateUrl: './colors.component.html',
  styleUrls: ['./colors.component.scss'],
})

export class ColorsComponent
  implements
  OnInit,
  OnDestroy {

  //#region Variable Declaration
  paginator: PaginatorState;
  sorting: SortState;
  grouping: GroupingState;
  isLoading: boolean;
  filterGroup: FormGroup;
  searchGroup: FormGroup;
  colorList = [];
  searchString = "";
  private subscriptions: Subscription[] = [];
  Columns: Column[];
  //#endregion

  //#region Constcructor
  constructor(
    private fb: FormBuilder,
    private modalService: NgbModal,
    public ColorService: ColorService,
    public helperService: HelperService,
    private router: Router
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
    
    ///this.ColorService.fetch();
    this.colorList = colorData;
    this.ColorService.paginator.page = 1;
    this.ColorService.paginator.pageSize = 10;
    this.ColorService.paginator.pageSizes = [];
    this.grouping = this.ColorService.grouping;
    this.paginator = this.ColorService.paginator;
    this.sorting = this.ColorService.sorting;
    const sb = this.ColorService.isLoading$.subscribe(
      (res) => (this.isLoading = res)
    );
    this.subscriptions.push(sb);

    this.Columns = this.helperService.getColorColumns();
    this.ColorService.patchState({ filter })
  }

  /**
   * NgOnDestroy
  */
  ngOnDestroy() {
    this.subscriptions.forEach((sb) => sb.unsubscribe());
  }

  /**
    * filter data
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
    this.ColorService.patchState({ filter });
  }

  /**
    * search form
  */
  searchForm() {
    this.searchGroup = this.fb.group({
      searchTerm: [''],
    });
    const searchEvent = this.searchGroup.controls.searchTerm.valueChanges
      .pipe(debounceTime(150), distinctUntilChanged())
      .subscribe((val) => this.search(val));
    this.subscriptions.push(searchEvent);
  }

  /**
    * search
  */
  search(searchTerm: string) {
    this.ColorService.paginator.page = 1;
    var filter = {
      "searchString": searchTerm
    }
    this.ColorService.patchState({ filter });
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
    this.ColorService.patchState({ sorting });
  }

  /**
    * paginate
  */
  paginate(paginator: PaginatorState) {
    this.ColorService.patchState({ paginator });
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
  edit(color: any) {
    if (color) {
      var selectedColor = {
        ColorId: color.colorId,
        Name: color.name
      }
    }

    const modalRef = this.modalService.open(EditColorComponent, { size: 'sm' });
    modalRef.componentInstance.color = selectedColor;
    modalRef.result.then((res) => {
      this.ColorService.fetch();
    });
  }

  /**
    * delete
  */
  delete(id: number) {
    const modalRef = this.modalService.open(DeleteconfirmationComponent);
    let data = {
      id: id,
      key: 'color',
    }
    modalRef.componentInstance.data = data;
    modalRef.result.then((res) => {
      if (res === 'yes') {
        this.ColorService.deleteColor(id).subscribe((response: any) => {
          if (response.result) {
            this.ColorService.fetch();
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


  redirectProduct(color: any) {
    this.router.navigate(['/products'], { queryParams: { idColor: color.colorId } })
  }
  //#endregion
}
