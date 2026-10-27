import { TestBed, fakeAsync, tick } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { of } from 'rxjs';
import { CompanyListComponent } from './company-list.component';
import { CompanyService } from '../services/company.service';

describe('Company symbol search', () => {
  it('finds a real symbol using the rendered search control', fakeAsync(() => {
    TestBed.configureTestingModule({
      declarations: [CompanyListComponent],
      imports: [TableModule, ButtonModule, InputTextModule, RouterTestingModule, NoopAnimationsModule],
      providers: [{ provide: CompanyService, useValue: { fetchAllCompanies: () => of([
        { id: '1', symbol: 'RELIANCE', bhavCopyInfos: [], bulkDeals: [] },
        { id: '2', symbol: 'OTHER', bhavCopyInfos: [], bulkDeals: [] }
      ]) } }]
    });
    const fixture = TestBed.createComponent(CompanyListComponent); fixture.detectChanges(); tick();
    const input = fixture.nativeElement.querySelector('#search-value') as HTMLInputElement;
    input.value = 'RELIANCE'; input.dispatchEvent(new Event('input')); tick(); fixture.detectChanges();
    const rows = fixture.nativeElement.querySelector('tbody').textContent;
    expect(rows).toContain('RELIANCE'); expect(rows).not.toContain('OTHER');
  }));
});
