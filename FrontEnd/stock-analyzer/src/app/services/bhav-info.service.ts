import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DomainConstants } from '../shared/domain.constants';
import { BhavInfo } from '../types/BhavInfo';
import * as moment from 'moment';

@Injectable({
  providedIn: 'root'
})
export class BhavInfoService {

  constructor(private http: HttpClient) { }

  analyzeBhavInfos(dateToProcess: Date) {
    //2018-06-22T08:00:19Z
    return this.http.post<string>(DomainConstants.StockAnalyzer_URL.replace('StockAnalyzer/', 'market-data/') + 'import-bhav?date=' + moment(dateToProcess).format('YYYY-MM-DD'), null);
  }

  fetchBhavInfos() {
    return this.http.get<BhavInfo[]>(DomainConstants.StockAnalyzer_URL + 'bhav-infos-of-all-companies/');
  }

  fetchBhavInfosBetween(startDate: Date, endDate: Date) {//2023-11-06T08:00:19Z
    const url = DomainConstants.StockAnalyzer_URL + 'bhav-infos-between';
    // Format dates correctly
    const startFormatted = moment(startDate).format('YYYY-MM-DD');
    const endFormatted = moment(endDate).format('YYYY-MM-DD');

    const params = {
      startDate: startFormatted, // YYYY-MM-DD
      endDate: endFormatted      // YYYY-MM-DD
    };
    return this.http.get<BhavInfo[]>(url, { params });
  }
}
