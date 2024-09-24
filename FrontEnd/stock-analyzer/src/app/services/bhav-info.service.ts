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
    return this.http.post<string>(DomainConstants.StockAnalyzer_URL + 'analyze-bhav-data?date=' + dateToProcess.toISOString(), null);
  }

  fetchBhavInfos() {
    return this.http.get<BhavInfo[]>(DomainConstants.StockAnalyzer_URL + 'bhav-infos-of-all-companies/');
  }

  fetchBhavInfosBetween(startDate: Date, endDate: Date) {//2023-11-06T08:00:19Z
    const url = DomainConstants.StockAnalyzer_URL + 'bhav-infos-between';
    // Format dates correctly
    const startFormatted = moment(startDate).endOf('day').toISOString();
    const endFormatted = moment(endDate).endOf('day').toISOString();

    const params = {
      startDate: startFormatted.split('T')[0], // YYYY-MM-DD
      endDate: endFormatted.split('T')[0]      // YYYY-MM-DD
    };
    return this.http.get<BhavInfo[]>(url, { params });
  }
}
