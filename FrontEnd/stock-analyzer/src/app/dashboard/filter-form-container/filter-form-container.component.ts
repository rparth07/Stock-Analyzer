import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FilterService } from 'src/app/services/filter.service';
import { Filter } from 'src/app/types/Filter';

@Component({
  selector: 'app-filter-form-container',
  templateUrl: './filter-form-container.component.html',
  styleUrls: ['./filter-form-container.component.css']
})
export class FilterFormContainerComponent implements OnInit {
  @Output() closeModalEvent = new EventEmitter<boolean>();

  filterGroup: FormGroup;
  criterias!: FormArray;

  sequence: number = 1;

  constructor(private formBuilder: FormBuilder, private filterService: FilterService) {
    this.filterGroup = this.formBuilder.group({
      FilterName: ['', Validators.required],
      Series: ['EQ', Validators.required],
      FilterType: ['Continuous', Validators.required],
      criterias: this.formBuilder.array([this.createInputRow()])
    });
  }

  ngOnInit() {
    this.criterias = this.filterGroup.get('criterias') as FormArray;
    this.sequence = 1;
  }

  closeModal() {
    this.closeModalEvent.emit(true);
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
}
