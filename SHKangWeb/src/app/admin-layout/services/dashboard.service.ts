import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { BaseService } from 'src/app/core/services/base.service';
import { map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  API_URL = `${environment.apiUrl}/admin/Dashboard/dashboard-data`;
  
  constructor(private httpClient: HttpClient, private baseService: BaseService) {
  }

  /**
   * Dashboard data
   */
  public dashboardData(){
    let headers = this.baseService.getHeaders(true);
    return this.httpClient.get(this.API_URL,{headers : headers}).pipe(
      map((response: any) => {
        return response;
      })
    )
  }
}
