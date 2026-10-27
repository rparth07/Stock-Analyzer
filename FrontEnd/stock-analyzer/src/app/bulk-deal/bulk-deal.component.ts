import { Component, OnInit } from '@angular/core';
import { Form } from '@angular/forms';
import { BulkDeal, StockAction } from '../types/BulkDeal';
import { BulkDealService } from '../services/bulk-deal.service';
import { Table } from 'primeng/table';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { MAT_DATE_LOCALE, MAT_DATE_FORMATS, DateAdapter } from '@angular/material/core';
import { MY_DATE_FORMATS } from '../shared/domain.constants';

@Component({
  selector: 'app-bulk-deal',
  templateUrl: './bulk-deal.component.html',
  styleUrls: ['./bulk-deal.component.css',
    '../shared/css/common-page-style.css'],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' }, // choose your locale
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
})
export class BulkDealComponent implements OnInit {
  date: Date = new Date();//analyze bulkinfo date

  endDate: Date = new Date();
  startDate: Date = new Date(this.endDate.getTime() - 7 * 24 * 60 * 60 * 1000);

  bulkDeals: BulkDeal[] = [];
  selectedBulkDeals!: BulkDeal[];
  loading: boolean = true;
  loadError = false;
  dateError = '';

  analyzeBulkDealsOfDate() {
    this.bulkDealService.analyzeBulkDeals(this.date)
      .subscribe({ next: (res: string) => {
        this.reloadPage();
      }, error: () => {} });
  }

  reloadPage() {
    this.endDate = new Date();
    this.startDate = new Date(this.endDate.getTime() - 7 * 24 * 60 * 60 * 1000);
    window.location.reload();
  }

  getStockAction(value: number) {
    return StockAction[value];
  }

  clear(table: Table) {
    table.clear();
    (<HTMLInputElement>document.getElementById('search-value')).value = '';
  }

  constructor(private bulkDealService: BulkDealService) { }

  ngOnInit() {
    this.getBulkDealsBetween(this.startDate, this.endDate);
  }

  getAllBulkDeals() {
    return this.bulkDealService.fetchBulkDeals();
  }

  getBulkDealsBetween(startDate: Date, endDate: Date) {
    if (!startDate || !endDate || !Number.isFinite(new Date(startDate).getTime()) || !Number.isFinite(new Date(endDate).getTime()) || new Date(startDate) > new Date(endDate)) {
      this.dateError = 'Choose a valid date range with the start date on or before the end date.';
      this.loading = false; return;
    }
    this.dateError = ''; this.loading = true; this.loadError = false;
    // console.log("this is outside Start Date", startDate);
    this.bulkDealService.fetchBulkDealsBetween(startDate, endDate)
      .subscribe({ next: (bulkDeals: BulkDeal[]) => {
        this.bulkDeals = bulkDeals;
        this.loading = false;
        // console.log('this bulk deal is called!');
        this.bulkDeals.forEach((bulkDeal) => (bulkDeal.dealDate = new Date(<Date>bulkDeal.dealDate)));
      }, error: () => { this.loading = false; this.loadError = true; } });
  }
}
