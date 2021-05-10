import { Component, OnInit } from '@angular/core';
import { PrivacyService } from '../../services/privacy.service';

@Component({
  selector: 'app-privacy',
  templateUrl: './privacy.component.html',
  styleUrls: ['./privacy.component.scss']
})
export class PrivacyComponent implements OnInit {

 
  Privacy:any;
  constructor(private privacyService : PrivacyService ) { }

  ngOnInit(): void {
    this.PrivacyPolicy()
  }

  PrivacyPolicy(){
    this.privacyService.Privacy({
      "shortName":"privacy"
  }).subscribe((data :any)=>{
    this.Privacy = data.data
      console.log(data);
    });
  }

}
