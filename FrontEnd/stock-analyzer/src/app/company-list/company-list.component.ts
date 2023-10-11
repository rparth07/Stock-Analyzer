import { Component, OnInit } from '@angular/core';
import { Form } from '@angular/forms';
import { Company } from '../types/Company';
import { CompanyService } from '../services/company.service';
import { SortEvent } from 'primeng/api';
import { RouterModule } from '@angular/router';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-company',
  templateUrl: './company-list.component.html',
  styleUrls: ['./company-list.component.css', '../shared/css/common-page-style.css']
})
export class CompanyListComponent implements OnInit {
  companies!: Company[];
  selectedCompanies!: Company[];
  loading: boolean = true;

  constructor(private companyService: CompanyService) { }

  clear(table: Table) {
    table.clear();
    (<HTMLInputElement>document.getElementById('search-value')).value = '';
  }

  customSort(event: SortEvent) {
    event.data!.sort((data1, data2) => {
      let value1 = null;
      let value2 = null;
      if (event.field!.startsWith('bhavCopyInfos')) {
        let field = event.field!.split('.')[1];
        value1 = data1.bhavCopyInfos.length !== 0 ? data1.bhavCopyInfos[0][field] : null;
        value2 = data2.bhavCopyInfos.length !== 0 ? data2.bhavCopyInfos[0][field] : null;
      } else {
        value1 = data1[event.field!];
        value2 = data2[event.field!];
      }
      let result = null;

      if (value1 == null && value2 != null)
        result = -1;
      else if (value1 != null && value2 == null)
        result = 1;
      else if (value1 == null && value2 == null)
        result = 0;
      else if (typeof value1 === 'string' && typeof value2 === 'string')
        result = value1.localeCompare(value2);
      else {
        // Compare the values directly
        result = (value1 < value2) ? -1 : (value1 > value2) ? 1 : 0;
      }

      return (event.order! * result);
    });
  }

  ngOnInit() {
    this.getAllCompanies()
      .subscribe((companies: Company[]) => {
        this.companies = companies;
        console.log(companies);
        this.loading = false;
      });
  }

  getAllCompanies() {
    return this.companyService.fetchAllCompanies();
    // return Promise.resolve([
    //   {
    //     companySymbol: 'Jainam',
    //     companyFullName: 'Jainam full name',
    //     bhavInfos: [],
    //     bulkDeals: []
    //   },
    //   {
    //     companySymbol: 'Parth',
    //     companyFullName: 'Parth full name',
    //     bhavInfos: [],
    //     bulkDeals: []
    //   },
    //   {
    //     companySymbol: 'Jainam',
    //     companyFullName: 'Jainam full name',
    //     bhavInfos: [],
    //     bulkDeals: []
    //   },
    //   {
    //     companySymbol: 'Jainam',
    //     companyFullName: 'Jainam full name',
    //     bhavInfos: [],
    //     bulkDeals: []
    //   },
    //   {
    //     companySymbol: 'Jainam',
    //     companyFullName: 'Jainam full name',
    //     bhavInfos: [],
    //     bulkDeals: []
    //   }
    // ]);
  }

  getCompanyQueryParams(company: Company): { [key: string]: string } {
    return {
      symbol: company.symbol
    };
  }
}
