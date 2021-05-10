import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-thank-you',
  templateUrl: './thank-you.component.html',
  styleUrls: ['./thank-you.component.scss']
})
export class ThankYouComponent implements OnInit {

  constructor() {
      window.location.hash="no-back-button";
      window.location.hash="Again-No-back-button";
      window.onhashchange=function(){window.location.hash="no-back-button"};
   }

  ngOnInit(): void {
  }

}
