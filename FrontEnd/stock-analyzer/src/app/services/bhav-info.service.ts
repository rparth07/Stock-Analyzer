import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DomainConstants } from '../shared/domain.constants';
import { BhavInfo } from '../types/BhavInfo';

@Injectable({
  providedIn: 'root'
})
export class BhavInfoService {

  constructor(private http: HttpClient) { }

  analyzeBhavInfos(dateToProcess: Date) {
    //2018-06-22T08:00:19Z
    return this.http.post<string>(DomainConstants.StockAnalyzer_URL + 'analyze-bhav-data?date=' + dateToProcess.toISOString(), null);
  }

  fetchBhavInfos() {
    return this.http.get<BhavInfo[]>(DomainConstants.StockAnalyzer_URL + 'bhav-infos-of-all-companies/');
  }

  fetchBhavInfosBetween(startDate: Date, endDate: Date) {
    const url = DomainConstants.StockAnalyzer_URL + 'bhav-infos-between';
    const params = {
      startDate: startDate.toISOString(),
      endDate: endDate.toISOString()
    };
    return this.http.get<BhavInfo[]>(url, { params });
  }
}
