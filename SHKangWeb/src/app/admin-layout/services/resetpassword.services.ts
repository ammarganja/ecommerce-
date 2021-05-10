import { Injectable, OnDestroy, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, forkJoin, Observable } from 'rxjs';
import { catchError, exhaustMap, map } from 'rxjs/operators';
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
import { Login } from '../_models/login.model';
import { baseFilter } from '../helpers/http-extensions';

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
export class resetpasswordService extends TableService<Login> implements OnDestroy {
  API_URL = `${environment.apiUrl}/ForgotPassword/reset-password`;
  constructor(@Inject(HttpClient) http) {
    super(http);
  }

  resetPassword(formdata) {
    return this.http
      .post(this.API_URL, formdata)
      .pipe(map(res=>res));
  }

  handlerError(
    handlerError: any
  ): import('rxjs').OperatorFunction<Object, any> {
    throw new Error('Method not implemented.');
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sb) => sb.unsubscribe());
  }
}
