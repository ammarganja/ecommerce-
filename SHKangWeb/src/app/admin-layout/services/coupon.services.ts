import { Injectable, OnDestroy, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, forkJoin, Observable } from 'rxjs';
import { exhaustMap, map } from 'rxjs/operators';
import { TableService, TableResponseModel, ITableState, BaseModel, PaginatorState, SortState, GroupingState } from 'src/app/shared/crud-table';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { Coupon } from '../_models/coupon.model';
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
export class CouponsService extends TableService<Coupon> implements OnDestroy {
  API_URL = `${environment.apiUrl}/admin/PromoCode/`;
  constructor(@Inject(HttpClient) http,private baseService : BaseService) {
    super(http);
  }

  // READ
  find(tableState: ITableState): Observable<TableResponseModel<Coupon>> {
    var params = {
      searchString: tableState.filter?.searchString,
      pageNo: tableState.paginator.page,
      limit: tableState.paginator.pageSize,
      column:tableState.sorting.column,
      direction: tableState.sorting.direction
    }
    let headers = this.baseService.getHeaders(true);
    
    return this.http.post<Coupon[]>(this.API_URL + 'promo-code-list',params, { headers: headers }).pipe(
      map((response: any) => {
        const result: TableResponseModel<Coupon> = {
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
    return this.http.get<Coupon[]>(this.API_URL).pipe(
      map((Coupons: Coupon[]) => {
        return Coupons.filter(c => ids.indexOf(c.id) > -1).map(c => {
          c.status = status;
          return c;
        });
      }),
      exhaustMap((Coupons: Coupon[]) => {
        const tasks$ = [];
        Coupons.forEach(Coupon => {
          tasks$.push(this.update(Coupon));
        });
        return forkJoin(tasks$);
      })
    );
  }

  createItem(formdata: any) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL+'add-promo-code', formdata,{ headers: headers })
      .pipe(map(res => res));
  }

  deleteCoupan(params: any) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL + 'delete-promo-code', params, { headers: headers })
      .pipe(map(res => res));
  }

  changeCoupanStatus(params: any) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL + 'change-promo-code-status', params, { headers: headers })
      .pipe(map(res => res));
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sb => sb.unsubscribe());
  }
}
