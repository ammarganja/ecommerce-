import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { SessionModel } from '../models/sessionmodel';
import { Base64Service } from '../utils/base64.service';

@Injectable({
  providedIn: 'root'
})
export class HelperService {

  public cartCount = 0;
  public userName = '';
  data = localStorage.getItem('userdata');
  userData : any;
  public isLogIn = false;
  public productList: any = [];

  constructor(private router:Router) {
    if(this.data){
      this.userData =  JSON.parse(Base64Service.decode(localStorage.getItem('userdata')));
      this.isLogIn = false || this.userData.userId;
    }
   }

  Number(value: any) {
    return !isNaN(value);
  }

  setToken(SessionModel: SessionModel): void {
    localStorage.removeItem('token');
    const base64Value = Base64Service.encode(SessionModel.access_token);
    localStorage.setItem('token', base64Value);
  }

  encodeData(storeData: string, localStorageText: string): void {
    localStorage.removeItem(localStorageText);
    const base64Value = Base64Service.encode(storeData);
    localStorage.setItem(localStorageText, base64Value);
  }

  decodeData(storedData: string): string {
    const normalText = Base64Service.decode(storedData);
    return normalText;
  }
}
