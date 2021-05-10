import { Injectable } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AuthHTTPService } from './services/auth-http.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  // private fields
  private unsubscribe: Subscription[] = []; // Read more: => https://brianflove.com/2016/12/11/anguar-2-unsubscribe-observables/
  private authLocalStorageToken = `${environment.appVersion}-${environment.USERDATA_KEY}`;

  // public fields
  isLoading$: Observable<boolean>;
  isLoadingSubject: BehaviorSubject<boolean>;

  constructor(private route: Router, private authHttpService: AuthHTTPService) {
    this.isLoadingSubject = new BehaviorSubject<boolean>(false);
  }
  clear(): void {
    localStorage.clear();
  }
  isAuthenticated(): boolean {
    return localStorage.getItem('token') != null && !this.isTokenExpired();
  }
  isTokenExpired(): boolean {
    return false;
  }
  decode() {
    return localStorage.getItem('token');
  }
  returnUserRoles() {
    return localStorage.getItem('role');
  }

  forgotPassword(email: string): Observable<boolean> {
    this.isLoadingSubject.next(true);
    return this.authHttpService
      .forgotPassword(email)
      .pipe(finalize(() => this.isLoadingSubject.next(false)));
  }
}
