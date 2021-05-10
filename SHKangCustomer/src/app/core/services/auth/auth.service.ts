import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ResponseModel } from '../../models/response.model';
import { User } from '../../models/user';
import { BaseService } from '../../utils/base.service';

//#region Injector
@Injectable({
  providedIn: 'root',
})
//#endregion

//#region Auth Service
export class AuthService {
  // private currentUserSubject: BehaviorSubject<User>;
  // public currentUser: Observable<User>;

  API_URL = environment.apiUrl;

  //#region  Constructor
  constructor(private http: HttpClient, private baseService: BaseService) { }

  //#endregion

  login(data: any) {
    return this.http
      .post(this.API_URL + 'Login/login', data)
      .pipe(map((res) => res));
  }

  forgotPassword(fromdata: object) {
    return this.http.post(
      this.API_URL + 'ForgotPassword/forgot-password',
      fromdata
    );
  }
  register(fromdata: object) {
    return this.http.post(
      this.API_URL + 'Register/registration',
      fromdata
    );
  }
  resetpassword(formdata: any) {
    return this.http
      .post(this.API_URL + 'ForgotPassword/reset-password', formdata)
      .pipe(map(res => res));
  }

  // changepassword(formdata:any){
  //   return this.http
  //   .post(this.API_URL+'User/update-user-password', formdata)
  //   .pipe(map(res=>res));
  // }

  changepassword(data: any) {
    let headers = this.baseService.getHeaders(true);
    return this.http.post(this.API_URL + 'User/update-user-password', data, { headers: headers }
    ).pipe(
      map((response: any) => {
        return response;
      })
    );
  }


  //#region public methods
  public isUserAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    // Check whether the token is expired and return
    // true or false
    return token != null && token != undefined;
  }


  /**
   * Get user detail
   * @param requestModel 
   * @returns 
   */
  getUserDetail(requestModel: any): Observable<ResponseModel> {
    let headers = this.baseService.getHeaders(true);
    return this.http.post<any>(this.API_URL + 'Common/user-detail', requestModel, { headers: headers }).pipe(
      map((response: any) => {
        return response;
      })
    );
  }

  getUserOrderSummary(requestModel : any): Observable<ResponseModel> {
    let headers = this.baseService.getHeaders(true);
    return this.http.post<any>(this.API_URL + 'UserCart/get-cart-details', requestModel, { headers: headers }).pipe(
      map((response: any) => {
        return response;
      })
    );
  }

  getUserAddressList(requestModel : any): Observable<ResponseModel> {
    let headers = this.baseService.getHeaders(true);
    return this.http.post<any>(this.API_URL + 'Common/user-address-list', requestModel, { headers: headers }).pipe(
      map((response: any) => {
        return response;
      })
    );
  }

  updateUserOrderCart(requestModel : any): Observable<ResponseModel> {
    let headers = this.baseService.getHeaders(true);
    return this.http.post<any>(this.API_URL + 'UserCart/update-cart', requestModel, { headers: headers }).pipe(
      map((response: any) => {
        return response;
      })
    );
  }

  deleteUserAddress(requestModel : any): Observable<ResponseModel> {
    let headers = this.baseService.getHeaders(true);
    return this.http.post<any>(this.API_URL + 'Common/delete-user-address', requestModel, { headers: headers }).pipe(
      map((response: any) => {
        return response;
      })
    );
  }


  addUserOrder(requestModel : any): Observable<ResponseModel> {
    let headers = this.baseService.getHeaders(true);
    return this.http.post<any>(this.API_URL + 'Common/add-order', requestModel, { headers: headers }).pipe(
      map((response: any) => {
        return response;
      })
    );
  }

  removeCartProduct(requestModel : any): Observable<ResponseModel> {
    let headers = this.baseService.getHeaders(true);
    return this.http.post<any>(this.API_URL + 'UserCart/delete-item-cart', requestModel, { headers: headers }).pipe(
      map((response: any) => {
        return response;
      })
    );
  }

  checkPromoCode(requestModel : any): Observable<ResponseModel> {
    let headers = this.baseService.getHeaders(true);
    return this.http.post<any>(this.API_URL + 'Common/check-promocode', requestModel, { headers: headers }).pipe(
      map((response: any) => {
        return response;
      })
    );
  }
  // public get currentUserValue(): User {
  //   return this.currentUserSubject.value;
  // }

  // getLoginUserDetail(): Observable<any> {
  //   return this.baseService.get('/user/details')
  //     .pipe(map<HttpResponse<any>, any>(response => {
  //       return response.body.data as any;
  //     }, (err:any) => {
  //       return err;
  //     }));
  // }

  //#endregion
}
//#endregion
