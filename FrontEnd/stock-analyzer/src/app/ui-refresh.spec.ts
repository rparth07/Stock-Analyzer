import { NotebookComponent } from './notebook/notebook.component';
import { BhavInfoComponent } from './bhav-info/bhav-info.component';
import { ApiStatusService } from './shared/api-status.service';
import { FormBuilder } from '@angular/forms';
import { of, Subject, throwError } from 'rxjs';
import { FilterFormContainerComponent } from './dashboard/filter-form-container/filter-form-container.component';
import { CompanyDetailComponent } from './company-list/company-detail/company-detail.component';

describe('Research interface behavior', () => {
  it('retains filter inputs and unlocks the form after a failed save', () => {
    const service = { addFilter: jasmine.createSpy().and.returnValue(throwError(() => new Error('offline'))) };
    const form = new FilterFormContainerComponent(new FormBuilder(), service as any); form.ngOnInit();
    form.filterGroup.patchValue({FilterName: 'My research'}); form.submitForm();
    expect(form.filterGroup.value.FilterName).toBe('My research'); expect(form.isLoading).toBeFalse();
    expect(form.errorMessage).toContain('inputs are still here');
  });
  it('prevents repeated writes while saving and emits cancel separately', () => {
    const request = new Subject(); const service = {addFilter:jasmine.createSpy().and.returnValue(request)};
    const form = new FilterFormContainerComponent(new FormBuilder(), service as any); form.ngOnInit();
    const closed = jasmine.createSpy(); form.closeModalEvent.subscribe(closed);
    form.closeModal(); expect(closed).toHaveBeenCalledWith(false);
    form.filterGroup.patchValue({FilterName:'One request'}); form.submitForm(); form.submitForm();
    expect(service.addFilter).toHaveBeenCalledTimes(1); request.next({}); expect(closed).toHaveBeenCalledWith(true);
  });
  it('rejects nonpositive and fractional periods before a request', () => {
    const service = {addFilter:jasmine.createSpy().and.returnValue(of({}))};
    const form = new FilterFormContainerComponent(new FormBuilder(), service as any); form.ngOnInit(); form.filterGroup.patchValue({FilterName:'Period test'});
    form.criterias.at(0).patchValue({periodValue:0}); form.submitForm();
    form.criterias.at(0).patchValue({periodValue:1.5}); form.submitForm(); expect(service.addFilter).not.toHaveBeenCalled();
  });
  it('charts only the selected series and preserves missing price gaps', () => {
    const detail = new CompanyDetailComponent({} as any, {} as any);
    detail.bhavInfos = [
      {series:'EQ',date:'2026-10-23',closePrice:100,previousClose:99},
      {series:'BE',date:'2026-10-26',closePrice:999,previousClose:998},
      {series:'EQ',date:'2026-10-26',closePrice:null,previousClose:100},
      {series:'EQ',date:'2026-10-27',closePrice:110,previousClose:100}
    ] as any; detail.updateChart();
    expect(detail.chartData.datasets[0].data).toEqual([100,null,110]); expect(detail.changePercent).toBe(10);
    (detail.bhavInfos[3] as any).previousClose=0; detail.updateChart(); expect(detail.changePercent).toBeNull();
  });
});

describe('Draft-safe workspace', () => {
  it('loads notes on demand and retains a failed-save draft', () => {
    const service = {getAllNotebooks:jasmine.createSpy().and.returnValue(of([])), updateNotebooks:jasmine.createSpy().and.returnValue(throwError(() => new Error('offline')))};
    const notes = new NotebookComponent(service as any); notes.ngOnInit(); expect(service.getAllNotebooks).not.toHaveBeenCalled();
    notes.toggleState(); notes.notebooks[0].content = 'Local draft'; notes.dirty = true; notes.updateNoteBooks();
    expect(notes.notebooks[0].content).toBe('Local draft'); expect(notes.openChatBox).toBeTrue(); expect(notes.saving).toBeFalse();
    expect(notes.message).toContain('draft is still here');
  });
  it('refreshes reports in place without replacing the selected range', () => {
    const service = {fetchBhavInfosBetween:jasmine.createSpy().and.returnValue(of([]))};
    const status = new ApiStatusService(); const refreshed = jasmine.createSpy(); status.refreshMarketStatus$.subscribe(refreshed);
    const page = new BhavInfoComponent(service as any, status); const start = new Date(2026,9,20), end = new Date(2026,9,27);
    page.startDate = start; page.endDate = end; page.reloadPage();
    expect(page.startDate).toBe(start); expect(page.endDate).toBe(end); expect(service.fetchBhavInfosBetween).toHaveBeenCalledWith(start,end); expect(refreshed).toHaveBeenCalled();
  });
});
