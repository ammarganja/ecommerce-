import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { HelperService } from "../helpers/helper-service.service";

// Error Interceptor
@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    
    //#region Constructor
    constructor(private router: Router,private  toastr: ToastrService,public helperService: HelperService) { }
    //#endregion

    //#region functions
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(catchError(err => {

            let error = '';
            if(err.error && err.error.message){
                error = err.error.message;
            }else{
                error = err.statusText;
            }
            if ([401, 403, 500].indexOf(err.status) !== -1) {
                // if 401 Unauthorized or 403 Forbidden response returned from api
                if (err.status == 403) {
                    this.router.navigate(['403']);
                } else if (err.status == 401) {
                    localStorage.clear();
                    this.toastr.error("error","Session has been timedout");
                    this.helperService.isLogIn = false;
                    this.router.navigate(['/customer/home']);
                } else if (err.status == 500) {
                    this.router.navigate(['500']);
                }
            }else{
                this.toastr.error("error",error);
            }
            return throwError(error);
        }))
    }
    //#endregion
}
