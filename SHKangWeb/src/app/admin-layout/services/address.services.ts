import { Injectable, OnDestroy, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, forkJoin, Observable } from 'rxjs';
import { exhaustMap, map } from 'rxjs/operators';
import { TableService, TableResponseModel, ITableState, BaseModel, PaginatorState, SortState, GroupingState } from 'src/app/shared/crud-table';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { Address } from '../_models/address.model';
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
export class AddressService extends TableService<Address> implements OnDestroy {
  API_URL = `${environment.apiUrl}/admin/Address/`;
  Common_Address_URL = `${environment.apiUrl}/Common/`;
  constructor(@Inject(HttpClient) http,private baseService : BaseService) {
    super(http);
  }

  // READ
  find(tableState: ITableState): Observable<TableResponseModel<Address>> {
    var params = {
      searchString: tableState.filter?.searchString,
      pageNo: tableState.paginator.page,
      limit: tableState.paginator.pageSize,
      column:tableState.sorting.column,
      direction: tableState.sorting.direction,
      userId : tableState.filter?.userId
    }

    let headers = this.baseService.getHeaders(true);
    console.log(params)
    return this.http.post<Address[]>(this.Common_Address_URL+'user-address-list/',params, { headers: headers }).pipe(
      map((response: any) => {
        ////const filteredResult = baseFilter(response, tableState);
        const result: TableResponseModel<Address> = {
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
    return this.http.get<Address[]>(this.API_URL).pipe(
      map((Addresss: any[]) => {
        return Addresss.filter(c => ids.indexOf(c.id) > -1).map(c => {
          c.status = status;
          return c;
        });
      }),
      exhaustMap((Addresss: Address[]) => {
        const tasks$ = [];
        Addresss.forEach(Address => {
          tasks$.push(this.update(Address));
        });
        return forkJoin(tasks$);
      })
    );
  }

  deleteUserAddress(addressId: any) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.Common_Address_URL + 'delete-user-address', { userAddressId: addressId }, { headers: headers })
      .pipe(map(res => res));
  }

  createItem(formdata: any) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.Common_Address_URL+'add-user-address', formdata,{ headers: headers })
      .pipe(map(res => res));
  }


  ngOnDestroy() {
    this.subscriptions.forEach(sb => sb.unsubscribe());
  }
}
