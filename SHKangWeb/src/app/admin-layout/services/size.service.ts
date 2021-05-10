
import { Injectable, OnDestroy, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, forkJoin, Observable } from 'rxjs';
import { exhaustMap, map } from 'rxjs/operators';
import { TableService, TableResponseModel, ITableState, BaseModel, PaginatorState, SortState, GroupingState } from '../../shared/crud-table';
import { Router } from '@angular/router';
import { Size } from '../_models/size.model';
import { baseFilter } from '../helpers/http-extensions';
import { environment } from 'src/environments/environment';
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
export class SizeService extends TableService<Size> implements OnDestroy {
  API_URL = `${environment.apiUrl}/admin/Size/`;
  constructor(@Inject(HttpClient) http, private baseService: BaseService) {
    super(http);
  }

  // READ
  find(tableState: ITableState): Observable<TableResponseModel<Size>> {
    var params = {
      searchString: tableState.filter?.searchString,
      pageNo: tableState.paginator.page,
      limit: tableState.paginator.pageSize,
      column:tableState.sorting.column,
      direction: tableState.sorting.direction
    }

    let headers = this.baseService.getHeaders(true);
    return this.http.post<Size[]>(this.API_URL + 'size-list',params, { headers: headers }).pipe(
      map((response: any) => {
        ////const filteredResult = baseFilter(response, tableState);
        const result: TableResponseModel<Size> = {
          items: response.data.items,
          total: response.data.total
        };
        return result;
      })
    );
  }

  deletesize(sizeId: any) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL + 'delete-size', { sizeId: sizeId }, { headers: headers })
      .pipe(map(res => res));
  }

  createItem(formdata: any) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL+'add-size', formdata,{ headers: headers })
      .pipe(map(res => res));
  }

  updateStatusForItems(ids: any, status: any): Observable<any> {
    return this.http.get<Size[]>(this.API_URL).pipe(
      map((Sizes: Size[]) => {
        return Sizes.filter(c => ids.indexOf(c.id) > -1).map(c => {
          c.status = status;
          return c;
        });
      }),
      exhaustMap((Sizes: Size[]) => {
        const tasks$ = [];
        Sizes.forEach(Size => {
          tasks$.push(this.update(Size));
        });
        return forkJoin(tasks$);
      })
    );
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sb => sb.unsubscribe());
  }
}
