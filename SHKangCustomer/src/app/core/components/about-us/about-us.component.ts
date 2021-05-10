import { Component, OnInit } from '@angular/core';
import { AboutusService } from '../../services/aboutus.service';
import { TestimonialsService } from '../../services/testimonial.service';
// import { testimonialservice } from '../../services/testimonial.service';

@Component({
  selector: 'app-about-us',
  templateUrl: './about-us.component.html',
  styleUrls: ['./about-us.component.scss']
})
export class AboutUsComponent implements OnInit {

  SlideOptions = { items: 3, dots: false, nav: true };  
  CarouselOptions = { items: 3, dots: true, nav: true };

  aboutus : any;
  testimonials:any;
  Images = ['assets/images/testimonial-img1.png', 
  'assets/images/testimonial-img2.png', 
  'assets/images/testimonial-img3.png'];
  constructor(private aboutusService : AboutusService, private testimonialsService : TestimonialsService) { }

  ngOnInit(): void {
    this.about()
    this.testimonial()
  }

about(){
  this.aboutusService.aboutus({
    "shortName":"about"
}).subscribe((data :any)=>{
  this.aboutus = data.data
  });
}
  
params = {
  "pageNo": 1,
  "limit": 5,
  "searchString": "",
  "column": "",
  "direction": "",
}

testimonial(){
  this.testimonialsService.getTestimonialList(this.params).subscribe((testmonials :any)=>{
  this.testimonials = testmonials.data.items
  });
}
}
