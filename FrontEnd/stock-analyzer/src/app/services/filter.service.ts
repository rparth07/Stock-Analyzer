import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DomainConstants } from '../shared/domain.constants';
import { Company } from '../types/Company';
import { Observable } from 'rxjs';
import { Filter, FilterResult } from '../types/Filter';

@Injectable({
  providedIn: 'root'
})
export class FilterService {

  constructor(private http: HttpClient) { }

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
