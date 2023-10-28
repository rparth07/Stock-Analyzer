import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DomainConstants } from '../shared/domain.constants';
import { Company } from '../types/Company';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  constructor(private http: HttpClient) { }

  fetchAllCompanies() {
    return this.http.get<Company[]>(DomainConstants.StockAnalyzer_URL + 'all-info-of-companies/');
  }

  fetchCompany(company: string | null) {
    // if (company === null) return;
    return this.http.get<Company>(DomainConstants.StockAnalyzer_URL + 'company-detail?symbol=' + company);
  }
}
