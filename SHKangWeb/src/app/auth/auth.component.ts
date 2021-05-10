import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss']
})
export class AuthComponent implements OnInit {
  today: Date = new Date();
  currentYear: string;
  constructor() {
    const currentDate = new Date();
    this.currentYear = currentDate.getFullYear().toString();
   }

  ngOnInit(): void {
  }

}
