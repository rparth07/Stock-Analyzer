import { ApiStatusService } from '../shared/api-status.service';
import { Subscription } from 'rxjs';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
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
export class DashboardComponent implements OnInit, OnDestroy {
  @ViewChild('filterTable', { static: true }) filterTable!: Table;
  @ViewChild('bulkDealTable', { static: true }) bulkDealTable!: Table;
  bulkDealFilterInput: string = '';

  selectedOption: string = '';
  filterDate: string = this.getDateOfToday();

  selectedFilter: Filter | undefined;
  filterOptions: Filter[] = [];
  filterResults: FilterResult[] = [];
  bulkDeals: BulkDeal[] = [];
  loadingFilterResults: boolean = false;

  filtersLoading = true;
  filtersError = false;
  resultError = false;
  private resultRequest?: Subscription;
  private dateRequest?: Subscription;
  private usingLatestDate = true;

  modalOpen: boolean = false;
  deleteModalOpen: boolean = false;

  constructor(private filterService: FilterService, private apiStatus: ApiStatusService) {
  }

  ngOnDestroy(): void {
    this.saveFilterDetailsInService();
    this.resultRequest?.unsubscribe();
    this.dateRequest?.unsubscribe();
  }

  saveFilterDetailsInService() {
    this.filterService.setSelectedOption(this.selectedOption);
    this.filterService.setFilterDate(this.filterDate);
    this.filterService.setFilterResults(this.filterResults);
    this.filterService.setBulkDeals(this.bulkDeals);
  }

  getDateOfToday(): string {
    return new Intl.DateTimeFormat('en-CA', { timeZone: 'Asia/Kolkata', year: 'numeric', month: '2-digit', day: '2-digit' }).format(new Date());
  }

  onFilterFormChange() {
    this.resultRequest?.unsubscribe();
    this.resultError = false;
    this.updateSelectedFilter();

    if (!this.selectedOption || !this.filterDate || !Number.isFinite(Date.parse(this.filterDate))) {
      this.filterResults = []; this.bulkDeals = []; this.loadingFilterResults = false; return;
    }
    this.loadingFilterResults = true;
    this.filterResults = []; this.bulkDeals = [];
    // Handle the selected option change here
    this.resultRequest = this.filterService.executeFilter(this.selectedOption, new Date(this.filterDate))
      .subscribe({
        next: (value) => {
          this.filterResults = value;
          this.loadingFilterResults = false;
          this.bulkDeals = this.filterResults.flatMap(result => result.company?.bulkDeals ?? []);

          // console.log('filter values = ' + value);
          // console.dir(value, { depth: null });
        },
        error: () => { this.loadingFilterResults = false; this.resultError = true; },
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

  openDeleteModal() {
    this.deleteModalOpen = true;
  }

  closeDeleteModal() {
    this.deleteModalOpen = false;
  }

  getAllFilterNames() {
    this.filtersLoading = true; this.filtersError = false;
    this.filterService.getAllFilterNames()
      .subscribe({
        next: (filters: Filter[]) => {
          this.filtersLoading = false;
          this.filterOptions = filters;
          //console.log('filters = ' + filters);
          // console.dir(filters, { depth: null });

          this.selectedOption = filters.some(filter => filter.filterName === this.filterService.getSelectedOption())
            ? this.filterService.getSelectedOption() :
            this.filterOptions[0]?.filterName ?? '';
          // console.log('get al filter names= ' + this.selectedOption);
          this.filterService.setSelectedOption(this.selectedOption);
          // console.log('after setting  = ' + this.filterService.getSelectedOption());

          this.getSavedFilterDetailsFromService();
          this.updateSelectedFilter();
          if (this.selectedOption) {
            this.onFilterFormChange();
          }
        },
        error: () => { this.filtersLoading = false; this.filtersError = true; },
      });
  }

  reloadPage() {
    // console.log('page reloaded!');
    // this.getAllFilterNames();
    this.getAllFilterNames();
  }

  onDateChanged() { this.usingLatestDate = false; this.onFilterFormChange(); }
  ngOnInit() {
    this.dateRequest = this.apiStatus.marketDate$.subscribe(date => {
      if (date && this.usingLatestDate && !this.filterService.getFilterDate()) {
        this.filterDate = date;
        if (this.filterOptions.length) this.onFilterFormChange();
      }
    });
    this.getAllFilterNames();
    // console.log('ngoninit = ' + this.selectedOption);
  }

  getSavedFilterDetailsFromService() {
    this.selectedOption = this.filterService.getSelectedOption() ?? '';
    this.filterDate = this.filterService.getFilterDate() != '' ? this.filterService.getFilterDate() : (this.apiStatus.marketDate$.value || this.getDateOfToday());
    this.filterResults = this.filterService.getFilterResults() ?? [];
    this.bulkDeals = this.filterService.getBulkDeals() ?? [];
  }

  onFilterDialogClosed(saved: boolean) { this.modalOpen = false; if (saved) this.getAllFilterNames(); }
  onDeleteDialogClosed(deleted: boolean) { this.deleteModalOpen = false; if (deleted) { this.filterService.setSelectedOption(''); this.getAllFilterNames(); } }
  fieldLabel(field: string): string {
    const labels: Record<string, string> = { ClosePrice: 'Close price', OpenPrice: 'Open price', HighPrice: 'High price', LowPrice: 'Low price', PreviousClose: 'Previous close', LastPrice: 'Last price', AvgPrice: 'Average price', TtlTrdQnty: 'Traded volume', TurnOverLacs: 'Turnover (lakh INR)', NoOfTrades: 'Trade count', DeliveryQty: 'Deliverable quantity', DeliveryPercentage: 'Delivery percentage' };
    return labels[field] ?? field;
  }

  getFieldName(filterCriteria: FilterCriteria) {
    //Avg fieldName(periodValue'D/W/M/Y') ^;
    // console.log("type=", filterCriteria.changeType == ChangeType.Increase);
    return `Avg ${this.fieldLabel(filterCriteria.fieldName)}
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
    return type == ChangeType.Increase ? '\u2191' : '\u2193';
  }

  getStockAction(value: number) {
    return StockAction[value];
  }

  filterBulkDeal(companyName: string) {
    // console.dir(this.bulkDealTable, { depth: null });
    this.bulkDealTable.filterGlobal(companyName, 'contains');
    this.bulkDealFilterInput = companyName;
  }

  customSort(event: SortEvent) {
    event.data!.sort((data1, data2) => {
      let value1 = null;
      let value2 = null;

      // console.dir(event, { depth: null });
      if (event.field === 'filterCriteria.fieldName') {
        value1 = data1.filterCriteria?.fieldName; value2 = data2.filterCriteria?.fieldName;
      } else if (event.field!.split('.').length > 2) {
        let fields = event.field!.split('.');
        value1 = data1.company?.bhavCopyInfos?.[0]?.[fields[2]];
        value2 = data2.company?.bhavCopyInfos?.[0]?.[fields[2]];
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

  displayFilterCriteria(criterias: FilterCriteria[] | undefined | null): string {
    let criteriaResult = '';
    if (criterias == null) {
      return criteriaResult;
    }

    criterias = [...criterias].sort((a, b) => a.sequence - b.sequence);

    criterias.forEach((criteria, index) => {

      criteriaResult += `${this.fieldLabel(criteria.fieldName)} (${criteria.periodValue} ${this.getPeriodType(criteria.periodType)}) ${criteria.changeType}`;

      if (index < criterias!.length - 1)
        criteriaResult += `\n${criteria.logicalOperator} `;
    });
    return criteriaResult;
  }
}