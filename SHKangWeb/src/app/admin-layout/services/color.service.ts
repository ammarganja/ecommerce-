

import { Injectable, OnDestroy, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, forkJoin, Observable } from 'rxjs';
import { exhaustMap, map } from 'rxjs/operators';
import { TableService, TableResponseModel, ITableState, BaseModel, PaginatorState, SortState, GroupingState } from 'src/app/shared/crud-table';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { baseFilter } from '../helpers/http-extensions';
import { Color } from '../_models/color.model';
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
export class ColorService extends TableService<Color> implements OnDestroy {
  API_URL = `${environment.apiUrl}/admin/color/`;
  constructor(@Inject(HttpClient) http, private baseService: BaseService) {
    super(http);
  }

  // READ
  find(tableState: ITableState): Observable<TableResponseModel<Color>> {
    var params = {
      searchString: tableState.filter?.searchString,
      pageNo: tableState.paginator.page,
      limit: tableState.paginator.pageSize,
      column:tableState.sorting.column,
      direction: tableState.sorting.direction
    }

    let headers = this.baseService.getHeaders(true);
    return this.http.post<Color[]>(this.API_URL + 'color-list',params, { headers: headers }).pipe(
      map((response: any) => {
        ////const filteredResult = baseFilter(response, tableState);
        const result: TableResponseModel<Color> = {
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
    return this.http.get<Color[]>(this.API_URL).pipe(
      map((Colors: Color[]) => {
        return Colors.filter(c => ids.indexOf(c.id) > -1).map(c => {
          c.status = status;
          return c;
        });
      }),
      exhaustMap((Colors: Color[]) => {
        const tasks$ = [];
        Colors.forEach(Color => {
          tasks$.push(this.update(Color));
        });
        return forkJoin(tasks$);
      })
    );
  }

  createItem(formdata: any) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL+'add-color', formdata,{ headers: headers })
      .pipe(map(res => res));
  }

  deleteColor(colorId: any) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL + 'delete-color', { colorId: colorId }, { headers: headers })
      .pipe(map(res => res));
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sb => sb.unsubscribe());
  }
}
