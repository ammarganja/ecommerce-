import { Injectable, OnDestroy, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, forkJoin, Observable } from 'rxjs';
import { exhaustMap, map } from 'rxjs/operators';
import {
  TableService,
  TableResponseModel,
  ITableState,
  BaseModel,
  PaginatorState,
  SortState,
  GroupingState,
} from 'src/app/shared/crud-table';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { Orders } from '../_models/orders.model';
import { baseFilter } from '../helpers/http-extensions';
import { BaseService } from 'src/app/core/services/base.service';
import { Product } from '../_models/product.model';

const DEFAULT_STATE: ITableState = {
  filter: {},
  paginator: new PaginatorState(),
  sorting: new SortState(),
  searchTerm: '',
  grouping: new GroupingState(),
  entityId: undefined,
};

@Injectable({
  providedIn: 'root',
})
export class orderService extends TableService<Orders> implements OnDestroy {
  API_URL = `${environment.apiUrl}/Common/`;
  constructor(@Inject(HttpClient) http, private baseService: BaseService) {
    super(http);
  }

  find(tableState: ITableState): Observable<TableResponseModel<Orders>> {
    var params = {
      searchString: tableState.filter?.searchString,
      pageNo: tableState.paginator.page,
      limit: tableState.paginator.pageSize,
      column: tableState.sorting.column,
      direction: tableState.sorting.direction,
    };

    let headers = this.baseService.getHeaders(true);
    return this.http
      .post<Orders[]>(this.API_URL + 'order-list', params, { headers: headers })
      .pipe(
        map((response: any) => {
          ////const filteredResult = baseFilter(response, tableState);
          const result: TableResponseModel<Orders> = {
            items: response.data.items,
            total: response.data.total,
          };
          return result;
        })
      );
  }
  deleteItems(ids: number[] = []): Observable<any> {
    const tasks$ = [];
    ids.forEach((id) => {
      tasks$.push(this.delete(id));
    });
    return forkJoin(tasks$);
  }

  updateStatusForItems(ids: number[], status: number): Observable<any> {
    return this.http.get<Orders[]>(this.API_URL).pipe(
      map((Orders: Orders[]) => {
        return Orders.filter((c) => ids.indexOf(c.id) > -1).map((c) => {
          c.status = status;
          return c;
        });
      }),
      exhaustMap((Orders: Orders[]) => {
        const tasks$ = [];
        Orders.forEach((Orders) => {
          tasks$.push(this.update(Orders));
        });
        return forkJoin(tasks$);
      })
    );
  }

  orderstatusChange(params) {
    let API = `${environment.apiUrl}/admin/OrderInvoice/update-order-invoice-status`;
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post<Orders[]>(API, params, { headers: headers })
      .pipe(
        map((response: any) => {
          return response;
        })
      );
  }

  deleteOrder(formdata) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post<Orders[]>(this.API_URL + 'delete-order', formdata, {
        headers: headers,
      })
      .pipe(
        map((response: any) => {
          console.log(response);
          return response;
        })
      );
  }

  getInvoiceDataById(formdata) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post<Orders[]>(
        `${environment.apiUrl}` + '/admin/OrderInvoice/order-invoice-detail',
        formdata,
        { headers: headers }
      )
      .pipe(
        map((response: any) => {
          console.log(response);
          return response;
        })
      );
  }

  addorderProductList(search) {
    let headers = this.baseService.getHeaders(true);
    var params = {
      pageNo: 1,
      limit: 10,
      searchString: search?search:'',
      column: '',
      direction: '',
    };
    return this.http
      .post(
        `${environment.apiUrl}` + '/admin/Product/product-order-list',
        params,
        { headers: headers }
      )
      .pipe(
        map((response: any) => {
          const result: TableResponseModel<Product> = {
            items: response.data.items,
            total: response.data.total,
          };
          return result;
        })
      );
  }

  getcustomersListing() {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(`${environment.apiUrl}` + '/admin/Master/get-user-list', null, {
        headers: headers,
      })
      .pipe(map((res) => res));
  }
  getAddressFromUserId(formdata) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(
        `${environment.apiUrl}` + '/admin/User/user-address-list',
        formdata,
        {
          headers: headers,
        }
      )
      .pipe(map((res) => res));
  }
  addNewOrder(formdata) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(`${environment.apiUrl}` + '/Common/add-order', formdata, {
        headers: headers,
      }).pipe(map((res) => res));
  }

  editOrderGEtData(formData) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(`${environment.apiUrl}` + '/Common/get-edit-order', formData, {
        headers: headers,
      }).pipe(map((res) => res));
  }

  getInvoiceDataToCreate(formData){
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(`${environment.apiUrl}` + '/admin/OrderInvoice/get-order-invoice', formData, {
        headers: headers,
      }).pipe(map((res) => res));
  }
  CreateNewInvoice(formData){
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(`${environment.apiUrl}` + '/admin/OrderInvoice/add-order-invoice', formData, {
        headers: headers,
      }).pipe(map((res) => res));
  }

  sendEmailInvoice(formdata){
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(`${environment.apiUrl}` + '/admin/OrderInvoice/email-order-invoice', formdata, {
        headers: headers,
      }).pipe(map((res) => res));
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sb) => sb.unsubscribe());
  }
}
