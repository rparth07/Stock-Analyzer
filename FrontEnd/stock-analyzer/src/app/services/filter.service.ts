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
    const url = `${DomainConstants.Filter_URL}execute-filter`;
    const params = {
      filterName: filterName,
      filterDate: filterDate.toISOString()
    };
    return this.http.get<FilterResult[]>(url, { params });
  }

  addFilter(filter: Filter): void {
    console.dir(filter, { depth: null });
    const url = `${DomainConstants.Filter_URL}add-filter`;
    this.http.post<Filter>(url, filter)
      .subscribe({
        next: (value) => console.log(value),
        error: (err) => console.log(err),
      });
  }

  getAllFilterNames(): Observable<Filter[]> {
    const url = `${DomainConstants.Filter_URL}get-filters`;
    return this.http.get<Filter[]>(url);
  }
}
