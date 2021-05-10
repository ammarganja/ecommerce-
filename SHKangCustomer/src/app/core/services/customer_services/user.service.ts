import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { CampaignModel } from '../../models/campaign.model';
import { ProductDetail } from '../../models/product-detail.model';
import { ResponseModel } from '../../models/response.model';
import { BaseService } from '../../utils/base.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly API_URL: string = environment.apiUrl;

  constructor(private http: HttpClient, private baseService: BaseService) {}

  //#region functions

  /**
   * getCampaignList
   * @param requestModel
   * @returns
   */
  saveUserDetail(requestModel: any): Observable<ResponseModel> {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post<any>(
        this.API_URL + 'Common/update-user',
        requestModel,
        { headers: headers }
      )
      .pipe(
        map((response: any) => {
          return response;
        })
      );
  }
  
  /**
   * get user addresses
   * @param requestModel 
   * @returns 
   */
  getUserAddresses(requestModel: any): Observable<ResponseModel> {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post<any>(
        this.API_URL + 'Common/user-address-list',
        requestModel,
        { headers: headers }
      )
      .pipe(
        map((response: any) => {
          return response;
        })
      );
  }

  /**
   * get country list
   * @returns 
   */
  getCountries(): Observable<any> {
    let headers = this.baseService.getHeaders(true);
    return this.http.post<any[]>(this.API_URL + 'Common/get-country-list',{}, { headers: headers }).pipe(
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
    return this.http.post<any[]>(this.API_URL + 'Common/get-state-list',{ countryId: countryId }, { headers: headers }).pipe(
      map((response: any) => {
        return response;
      })
    );
  }

  /**
   * Add User Address
   * @param requestModel 
   * @returns 
   */
  saveUserAddress(requestModel: any): Observable<ResponseModel> {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post<any>(
        this.API_URL + 'Common/add-user-address',
        requestModel,
        { headers: headers }
      )
      .pipe(
        map((response: any) => {
          return response;
        })
      );
  }
  
  //#endregion
}
