import { Component, OnInit } from '@angular/core';
import { FormGroup, FormArray, FormBuilder, Validators } from '@angular/forms';
import { FilterService } from '../services/filter.service';
import { ChangeType, Filter, FilterCriteria, FilterResult, PeriodType } from '../types/Filter';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css', '../shared/css/common-page-style.css']
})
export class DashboardComponent implements OnInit {
  selectedOption: string = '';
  filterDate: string = new Date().toISOString().split('T')[0];

  filterOptions: Filter[] = [];
  filterResults: FilterResult[] = [];
  loadingFilterResults: boolean = true;

  filterGroup: FormGroup;
  criterias!: FormArray;

  modalOpen: boolean = false;
  sequence: number = 1;

  onOptionChange() {
    // Handle the selected option change here
    this.filterService.executeFilter(this.selectedOption, new Date(this.filterDate))
      .subscribe({
        next: (value) => {
          this.filterResults = value;
          this.loadingFilterResults = false;
          console.log('filter values = ' + value);
          console.dir(value, { depth: null });
        },
        error: (err) => console.log(err),
      });
  }

  clear(table: Table) {
    table.clear();
    (<HTMLInputElement>document.getElementById('search-value')).value = '';
  }

  openModal() {
    this.modalOpen = true;
  }

  closeModal() {
    this.modalOpen = false;
  }

  constructor(private formBuilder: FormBuilder, private filterService: FilterService) {
    this.filterGroup = this.formBuilder.group({
      FilterName: ['', Validators.required],
      Series: ['EQ', Validators.required],
      criterias: this.formBuilder.array([this.createInputRow()])
    });
  }

  ngOnInit() {
    this.criterias = this.filterGroup.get('criterias') as FormArray;
    this.sequence = 1;

    this.filterService.getAllFilterNames()
      .subscribe({
        next: (value) => {
          this.filterOptions = value;
          //console.log('value = ' + value);
          console.dir(value, { depth: null });
          this.selectedOption = this.filterOptions[0].filterName;
          this.onOptionChange();
        },
        error: (err) => console.log(err),
      });

    console.log("this.filterOptions = " + this.filterOptions);
  }

  addRow() {
    this.sequence++;
    this.criterias.push(this.createInputRow());
  }

  removeRow(index: number) {
    this.sequence--;
    this.criterias.removeAt(index);
  }

  createInputRow(): FormGroup {
    return this.formBuilder.group({
      sequence: this.sequence,
      fieldName: ['ClosePrice', Validators.required],
      periodValue: [1, Validators.required],
      periodType: ['Days', Validators.required],
      changeType: ['Increase', Validators.required],
      logicalOperator: ['And', Validators.required]
    });
  }

  submitForm() {
    if (this.filterGroup.valid) {
      console.log('Form submitted:', this.filterGroup.value);
      this.filterService
        .addFilter(this.filterGroup.value as Filter);
      this.closeModal();
      this.filterGroup.reset();
    }
  }

  getFieldName(filterCriteria: FilterCriteria) {
    //Avg fieldName(periodValue'D/W/M/Y') ^;
    return `AVG ${filterCriteria.fieldName}
    (${filterCriteria.periodValue}${this.getPeriodType(filterCriteria.PeriodType.toString())}${this.getChangeTypeSymbol(filterCriteria.changeType)})`
  }

  getPeriodType(type: string): string {
    if (type == PeriodType.Days.toString()) {
      return 'D';
    } else if (type == PeriodType.Weeks.toString()) {
      return 'W';
    } else if (type == PeriodType.Months.toString()) {
      return 'M';
    } else if (type == PeriodType.Years.toString()) {
      return 'Y';
    }
    return 'Y';
  }

  getChangeTypeSymbol(type: ChangeType) {
    if (type === ChangeType.Increase) {
      return '↑';
    }
    return '↓';
  }
}
