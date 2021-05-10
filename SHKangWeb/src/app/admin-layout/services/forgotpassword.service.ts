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
import { ForogtPassword } from '../_models/forgotpassword.model';
import { baseFilter } from '../helpers/http-extensions';


@Injectable({
  providedIn: 'root',
})
export class forgotpasswordService extends TableService<ForogtPassword> implements OnDestroy {
  API_URL = `${environment.apiUrl}/ForgotPassword/forgot-password`;
  constructor(@Inject(HttpClient) http) {
    super(http);
  }

  forgot_password(formdata) {
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
