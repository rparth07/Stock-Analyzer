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
    let response;
    this.http.post<string>(DomainConstants.StockAnalyzer_URL + 'analyze-bulk-deals?date=' + dateToProcess.toISOString(), null)
      .subscribe(res => response = res);
    return response;
  }

  fetchBulkDeals() {
    return this.http.get<BulkDeal[]>(DomainConstants.StockAnalyzer_URL + 'all-bulk-deals/');
  }
}
