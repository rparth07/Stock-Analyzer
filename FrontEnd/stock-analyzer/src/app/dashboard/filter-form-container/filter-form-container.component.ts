import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
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

  @ViewChild('errorSummary') errorSummary?: ElementRef<HTMLElement>;
  focusError() { setTimeout(() => this.errorSummary?.nativeElement.focus()); }
  errorMessage = '';
  sequence: number = 1;
  isLoading: Boolean = false;

  constructor(private formBuilder: FormBuilder, private filterService: FilterService) {
    this.filterGroup = this.formBuilder.group({
      FilterName: ['', [Validators.required, Validators.pattern(/\S/), Validators.maxLength(80)]],
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
    if (!this.isLoading) this.closeModalEvent.emit(false);
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
      periodValue: [2, [Validators.required, Validators.min(1), Validators.pattern(/^[0-9]+$/)]],
      periodType: ['Days', Validators.required],
      changeType: ['Increase', Validators.required],
      logicalOperator: ['And', Validators.required]
    });
  }

  submitForm() {
    if (this.isLoading) return;
    this.filterGroup.markAllAsTouched();
    if (this.filterGroup.invalid) { this.errorMessage = 'Enter a name and positive whole numbers for every period.'; this.focusError(); return; }
    this.isLoading = true; this.errorMessage = '';
    const value = this.filterGroup.getRawValue(); value.FilterName = value.FilterName.trim();
    value.criterias = value.criterias.map((row: any, i: number) => ({ ...row, sequence: i + 1 }));
    this.filterService.addFilter(value as Filter).subscribe({
      next: () => { this.isLoading = false; this.closeModalEvent.emit(true); },
      error: () => { this.isLoading = false; this.errorMessage = 'The filter was not saved. Your inputs are still here. Try again or use a different name.'; this.focusError(); }
    });
  }
}
