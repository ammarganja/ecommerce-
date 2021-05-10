import { Injectable } from '@angular/core';
import {
  Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
} from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LoginpopupComponent } from './authmodule/loginpopup/loginpopup.component';

@Injectable({
  providedIn: 'root',
})
export class AuthServiceService {
  constructor(private _router: Router, private model: NgbModal) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    //check some condition
    if (!localStorage.getItem('userId')) {
      this.model.open(LoginpopupComponent, {
        windowClass: 'login-modal in',
        centered: true,
      });
    }
    return true;
  }
}
