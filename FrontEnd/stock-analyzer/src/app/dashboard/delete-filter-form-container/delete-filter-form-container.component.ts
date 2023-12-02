import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormArray, FormBuilder, Validators } from '@angular/forms';
import { Filter } from 'src/app/types/Filter';
import { FilterService } from 'src/app/services/filter.service';

@Component({
  selector: 'app-delete-filter-form-container',
  templateUrl: './delete-filter-form-container.component.html',
  styleUrl: './delete-filter-form-container.component.css'
})
export class DeleteFilterFormContainerComponent implements OnInit, AfterViewInit {
  @Output() closeDeleteModalEvent = new EventEmitter<boolean>();
  @Input() filterOptions: Filter[] = [];

  selectedOption: string = '';
  selectedFilter: Filter | undefined;

  constructor(private filterService: FilterService) {

  }

  ngOnInit() {
    this.selectedOption = this.filterOptions.length > 0 ? this.filterOptions[0].filterName : '';
    this.updateSelectedFilter();
    // console.log('this.selectedFilter = ' + this.selectedFilter);
    // console.dir(this.filterOptions, { depth: null });
  }

  ngAfterViewInit() {
    if (this.filterOptions.length > 0) {
      this.selectedFilter = this.filterOptions[0];
    }
  }

  updateSelectedFilter() {
    // console.log('this.selected option = ' + this.selectedOption);
    this.selectedFilter = this.filterOptions
      .filter(_ => _.filterName == this.selectedOption)[0];
    // console.log('selected filter = ');
    // console.dir(this.selectedFilter, { depth: null });
  }

  closeModal() {
    this.closeDeleteModalEvent.emit(true);
  }

  submitForm() {
    this.filterService.deleteFilter(this.selectedOption)
      .subscribe({
        next: (value) => {
          // console.log(value);
          this.closeModal();
        },
        error: (err) => console.log(err),
      });
  }
}
