import { HttpClient } from '@angular/common/http';
import { DomainConstants } from './shared/domain.constants';
import { Component } from '@angular/core';
import { ApiStatusService } from './shared/api-status.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'stock-analyzer';
  marketStatus: { sources: { source: string; tradingDate: string; importedAt: string }[] } | null = null;
  constructor(public apiStatus: ApiStatusService, http: HttpClient) {
    const url = DomainConstants.StockAnalyzer_URL.replace('StockAnalyzer/', 'market-data/status');
    http.get<NonNullable<AppComponent['marketStatus']>>(url).subscribe({ next: status => this.marketStatus = status, error: () => {} });
  }
}
