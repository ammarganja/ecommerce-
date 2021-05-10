import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BaseService {

  /** Region: Variable Declaration */
  private readonly API_URL: string = environment.apiUrl;
  /** End Region */

  /** Region: Constructor */
  constructor(
    private http: HttpClient,
    private router: Router,
  ) { }
  /** EndRegion */

  get(url: string,requestParamModel?:any) {
    const fullURL = this.getURL(url,requestParamModel);
    const headers = this.getHeaders();
    return this.http.get(fullURL, {
      headers: headers,
      observe: 'response'
    });
  }

  post(url: string, bodyModel: any) {
    const fullURL = this.getURL(url);
    const headers = this.getHeaders();
    return this.http.post(fullURL, bodyModel, {
      headers: headers
    });
  }

  patch(url: string, bodyModel: any) {
    const fullURL = this.getURL(url);
    const headers = this.getHeaders();
    return this.http.patch(fullURL, bodyModel, {
      headers: headers
    });
  }

  put(url: string, bodyModel: any) {
    const fullURL = this.getURL(url);
    const headers = this.getHeaders();
    return this.http.patch(fullURL, bodyModel, {
      headers: headers
    });
  }

  delete(url: string) {
    const fullURL = this.getURL(url);
    const headers = this.getHeaders();
    return this.http.delete(fullURL, {
      headers: headers
    });
  }

  postWithoutToken(url: string, bodyModel: any) {
    const fullURL = this.getURL(url);
    const headers = this.getHeaders(false);
    return this.http.post(fullURL, bodyModel, {
      headers: headers
    });
  }

  patchWithoutToken(url: string, bodyModel: any) {
    const fullURL = this.getURL(url);
    const headers = this.getHeaders(false);
    return this.http.post(fullURL, bodyModel, {
      headers: headers
    });
  }
  upload(url: string, bodyModel: FormData) {
    const fullURL = this.getURL(url);
    let headers: HttpHeaders = new HttpHeaders();
    headers = headers.append('Authorization', this.getAccessToken());
    return this.http.post(fullURL, bodyModel, {
      headers: headers
    });
  }

  public getURL(url: string,requestParamModel?: any): string {
    let newURL: string = this.API_URL + url;
    return newURL;
  }

  public getAccessToken(): string {
    const token = localStorage.getItem('token');
    if (token !== null) {
      return 'Bearer ' + token;
    } else {
      ////this.notification.error('Login session expired.');
      this.router.navigate(['/customer/home']);
    }
    return '';
  }

  public getHeaders(authorizationHeader: boolean = true): HttpHeaders {
    let headers: HttpHeaders = new HttpHeaders();
    headers = headers.append('Accept', 'application/json');
    headers = headers.append('Content-Type', 'application/json');
    if (authorizationHeader) {
      headers = headers.append('Authorization', this.getAccessToken());
    }
    return headers;
  }

  public getHeadersForImage(authorizationHeader: boolean = true): HttpHeaders {
    let headers: HttpHeaders = new HttpHeaders();
    // headers = headers.append('Accept', 'application/json');
    //headers = headers.append('Content-Type', 'application/x-www-form-urlencoded');
    if (authorizationHeader) {
      headers = headers.append('Authorization', this.getAccessToken());
    }
    return headers;
  }

  public getHttpParams(params:any){
    let httpParams : HttpParams = new HttpParams();
    httpParams = httpParams.append('searchString', params.searchString);
    httpParams = httpParams.append('pageNo', params.pageNo);
    httpParams = httpParams.append('limit', params.limit);
    return httpParams;
  }
  /** EndRegion */
}
