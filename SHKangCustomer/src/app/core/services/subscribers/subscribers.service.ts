import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

/**
 * Subscribers service.
 * This is the service which is for those variables which needs to be global.
 * When there is a change in the variable then the component will call this service method
 * and change the variable value.
 * We need to subscribe this service wherver it needs.
 */
export class SubscribersService {

  //#region Variables
  changed_username: Subject<string> = new Subject<string>();
  headDrawer:Subject<boolean> = new Subject<boolean>();
  private searchProduct = new Subject<any>();
  //#endregion

  //#region Costructor
  constructor() { }
  //#endregion

  //#region functions
  /**
   * Change the user name
   * @param name 
   */
  changeUserName(name:any){
    this.changed_username.next(name);
  }

  headDrawerToggle(flag: boolean){
    this.headDrawer.next(flag);
  }
  searchTheProduct(product : string){
    this.searchProduct.next({ text: product });
  }

  getSearchedDetail(){
    return this.searchProduct.asObservable();
  }

  //#endregion
}
