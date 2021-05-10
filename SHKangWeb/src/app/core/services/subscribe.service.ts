import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

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
  changed_userstatus: Subject<any> = new Subject<any>();
  //#endregion

  //#region Costructor
  constructor() { }
  //#endregion

  //#region functions
  /**
   * Change the user name
   * @param name 
   */
  changeUserStatus(status){
    this.changed_userstatus.next(status);
  }

  //#endregion
}
