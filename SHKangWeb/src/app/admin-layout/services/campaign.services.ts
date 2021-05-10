import { Injectable, OnDestroy, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, forkJoin, Observable } from 'rxjs';
import { exhaustMap, map } from 'rxjs/operators';
import { TableService, TableResponseModel, ITableState, BaseModel, PaginatorState, SortState, GroupingState } from 'src/app/shared/crud-table';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { campaign } from '../_models/campaign.model';
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
export class campaignService extends TableService<campaign> implements OnDestroy {
  API_URL = `${environment.apiUrl}/Common/`;
  CAMPAIGN_APIURL = `${environment.apiUrl}/admin/Campaign/`;
  constructor(@Inject(HttpClient) http, private baseService: BaseService) {
    super(http);
  }

  // READ
  find(tableState: ITableState): Observable<TableResponseModel<campaign>> {
    var params = {
      searchString: tableState.filter?.searchString,
      pageNo: tableState.paginator.page,
      limit: tableState.paginator.pageSize,
      column: tableState.sorting.column,
      direction: tableState.sorting.direction
    }

    let headers = this.baseService.getHeaders(true);
    return this.http.post<campaign[]>(this.API_URL + 'get-campaign-list', params, { headers: headers }).pipe(
      map((response: any) => {
        ////const filteredResult = baseFilter(response, tableState);
        const result: TableResponseModel<campaign> = {
          items: response.data.items,
          total: response.data.total
        };
        return result;
      })
    );
  }

  // deleteItems(ids: number[] = []): Observable<any> {
  //   const tasks$ = [];
  //   ids.forEach(id => {
  //     tasks$.push(this.delete(id));
  //   });
  //   return forkJoin(tasks$);
  // }

  deletecampaign(formdata: any) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post<campaign[]>(this.CAMPAIGN_APIURL + 'delete-campaign', formdata, {
        headers: headers,
      })
      .pipe(
        map((response: any) => {
          console.log(response);
          return response;
        })
      );
  }

  updateStatusForItems(ids: number[], status: number): Observable<any> {
    return this.http.get<campaign[]>(this.API_URL).pipe(
      map((campaigns: any[]) => {
        return campaigns.filter(c => ids.indexOf(c.id) > -1).map(c => {
          c.status = status;
          return c;
        });
      }),
      exhaustMap((campaigns: campaign[]) => {
        const tasks$ = [];
        campaigns.forEach(campaign => {
          tasks$.push(this.update(campaign));
        });
        return forkJoin(tasks$);
      })
    );
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sb => sb.unsubscribe());
  }
}
