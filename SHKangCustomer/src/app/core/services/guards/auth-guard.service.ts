import { Injectable, NgModule } from '@angular/core';
import { HelperService } from '../../helpers/helper-service.service';
import { NgModel } from '@angular/forms';
import {
  Router,
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
} from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs/internal/Observable';
import { LoginpopupComponent } from 'src/app/authmodule/loginpopup/loginpopup.component';
// import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuardService implements CanActivate {
  constructor(private router: Router,private model:NgbModal,public helperService: HelperService)
  {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot){
    if(localStorage.getItem('token')){
      return true;
    }
    else{
      this.helperService.isLogIn = false;
      this.model.open(LoginpopupComponent,{centered:true,windowClass:'login-modal in'})
      return false;
    }
    localStorage.setItem('returnUrl', JSON.stringify(state.url));
  }

}
