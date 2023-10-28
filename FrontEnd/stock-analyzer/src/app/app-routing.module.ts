import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BhavInfoComponent } from './bhav-info/bhav-info.component';
import { CompanyListComponent } from './company-list/company-list.component';
import { BulkDealComponent } from './bulk-deal/bulk-deal.component';
import { ClientComponent } from './client/client.component';
import { ProfileComponent } from './profile/profile.component';
import { CompanyDetailComponent } from './company-list/company-detail/company-detail.component';
import { DashboardComponent } from './dashboard/dashboard.component';

const routes: Routes = [
  {
    path: 'dashboard',
    component: DashboardComponent
  },
  {
    path: 'bhavinfo',
    component: BhavInfoComponent
  },
  {
    path: 'bulkdeal',
    component: BulkDealComponent
  },
  {
    path: 'company-list',
    component: CompanyListComponent
  },
  {
    path: 'company-detail',
    component: CompanyDetailComponent,
  },
  {
    path: 'client',
    component: ClientComponent
  },
  {
    path: 'profile',
    component: ProfileComponent
  },
  {
    path: '**',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
