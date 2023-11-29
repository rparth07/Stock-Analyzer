import { Component, OnInit } from '@angular/core';
import { Form } from '@angular/forms';
import { BulkDeal, StockAction } from '../types/BulkDeal';
import { BulkDealService } from '../services/bulk-deal.service';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-bulk-deal',
  templateUrl: './bulk-deal.component.html',
  styleUrls: ['./bulk-deal.component.css',
    '../shared/css/common-page-style.css']
})
export class BulkDealComponent implements OnInit {
  date: Date = new Date();//analyze bulkinfo date

  endDate: Date = new Date();
  startDate: Date = new Date(this.endDate.getTime() - 7 * 24 * 60 * 60 * 1000);

  analyzeBulkDealsOfDate() {
    this.bulkDealService.analyzeBulkDeals(this.date);
  }

  bulkDeals!: BulkDeal[];
  selectedBulkDeals!: BulkDeal[];
  loading: boolean = true;

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
    console.log("this is outside Start Date", startDate);
    this.bulkDealService.fetchBulkDealsBetween(startDate, endDate)
      .subscribe((bulkDeals: BulkDeal[]) => {
        this.bulkDeals = bulkDeals;
        this.loading = false;
        console.log('this bulk deal is called!');
        this.bulkDeals.forEach((bulkDeal) => (bulkDeal.dealDate = new Date(<Date>bulkDeal.dealDate)));
      });
  }
}
