import { Component, OnInit } from '@angular/core';
import { Form, NgForm } from '@angular/forms';
import { BhavInfo } from '../types/BhavInfo';
import { BhavInfoService } from '../services/bhav-info.service';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-bhav-info',
  templateUrl: './bhav-info.component.html',
  styleUrls: ['./bhav-info.component.css',
    '../shared/css/common-page-style.css']
})
export class BhavInfoComponent implements OnInit {

  date: Date = new Date();

  analyzeBhavInfosOfDate() {
    this.bhavInfoService.analyzeBhavInfos(this.date);
  }

  bhavInfos!: BhavInfo[];
  selectedBhavInfos!: BhavInfo[];
  loading: boolean = true;

  constructor(private bhavInfoService: BhavInfoService) { }

  ngOnInit() {
    this.getAllBhavInfos()
      .subscribe((response: BhavInfo[]) => {
        this.bhavInfos = response;
        this.loading = false;
        // console.log("this is the data", JSON.stringify(this.bhavInfos));
        this.bhavInfos.forEach((bhavInfo) => (bhavInfo.date = new Date(<Date>bhavInfo.date)));
      });
  }

  clear(table: Table) {
    table.clear();
    (<HTMLInputElement>document.getElementById('search-value')).value = '';
  }

  getAllBhavInfos() {
    return this.bhavInfoService.fetchBhavInfos();
    // return Promise.resolve([
    //   {
    //     companySymbol: 'ACC',
    //     series: 'ES',
    //     date: new Date(),
    //     previousClose: 100,
    //     openPrice: 50,
    //     highPrice: 500,
    //     lowPrice: 10,
    //     lastPrice: 50,
    //     closePrice: 105,
    //     avgPrice: 120,
    //     ttlTrdQnty: 150,
    //     turnOverLacs: 50000,
    //     noOfTrades: 50,
    //     deliveryQty: 56,
    //     deliveryPercentage: 50
    //   },
    //   {
    //     companySymbol: '5Paisa',
    //     series: 'EN',
    //     date: new Date(),
    //     previousClose: 140,
    //     openPrice: 40,
    //     highPrice: 200,
    //     lowPrice: 104,
    //     lastPrice: 60,
    //     closePrice: 705,
    //     avgPrice: 140,
    //     ttlTrdQnty: 50,
    //     turnOverLacs: 530000,
    //     noOfTrades: 520,
    //     deliveryQty: 526,
    //     deliveryPercentage: 520
    //   },
    //   {
    //     companySymbol: 'Jainam',
    //     series: 'EQ',
    //     date: new Date(),
    //     previousClose: 234,
    //     openPrice: 230,
    //     highPrice: 250,
    //     lowPrice: 112,
    //     lastPrice: 520,
    //     closePrice: 1035,
    //     avgPrice: 130,
    //     ttlTrdQnty: 130,
    //     turnOverLacs: 500030,
    //     noOfTrades: 332,
    //     deliveryQty: 51,
    //     deliveryPercentage: 20
    //   },
    //   {
    //     companySymbol: 'Jainam',
    //     series: 'EQ',
    //     date: new Date(),
    //     previousClose: 234,
    //     openPrice: 230,
    //     highPrice: 250,
    //     lowPrice: 112,
    //     lastPrice: 520,
    //     closePrice: 1035,
    //     avgPrice: 130,
    //     ttlTrdQnty: 130,
    //     turnOverLacs: 500030,
    //     noOfTrades: 332,
    //     deliveryQty: 51,
    //     deliveryPercentage: 20
    //   },
    //   {
    //     companySymbol: 'Jainam',
    //     series: 'EQ',
    //     date: new Date(),
    //     previousClose: 234,
    //     openPrice: 230,
    //     highPrice: 250,
    //     lowPrice: 112,
    //     lastPrice: 520,
    //     closePrice: 1035,
    //     avgPrice: 130,
    //     ttlTrdQnty: 130,
    //     turnOverLacs: 500030,
    //     noOfTrades: 332,
    //     deliveryQty: 51,
    //     deliveryPercentage: 20
    //   }
    // ]);
  }
}
