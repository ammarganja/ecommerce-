import { Injectable, OnDestroy, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { TableService, ITableState, TableResponseModel } from 'src/app/shared/crud-table';
import { baseFilter } from '../helpers/http-extensions';
import { Product } from '../_models/product.model';

@Injectable({
  providedIn: 'root'
})
export class RemarksService extends TableService<Product> implements OnDestroy {
  API_URL = `${environment.apiUrl}/productRemarks`;
  constructor(@Inject(HttpClient) http) {
    super(http);
  }

  // READ
  find(tableState: ITableState): Observable<TableResponseModel<Product>> {
    return this.http.get<Product[]>(this.API_URL).pipe(
      map((response: Product[]) => {
        const filteredResult = baseFilter(response.filter(el => el.carId === tableState.entityId), tableState);
        const result: TableResponseModel<Product> = {
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

  ngOnDestroy() {
    this.subscriptions.forEach(sb => sb.unsubscribe());
  }
}
