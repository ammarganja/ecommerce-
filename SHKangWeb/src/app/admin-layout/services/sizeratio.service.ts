import { Injectable, OnDestroy, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, forkJoin, Observable } from 'rxjs';
import { exhaustMap, map } from 'rxjs/operators';
import { TableService, TableResponseModel, ITableState, BaseModel, PaginatorState, SortState, GroupingState } from 'src/app/shared/crud-table';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { sizeratio } from '../_models/sizeratio.model';
import { baseFilter } from '../helpers/http-extensions';

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
export class sizeratiosService extends TableService<sizeratio> implements OnDestroy {
  API_URL = `${environment.apiUrl}/sizeratios`;
  constructor(@Inject(HttpClient) http) {
    super(http);
  }

  // READ
  find(tableState: ITableState): Observable<TableResponseModel<sizeratio>> {
    return this.http.get<sizeratio[]>(this.API_URL).pipe(
      map((response: sizeratio[]) => {
        const filteredResult = baseFilter(response, tableState);
        const result: TableResponseModel<sizeratio> = {
          items: filteredResult.items,
          total: filteredResult.total
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
    return this.http.get<sizeratio[]>(this.API_URL).pipe(
      map((sizeratios: any[]) => {
        return sizeratios.filter(c => ids.indexOf(c.id) > -1).map(c => {
          c.status = status;
          return c;
        });
      }),
      exhaustMap((sizeratios: sizeratio[]) => {
        const tasks$ = [];
        sizeratios.forEach(sizeratio => {
          tasks$.push(this.update(sizeratio));
        });
        return forkJoin(tasks$);
      })
    );
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sb => sb.unsubscribe());
  }
}
