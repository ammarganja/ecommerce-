import { Injectable, OnDestroy, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, forkJoin, Observable } from 'rxjs';
import { exhaustMap, map } from 'rxjs/operators';
import { TableService, TableResponseModel, ITableState, BaseModel, PaginatorState, SortState, GroupingState } from '../../shared/crud-table';
import { createinvoice } from '../_models/createinvoice.model';
import { baseFilter } from '../helpers/http-extensions';
import { environment } from 'src/environments/environment';

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
export class CreateinvoiceService extends TableService<createinvoice> implements OnDestroy {
  API_URL = `${environment.apiUrl}/createinvoices`;
  constructor(@Inject(HttpClient) http) {
    super(http);
  }

  // READ
  find(tableState: ITableState): Observable<TableResponseModel<createinvoice>> {
    return this.http.get<createinvoice[]>(this.API_URL).pipe(
      map((response: createinvoice[]) => {
        const filteredResult = baseFilter(response, tableState);
        const result: TableResponseModel<createinvoice> = {
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
    return this.http.get<createinvoice[]>(this.API_URL).pipe(
      map((createinvoices: createinvoice[]) => {
        return createinvoices.filter(c => ids.indexOf(c.id) > -1).map(c => {
          c.status = status;
          return c;
        });
      }),
      exhaustMap((createinvoices: createinvoice[]) => {
        const tasks$ = [];
        createinvoices.forEach(createinvoice => {
          tasks$.push(this.update(createinvoice));
        });
        return forkJoin(tasks$);
      })
    );
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sb => sb.unsubscribe());
  }
}
