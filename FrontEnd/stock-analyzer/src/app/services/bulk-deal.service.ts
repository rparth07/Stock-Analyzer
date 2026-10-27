import * as moment from 'moment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DomainConstants } from '../shared/domain.constants';
import { BulkDeal } from '../types/BulkDeal';

@Injectable({
  providedIn: 'root'
})
export class BulkDealService {
  constructor(private http: HttpClient) { }

  analyzeBulkDeals(dateToProcess: Date) {
    //2018-06-22T08:00:19Z
    return this.http.post<string>(DomainConstants.StockAnalyzer_URL + 'analyze-bulk-deals?date=' + moment(dateToProcess).format('YYYY-MM-DD'), null);
  }

  fetchBulkDeals() {
    return this.http.get<BulkDeal[]>(DomainConstants.StockAnalyzer_URL + 'all-bulk-deals/');
  }

  fetchBulkDealsBetween(startDate: Date, endDate: Date) {
    const url = DomainConstants.StockAnalyzer_URL + 'bulk-deals-between';
    const params = {
      startDate: moment(startDate).format('YYYY-MM-DD'),
      endDate: moment(endDate).format('YYYY-MM-DD')
    };
    return this.http.get<BulkDeal[]>(url, { params });
  }
}
