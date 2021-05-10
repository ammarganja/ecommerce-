import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import {
  GroupingState,
  ICreateAction,
  IDeleteAction,
  IEditAction,
  IGroupingView,
  ISortView,
  PaginatorState,
  SortState,
} from 'src/app/shared/crud-table';
import { CategorysService } from '../../services/category.services';
import { EditCategoryComponent } from './edit-category/edit-category.component';
import { categoryData } from './category.data';
import { DeleteconfirmationComponent } from 'src/app/core/components/deleteconfirmation/deleteconfirmation.component';
import { HelperService } from '../../services/helper.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.scss'],
})
export class CategoryComponent

  //#region Variable Declaration

  implements
  OnInit,
  ICreateAction,
  IEditAction,
  IDeleteAction,
  ISortView,
  IGroupingView {
  paginator: PaginatorState;
  sorting: SortState;
  grouping: GroupingState;
  isLoading: boolean;
  private subscriptions: Subscription[] = [];
  Columns: any[];
  categoryList = [];
  searchString = "";

  //#endregion

  //#region Constcructor

  constructor(
    private fb: FormBuilder,
    private modalService: NgbModal,
    public CategorysService: CategorysService,
    public helperservice: HelperService,
    private router: Router
  ) { }

  //#endregion

  //#region Methods

  /**
   * NgOnInit
   * angular lifecircle hooks
   */

  ngOnInit(): void {
    var filter = {
      "searchString": ''
    }

    ////this.categoryList = categoryData;
    this.CategorysService.paginator.page = 1;
    this.CategorysService.paginator.pageSize = 10;
    this.CategorysService.paginator.pageSizes = [];
    this.grouping = this.CategorysService.grouping;
    this.paginator = this.CategorysService.paginator;
    this.sorting = this.CategorysService.sorting;
    const sb = this.CategorysService.isLoading$.subscribe(
      (res) => (this.isLoading = res)
    );
    this.subscriptions.push(sb);

    /** Table Colums data */
    this.Columns = this.helperservice.getCategoryColumns();
    /**End Table Columns Data */

    this.CategorysService.patchState({ filter });
  }

  /**
 *search
 @param searchTerm (string)
 */

  search(searchTerm: string) {
    this.CategorysService.paginator.page = 1;
    var filter = {
      "searchString": searchTerm
    }
    this.CategorysService.patchState({ filter });
  }

  //#endsearch

  /**
 *sorting
 @param column (string)
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
    this.CategorysService.patchState({ sorting });
  }

  //#endsorting

  /**
   *pagination
   @param paginator (PaginatorState)
   */

  paginate(paginator: PaginatorState) {
    this.CategorysService.patchState({ paginator });
  }

  //#endpagination

  /**
 *form actions
 */

  create() {
    this.edit(undefined);
  }

  //#endform actions

  /**
 *edit Category
 @param customer (customer)
 */

  edit(category: any) {
    if (category) {
      var user = this.helperservice.getUserDetail();
      var selectedCategory = {
        CategoryType: category.name,
        ProductCategoryTypeId: category.productCategoryTypeId,
        UpdatedBy: user.userId
      }
    }

    const modalRef = this.modalService.open(EditCategoryComponent, {
      size: 'sm',
    });
    modalRef.componentInstance.category = selectedCategory;
    modalRef.result.then(
      () => this.CategorysService.fetch(),
      () => { }
    );
  }

  //#end editcategory


  /**
*delete popup
@param id (number)
*/

  delete(id: number) {
    const modalRef = this.modalService.open(DeleteconfirmationComponent);
    let data = {
      id: id,
      key: 'categoty',
    };
    modalRef.componentInstance.data = data;
    modalRef.result.then((res) => {
      if (res === 'yes') {
        var user = this.helperservice.getUserDetail();
        this.CategorysService.deleteCategory(id, user.userId).subscribe((response: any) => {
          if (response.result) {
            this.CategorysService.fetch();
          } else {

          }
        });
      }
    });
  }

  redirectProduct(category: any) {
    this.router.navigate(['/products'], { queryParams: { idCategory: category.productCategoryTypeId } })
  }

  //#end deletepopup
}
