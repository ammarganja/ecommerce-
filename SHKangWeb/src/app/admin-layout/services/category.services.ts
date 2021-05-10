import { Injectable, OnDestroy, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, forkJoin, Observable } from 'rxjs';
import { exhaustMap, map } from 'rxjs/operators';
import { TableService, TableResponseModel, ITableState, BaseModel, PaginatorState, SortState, GroupingState } from 'src/app/shared/crud-table';
// import { Category } from '../../_models/category.model';
import { Router } from '@angular/router';
// import { baseFilter } from '../../helpers/http-extensions';
import { environment } from 'src/environments/environment';
import { Category } from '../_models/category.model';
import { baseFilter } from '../helpers/http-extensions';
import { BaseService } from 'src/app/core/services/base.service';

const DEFAULT_STATE: ITableState = {
  filter: {},
  paginator: new PaginatorState(),
  sorting: new SortState(),
  searchTerm: '',
  grouping: new GroupingState(),
  entityId: undefined
};

@Injectable({
  providedIn: 'root'
})
export class CategorysService extends TableService<Category> implements OnDestroy {
  API_URL = `${environment.apiUrl}/admin/ProductCategoryType/`;
  constructor(@Inject(HttpClient) http,private baseService: BaseService) {
    super(http);
  }

  // READ
  find(tableState: ITableState): Observable<TableResponseModel<Category>> {
    var params = {
      searchString: tableState.filter?.searchString,
      pageNo: tableState.paginator.page,
      limit: tableState.paginator.pageSize,
      column:tableState.sorting.column,
      direction: tableState.sorting.direction
    }
    let headers = this.baseService.getHeaders(true);
    return this.http.post<Category[]>(this.API_URL+'product-category-type-list',params, { headers: headers }).pipe(
      map((response: any) => {
        ////const filteredResult = baseFilter(response, tableState);
        const result: TableResponseModel<Category> = {
          items: response.data.items,
          total: response.data.total
        };
        return result;
      })
    );
  }

  deleteItems(ids: number[] = []): Observable<any> {
    const tasks$ = [];
    ids.forEach(id => {
      tasks$.push(this.delete(id));
    });
    return forkJoin(tasks$);
  }

  updateStatusForItems(ids: number[], status: number): Observable<any> {
    return this.http.get<Category[]>(this.API_URL).pipe(
      map((Categorys: any[]) => {
        return Categorys.filter(c => ids.indexOf(c.id) > -1).map(c => {
          c.status = status;
          return c;
        });
      }),
      exhaustMap((Categorys: Category[]) => {
        const tasks$ = [];
        Categorys.forEach(Category => {
          tasks$.push(this.update(Category));
        });
        return forkJoin(tasks$);
      })
    );
  }

  createItem(formdata: any) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL+'add-product-category-type', formdata,{ headers: headers })
      .pipe(map(res => res));
  }

  deleteCategory(productCategoryTypeId: any,updatedBy:any) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL + 'delete-product-category-type', 
      { productCategoryTypeId: productCategoryTypeId,updatedBy:updatedBy }, { headers: headers })
      .pipe(map(res => res));
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sb => sb.unsubscribe());
  }
}
