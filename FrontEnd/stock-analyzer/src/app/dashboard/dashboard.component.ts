import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormArray, FormBuilder, Validators } from '@angular/forms';
import { FilterService } from '../services/filter.service';
import { ChangeType, Filter, FilterCriteria, FilterResult, PeriodType } from '../types/Filter';
import { Table } from 'primeng/table';
import { BulkDeal, StockAction } from '../types/BulkDeal';
import { SortEvent } from 'primeng/api';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css', '../shared/css/common-page-style.css']
})
export class DashboardComponent implements OnInit {
  @ViewChild('filterTable', { static: true }) filterTable!: Table;
  @ViewChild('bulkDealTable', { static: true }) bulkDealTable!: Table;
  bulkDealFilterInput: string = '';

  selectedOption: string = '';
  filterDate: string = new Date().toISOString().split('T')[0];

  selectedFilter!: Filter;
  filterOptions: Filter[] = [];
  filterResults: FilterResult[] = [];
  bulkDeals: BulkDeal[] = [];
  loadingFilterResults: boolean = true;

  modalOpen: boolean = false;

  constructor(private filterService: FilterService) {
  }

  onFilterFormChange() {
    this.updateSelectedFilter();

    this.loadingFilterResults = true;
    // Handle the selected option change here
    this.filterService.executeFilter(this.selectedOption, new Date(this.filterDate))
      .subscribe({
        next: (value) => {
          this.filterResults = value;
          this.loadingFilterResults = false;
          this.bulkDeals = this.filterResults
            .filter(_ => _.company.bulkDeals.length > 0)
            .map(_ => _.company.bulkDeals[0]);

          // console.log('filter values = ' + value);
          // console.dir(value, { depth: null });
        },
        error: (err) => console.log(err),
      });
  }

  updateSelectedFilter() {
    this.selectedFilter = this.filterOptions
      .filter(_ => _.filterName == this.selectedOption)[0];
  }

  clearFilterTable(table: Table) {
    table.clear();
    (<HTMLInputElement>document.getElementById('filter-search-value')).value = '';
  }

  clearBulkDealTable(table: Table) {
    table.clear();
    (<HTMLInputElement>document.getElementById('bulk-search-value')).value = '';
  }

  openModal() {
    this.modalOpen = true;
  }

  closeModal() {
    this.modalOpen = false;
  }

  ngOnInit() {
    this.filterService.getAllFilterNames()
      .subscribe({
        next: (value) => {
          this.filterOptions = value;
          //console.log('value = ' + value);
          console.dir(value, { depth: null });
          this.selectedOption = this.filterOptions[0]?.filterName ?? '';
          this.updateSelectedFilter();
          this.onFilterFormChange();
        },
        error: (err) => console.log(err),
      });

    // console.log("this.filterOptions = " + this.filterOptions);
  }

  getFieldName(filterCriteria: FilterCriteria) {
    //Avg fieldName(periodValue'D/W/M/Y') ^;
    // console.log("type=", filterCriteria.changeType == ChangeType.Increase);
    return `AVG ${filterCriteria.fieldName}
    (${filterCriteria.periodValue}${this.getPeriodType(filterCriteria.periodType)}${this.getChangeTypeSymbol(filterCriteria.changeType)})`
  }

  getPeriodType(type: string): string {
    if (type == PeriodType.Days) {
      return 'D';
    } else if (type == PeriodType.Weeks) {
      return 'W';
    } else if (type == PeriodType.Months) {
      return 'M';
    } else if (type == PeriodType.Years) {
      return 'Y';
    }
    return 'Y';
  }

  getChangeTypeSymbol(type: string) {
    if (type == ChangeType.Increase) {
      return '↑';
    }
    return '↓';
  }

  getStockAction(value: number) {
    return StockAction[value];
  }

  filterBulkDeal(companyName: string) {
    console.dir(this.bulkDealTable, { depth: null });
    this.bulkDealTable.filterGlobal(companyName, 'contains');
    this.bulkDealFilterInput = companyName;
  }

  customSort(event: SortEvent) {
    event.data!.sort((data1, data2) => {
      let value1 = null;
      let value2 = null;

      console.dir(event, { depth: null });
      if (event.field!.split('.').length > 2) {
        let fields = event.field!.split('.');
        value1 = data1.company.bhavCopyInfos[0][fields[2]];
        value2 = data2.company.bhavCopyInfos[0][fields[2]];
      } else if (event.field!.split('.').length > 1) {
        let fields = event.field!.split('.');
        value1 = data1.company[fields[1]];
        value2 = data2.company[fields[1]];
      } else {
        value1 = data1[event.field!];
        value2 = data2[event.field!];
      }
      let result = null;
      // console.log('value1=', value1 + ' - value2=' + value2);
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

  displayFilterCriteria(criterias: FilterCriteria[]): string {
    criterias = criterias.sort(_ => _.sequence);

    let criteriaResult = '';
    criterias.forEach((criteria, index) => {

      criteriaResult += `${criteria.fieldName} (${criteria.periodValue} ${this.getPeriodType(criteria.periodType)}) ${criteria.changeType}`;

      if (index < criterias.length - 1)
        criteriaResult += `\n${criteria.logicalOperator} `;
    });
    return criteriaResult;
  }
}