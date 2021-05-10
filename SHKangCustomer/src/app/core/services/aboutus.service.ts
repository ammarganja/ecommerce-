import { HttpClient, HttpResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ResponseModel } from '../models/response.model';
import { TestimonialModel } from '../models/testimonial.model';
import { BaseService } from '../utils/base.service';

//#region Injector
@Injectable({
  providedIn: 'root',
})
//#endregion

//#region Auth Service
export class AboutusService {
  API_URL = environment.apiUrl;
  constructor(private http: HttpClient, private baseService: BaseService) {}

  aboutus(data: any) {
      
    return this.http.post(this.API_URL+'Content/content-detail', data,{
    }).pipe(map((res) =>{
        return res
    }));
  }


  getTestimonialList(requestModel: any): Observable<ResponseModel> {
    let headers = this.baseService.getHeaders(true);
    return this.http.post<TestimonialModel[]>(this.API_URL + 'Common/testimonial-list', requestModel, { headers: headers }).pipe(
        map((response: any) => {
            return response;
        })
    );
}
}