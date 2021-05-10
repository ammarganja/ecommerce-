import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from './core/services/guards/auth-guard.service';

const routes: Routes = [
  {
  path: 'customer',
  loadChildren: () => import('./customer-module/customer-module')
    .then(mod => mod.CustomerModule)
},
{
  path: '',
  loadChildren: () => import('./authmodule/auth.module')
      .then(mod => mod.AuthModule)
}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
