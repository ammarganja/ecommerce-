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
export class CampaignProductService extends TableService<campaign> implements OnDestroy {
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
      direction: tableState.sorting.direction,
      ProductCategoryTypeId: tableState.filter?.ProductCategoryTypeId
    }
    
    let headers = this.baseService.getHeaders(true);
    return this.http.post<campaign[]>(this.CAMPAIGN_APIURL + 'get-campaign-product-list', params, { headers: headers }).pipe(
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

  createItem(campaignData: any) {
    let headers = this.baseService.getHeadersForImage(true);
    const formData = new FormData();

    formData.append('CampaignId', campaignData.CampaignId);
    formData.append('CampaignName', campaignData.CampaignName);
    formData.append('Description', campaignData.Description);
    formData.append('Image', campaignData.Image);
    formData.append('ProductId', campaignData.ProductId);
    formData.append('UpdatedBy', campaignData.UpdatedBy);
    formData.append('IsImageDeleted', campaignData.IsImageDeleted);
    formData.append('IsShowinFrontend', campaignData.IsShowinFrontend);
    formData.append('file[]', campaignData.Image);

    return this.http
      .post(this.CAMPAIGN_APIURL+'add-campaign', formData,{ headers: headers })
      .pipe(map(res => res));
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sb => sb.unsubscribe());
  }
}
