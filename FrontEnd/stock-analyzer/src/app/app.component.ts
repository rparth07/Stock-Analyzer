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
  get latestPriceDate(): string {
    return this.marketStatus?.sources.find(source => source.source.toLowerCase().includes('bhav'))?.tradingDate.slice(0,10) ?? 'Unavailable';
  }
  marketStatus: { sources: { source: string; tradingDate: string; importedAt: string }[] } | null = null;
  constructor(public apiStatus: ApiStatusService, private http: HttpClient) {
    this.loadMarketStatus();
    this.apiStatus.refreshMarketStatus$.subscribe(() => this.loadMarketStatus());
  }
  loadMarketStatus() {
    const url = DomainConstants.StockAnalyzer_URL.replace('StockAnalyzer/', 'market-data/status');
    this.http.get<NonNullable<AppComponent['marketStatus']>>(url).subscribe({ next: status => { this.marketStatus = status; const date = this.latestPriceDate; if (date !== 'Unavailable') this.apiStatus.marketDate$.next(date); }, error: () => {} });
  }
}
