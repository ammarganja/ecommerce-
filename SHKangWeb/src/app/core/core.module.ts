import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FirstLetterPipe } from './pipes/first-letter.pipe';
import { SafePipe } from './pipes/safe.pipe';
import { DeleteconfirmationComponent } from './components/deleteconfirmation/deleteconfirmation.component';
import { SubscribersService } from './services/subscribe.service';

@NgModule({
  declarations: [FirstLetterPipe, SafePipe, DeleteconfirmationComponent],
  imports: [CommonModule],
  exports: [FirstLetterPipe, SafePipe],
  providers:[SubscribersService]
})
export class CoreModule { }
