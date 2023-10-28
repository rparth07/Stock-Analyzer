import { Component, OnInit } from '@angular/core';
import { FormGroup, FormArray, FormBuilder, Validators } from '@angular/forms';
import { FilterService } from '../services/filter.service';
import { Filter } from '../types/Filter';
import { valueOrDefault } from 'chart.js/dist/helpers/helpers.core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  selectedOption: string = 'filter1';

  filterOptions: Filter[] = [];

  filterGroup: FormGroup;
  criterias!: FormArray;

  modalOpen: boolean = false;
  sequence: number = 1;

  onOptionChange() {
    // Handle the selected option change here
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
      fieldName: ['closePrice', Validators.required],
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
    }
  }

}
