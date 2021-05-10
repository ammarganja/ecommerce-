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
} from '../../shared/crud-table';
import { Router } from '@angular/router';
import { Customer } from '../_models/customer.model';
import { baseFilter } from '../helpers/http-extensions';
import { environment } from 'src/environments/environment';
import { BaseService } from 'src/app/core/services/base.service';

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
export class CustomersService
  extends TableService<Customer>
  implements OnDestroy {
  API_URL = `${environment.apiUrl}/admin/User/`;
  constructor(@Inject(HttpClient) http, private baseService: BaseService) {
    super(http);
  }

  // READ
  find(tableState: ITableState): Observable<TableResponseModel<Customer>> {
    var params = {
      searchString: tableState.filter?.searchString,
      pageNo: tableState.paginator.page,
      limit: tableState.paginator.pageSize,
      column: tableState.sorting.column,
      direction: tableState.sorting.direction,
      Status: tableState.filter?.Status,
    };

    let headers = this.baseService.getHeaders(true);

    return this.http
      .post<Customer[]>(this.API_URL + 'user-list', params, {
        headers: headers,
      })
      .pipe(
        map((response: any) => {
          const result: TableResponseModel<Customer> = {
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
    return this.http.get<Customer[]>(this.API_URL).pipe(
      map((customers: Customer[]) => {
        return customers
          .filter((c) => ids.indexOf(c.id) > -1)
          .map((c) => {
            c.status = status;
            return c;
          });
      }),
      exhaustMap((customers: Customer[]) => {
        const tasks$ = [];
        customers.forEach((customer) => {
          tasks$.push(this.update(customer));
        });
        return forkJoin(tasks$);
      })
    );
  }

  deleteCustomer(userId: any) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(
        this.API_URL + 'delete-user',
        { userId: userId },
        { headers: headers }
      )
      .pipe(map((res) => res));
  }

  changeCustomerStatus(userId: any, status: any) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(
        this.API_URL + 'change-user-status',
        { userId: userId, Status: status },
        { headers: headers }
      )
      .pipe(map((res) => res));
  }

  saveCustomer(params: any) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(
        this.API_URL + 'add-user',
        params,
        { headers: headers }
      )
      .pipe(map((res) => res));
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sb) => sb.unsubscribe());
  }
}
