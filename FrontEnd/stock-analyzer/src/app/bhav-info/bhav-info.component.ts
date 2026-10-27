import { Component, OnInit } from '@angular/core';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE, MatDateFormats } from '@angular/material/core';
import { BhavInfo } from '../types/BhavInfo';
import { BhavInfoService } from '../services/bhav-info.service';
import { Table } from 'primeng/table';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { MY_DATE_FORMATS } from '../shared/domain.constants';

@Component({
  selector: 'app-bhav-info',
  templateUrl: './bhav-info.component.html',
  styleUrls: ['./bhav-info.component.css',
    '../shared/css/common-page-style.css'],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' }, // choose your locale
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
},)
export class BhavInfoComponent implements OnInit {

  analyzeBhavInfodate: Date = new Date();
  endDate: Date = new Date();
  startDate: Date = new Date(this.endDate.getTime() - 7 * 24 * 60 * 60 * 1000);

  analyzeBhavInfosOfDate() {
    this.bhavInfoService.analyzeBhavInfos(this.analyzeBhavInfodate)
      .subscribe({ next: res => {
        this.reloadPage();
      }, error: () => {} });
  }

  reloadPage() {
    this.endDate = new Date();
    this.startDate = new Date(this.endDate.getTime() - 7 * 24 * 60 * 60 * 1000);
    window.location.reload();
  }

  bhavInfos: BhavInfo[] = [];
  selectedBhavInfos!: BhavInfo[];
  loading: boolean = true;
  loadError = false;
  dateError = '';

  constructor(private bhavInfoService: BhavInfoService) { }

  ngOnInit() {
    this.endDate = new Date();
    this.startDate = new Date(this.endDate.getTime() - 7 * 24 * 60 * 60 * 1000);
    this.getBhavInfosBetween(this.startDate, this.endDate);
  }

  clear(table: Table) {
    table.clear();
    (<HTMLInputElement>document.getElementById('search-value')).value = '';
  }

  getAllBhavInfos() {
    return this.bhavInfoService.fetchBhavInfos();
  }

  getBhavInfosBetween(startDate: Date, endDate: Date) {
    if (!startDate || !endDate || !Number.isFinite(new Date(startDate).getTime()) || !Number.isFinite(new Date(endDate).getTime()) || new Date(startDate) > new Date(endDate)) {
      this.dateError = 'Choose a valid date range with the start date on or before the end date.';
      this.loading = false; return;
    }
    this.dateError = ''; this.loading = true; this.loadError = false;
    this.bhavInfoService.fetchBhavInfosBetween(startDate, endDate)
      .subscribe({ next: (response: BhavInfo[]) => {
        this.bhavInfos = response;
        this.loading = false;
        // console.log("this is the data", JSON.stringify(this.bhavInfos));
        this.bhavInfos.forEach((bhavInfo) => (bhavInfo.date = new Date(<Date>bhavInfo.date)));
      }, error: () => { this.loading = false; this.loadError = true; } });
  }
}
