import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SpinnerService {

  // A BehaviourSubject is an Observable with a default value
  public isLoading = new BehaviorSubject<boolean>(false);
  
  constructor() { }

  show() {
    this.isLoading.next(true);
    localStorage.setItem("loader","true");
  }
  hide() {
    this.isLoading.next(false);
    localStorage.setItem("loader","false");
  }
}