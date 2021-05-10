import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HelperService } from 'src/app/core/helpers/helper-service.service';
import { AuthService } from 'src/app/core/services/auth/auth.service';
import { Base64Service } from 'src/app/core/utils/base64.service';

@Component({
  selector: 'app-profile-setting',
  templateUrl: './profile-setting.component.html',
  styleUrls: ['./profile-setting.component.scss']
})
export class ProfileSettingComponent implements OnInit {
  URL: any;

  constructor(public helperService: HelperService,private router: Router    ) { }

  ngOnInit(): void {
    this.getUserName();
    this.URL = this.router.url;

  }

  getUserName(){
    if(this.helperService.userName){
      return this.helperService.userName;
    }else{
      var userData : any = JSON.parse(Base64Service.decode(localStorage.getItem('userdata')));
      this.helperService.userName = userData.firstName + (userData.lastName ?   ' '+userData.lastName : '')
    }

    return '';
  }


}
