import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DomainConstants } from '../shared/domain.constants';
import { Company } from '../types/Company';
import { Observable } from 'rxjs';
import { Filter } from '../types/Filter';

@Injectable({
  providedIn: 'root'
})
export class FilterService {

  constructor(private http: HttpClient) { }

  filterCompaniesBy(filterName: string): Observable<Company[]> {
    const url = `${DomainConstants.Filter_URL}${filterName}`;
    return this.http.get<Company[]>(url);
  }

  // addNewFilter(filter: Filter): void {
  //   console.dir(filter, { depth: null });
  //   const url = DomainConstants.Filter_URL + 'add-filter';
  //   this.http.post(url, filter);
  // }

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
    const url = DomainConstants.Filter_URL + 'get-filters';
    return this.http.get<Filter[]>(url);
  }
}
