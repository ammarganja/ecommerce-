import { Component, OnInit } from '@angular/core';
import { TermsServices } from '../../services/terms.service';

@Component({
  selector: 'app-terms',
  templateUrl: './terms.component.html',
  styleUrls: ['./terms.component.scss']
})
export class TermsComponent implements OnInit {

  termsConditions:any;
  constructor(private termsServices : TermsServices ) { }

  ngOnInit(): void {
    this.terms()
  }

  terms(){
    this.termsServices.TermsConditions({
      "shortName":"Terms"
  }).subscribe((data :any)=>{
    this.termsConditions = data.data
      console.log(data);
    });
  }

}
