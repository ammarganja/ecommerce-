import { HttpClient, HttpResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { BaseService } from '../utils/base.service';

//#region Injector
@Injectable({
  providedIn: 'root',
})
//#endregion

//#region Auth Service
export class FrontendService {
}
