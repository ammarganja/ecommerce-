import { HttpClient, HttpResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { BaseService } from '../../utils/base.service';

//#region Injector
@Injectable({
  providedIn: 'root',
})
//#endregion

//#region Auth Service
export class MyorderService {
  API_URL = environment.apiUrl;
  constructor(private http: HttpClient, private baseService: BaseService) {}

  getMyOrder(requestModel: any) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL + 'Order/user-order-detail', requestModel, {
        headers: headers,
      })
      .pipe(
        map((response: any) => {
          return response;
        })
      );
  }
  getInvoiceDetails(formdata:any) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL + 'Common/order-invoice-detail', formdata, {
        headers: headers,
      })
      .pipe(
        map((response: any) => {
          return response;
        })
      );
  }
  getTrackingDetails(formdata:any){
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL + 'Order/track-order', formdata, {
        headers: headers,
      })
      .pipe(
        map((response: any) => {
          return response;
        })
      );
  }

  MakePayment(formdata:any){
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL + 'Order/process-invoice-payment', formdata, {
        headers: headers,
      })
      .pipe(
        map((response: any) => {
          return response;
        })
      );
  }
}
