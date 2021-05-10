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


@Injectable({
  providedIn: 'root'
})
export class CommonService{
  API_URL = `${environment.apiUrl}/admin/Master/`;
  constructor(private  http : HttpClient,private baseService : BaseService) {
  }

  /***
   * get countries
   */
  getCountries(): Observable<any> {
    let headers = this.baseService.getHeaders(true);
    return this.http.post<any[]>(this.API_URL + 'get-country-list',{}, { headers: headers }).pipe(
      map((response: any) => {
        return response;
      })
    );
  }

  /**
   * get states by country
   * @param countryId 
   * @returns 
   */
  getStates(countryId: any): Observable<any> {
    let headers = this.baseService.getHeaders(true);
    return this.http.post<any[]>(this.API_URL + 'get-state-list',{ countryId: countryId }, { headers: headers }).pipe(
      map((response: any) => {
        return response;
      })
    );
  }
}
