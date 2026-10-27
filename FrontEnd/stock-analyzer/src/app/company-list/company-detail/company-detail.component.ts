import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Table } from 'primeng/table';
import { CompanyService } from 'src/app/services/company.service';
import { BhavInfo } from 'src/app/types/BhavInfo';
import { BulkDeal, StockAction } from 'src/app/types/BulkDeal';
import { Company } from 'src/app/types/Company';

@Component({
  selector: 'app-company-detail',
  templateUrl: './company-detail.component.html',
  styleUrls: ['./company-detail.component.css', '../../shared/css/common-page-style.css']
})
export class CompanyDetailComponent implements OnInit {
  company: Company | null = null;
  errorMessage = '';

  bhavInfos: BhavInfo[] = [];
  selectedBhavInfos!: BhavInfo[];

  bulkDeals: BulkDeal[] = [];
  selectedBulkDeals!: BulkDeal[];

  loading: boolean = true;
  showBhavInfo: Boolean = true;

  constructor(private route: ActivatedRoute, private companyService: CompanyService) { }

  ngOnInit(): void {
    this.getCompanyDetails();
  }

  clear(table: Table) {
    table.clear();
    (<HTMLInputElement>document.getElementById('search-value')).value = '';
  }

  getCompanyDetails(): void {
    let companyName = this.route.snapshot.queryParams['symbol'];
    if (!companyName || typeof companyName !== 'string' || !companyName.trim()) {
      this.errorMessage = 'Choose a company from the company list.'; this.loading = false; return;
    }
    this.loading = true; this.errorMessage = '';
    this.companyService.fetchCompany(companyName)
      .subscribe({ next: company => {
        if (!company) { this.errorMessage = 'No company data was returned.'; this.loading = false; return; }
        this.company = company;
        this.bhavInfos = this.company.bhavCopyInfos ?? [];
        this.bulkDeals = this.company.bulkDeals ?? [];
        this.showBhavInfo = true;
        this.loading = false;
      }, error: () => { this.loading = false; this.errorMessage = 'Company data is unavailable. Try again or choose another symbol.'; } });
  }

  showBhavInfos() {
    this.showBhavInfo = true;
  }

  showBulkDeals() {
    this.showBhavInfo = false;
  }

  getStockAction(value: number) {
    return StockAction[value];
  }
}
