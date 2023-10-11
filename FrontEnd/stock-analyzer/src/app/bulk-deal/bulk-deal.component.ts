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
  date: Date = new Date();

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
    this.getAllBulkDeals()
      .subscribe((bulkDeals) => {
        this.bulkDeals = bulkDeals;
        this.loading = false;

        this.bulkDeals.forEach((bulkDeal) => (bulkDeal.dealDate = new Date(<Date>bulkDeal.dealDate)));
      });
  }

  getAllBulkDeals() {
    return this.bulkDealService.fetchBulkDeals();
    // return Promise.resolve([
    //   {
    //     clientName: 'Jainam',
    //     dealDate: new Date(),
    //     companySymbol: 'ACC',
    //     companyFullName: 'ACC',
    //     stockAction: StockAction.Buy,
    //     quantity: 100,
    //     tradePrice: 10,
    //     remarks: null
    //   },
    //   {
    //     clientName: 'Parth',
    //     dealDate: new Date(),
    //     companySymbol: '5Paisa',
    //     companyFullName: '5Paisa',
    //     stockAction: StockAction.Buy,
    //     quantity: 1000,
    //     tradePrice: 20,
    //     remarks: null
    //   },
    //   {
    //     clientName: 'Jainam',
    //     dealDate: new Date(),
    //     companySymbol: 'ACC',
    //     companyFullName: 'ACC',
    //     stockAction: StockAction.Buy,
    //     quantity: 100,
    //     tradePrice: 10,
    //     remarks: null
    //   },
    //   {
    //     clientName: 'Jainam',
    //     dealDate: new Date(),
    //     companySymbol: 'ACC',
    //     companyFullName: 'ACC',
    //     stockAction: StockAction.Buy,
    //     quantity: 100,
    //     tradePrice: 10,
    //     remarks: null
    //   },
    //   {
    //     clientName: 'Jainam',
    //     dealDate: new Date(),
    //     companySymbol: 'ACC',
    //     companyFullName: 'ACC',
    //     stockAction: StockAction.Buy,
    //     quantity: 100,
    //     tradePrice: 10,
    //     remarks: null
    //   }
    // ]);
  }
}
