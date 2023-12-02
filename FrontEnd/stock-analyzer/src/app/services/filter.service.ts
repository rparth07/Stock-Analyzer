import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DomainConstants } from '../shared/domain.constants';
import { Company } from '../types/Company';
import { Observable } from 'rxjs';
import { Filter, FilterResult } from '../types/Filter';
import { BulkDeal } from '../types/BulkDeal';

@Injectable({
  providedIn: 'root'
})
export class FilterService {

  private selectedOption: string;
  private filterDate: string;
  private filterResults: FilterResult[];
  private bulkDeals: BulkDeal[];

  constructor(private http: HttpClient) {
    this.selectedOption = '';
    this.filterDate = '';
    this.filterResults = [];
    this.bulkDeals = [];
  }

  setSelectedOption(option: string) {
    this.selectedOption = option;
    console.log('----> IN SERVICE => selectedOption = ' + this.selectedOption);
  }

  getSelectedOption(): string {
    return this.selectedOption;
  }

  setFilterDate(filterDate: string) {
    this.filterDate = filterDate;
    console.log('----> IN SERVICE => filterDate = ' + this.filterDate);
  }

  getFilterDate(): string {
    return this.filterDate;
  }

  setFilterResults(results: FilterResult[]) {
    this.filterResults = results;
    console.log('----> IN SERVICE => filterResults = ' + this.filterResults);
  }

  getFilterResults(): FilterResult[] {
    return this.filterResults;
  }

  setBulkDeals(bulkDeals: BulkDeal[]) {
    this.bulkDeals = bulkDeals;
    console.log('----> IN SERVICE => bulkDeals = ' + this.bulkDeals);
  }

  getBulkDeals(): BulkDeal[] {
    return this.bulkDeals;
  }

  executeFilter(filterName: string, filterDate: Date): Observable<FilterResult[]> {
    const url = `${DomainConstants.Filter_URL}get-filter-result`;
    const params = {
      filterName: filterName,
      filterDate: filterDate.toISOString()
    };
    return this.http.get<FilterResult[]>(url, { params });
  }

  addFilter(filter: Filter) {
    console.dir(filter, { depth: null });
    const url = `${DomainConstants.Filter_URL}add-filter`;
    return this.http.post<Filter>(url, filter);
  }

  getAllFilterNames(): Observable<Filter[]> {
    const url = `${DomainConstants.Filter_URL}get-filters`;
    return this.http.get<Filter[]>(url);
  }

  deleteFilter(filter: string) {
    const url = `${DomainConstants.Filter_URL}delete-filter/${filter}`;
    return this.http.delete(url);
  }
}
