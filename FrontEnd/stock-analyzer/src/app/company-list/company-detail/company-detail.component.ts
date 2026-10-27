import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Table } from 'primeng/table';
import { CompanyService } from 'src/app/services/company.service';
import { BhavInfo } from 'src/app/types/BhavInfo';
import { BulkDeal, StockAction } from 'src/app/types/BulkDeal';
import { Company } from 'src/app/types/Company';

@Component({
  selector: 'app-company-detail',
  templateUrl: './company-detail.component.html',
  styleUrls: ['./company-detail.component.css', '../../shared/css/common-page-style.css']
})
export class CompanyDetailComponent implements OnInit {
  company: Company | null = null;
  errorMessage = '';

  bhavInfos: BhavInfo[] = [];
  selectedBhavInfos!: BhavInfo[];

  bulkDeals: BulkDeal[] = [];
  selectedBulkDeals!: BulkDeal[];

  loading: boolean = true;
  showBhavInfo: Boolean = true;
  chartSeries = 'EQ';
  chartSessions = 10;
  availableSeries: string[] = [];
  chartRows: BhavInfo[] = [];
  chartData: any = null;
  latestReport: BhavInfo | null = null;
  closingPrice: number | null = null;
  changePercent: number | null = null;
  chartOptions: any = {
    responsive: true, maintainAspectRatio: false, animation: false,
    plugins: { legend: { display: false }, tooltip: { displayColors: false } },
    scales: { x: { grid: { display: false }, ticks: { color: '#607783', maxTicksLimit: 7 } },
      y: { beginAtZero: false, grid: { color: '#e8eef1' }, ticks: { color: '#607783' }, title: { display: true, text: 'Closing price (INR)' } } }
  };
  updateChart(): void {
    const rows = this.bhavInfos.filter(row => row.series === this.chartSeries)
      .sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());
    this.chartRows = this.chartSessions ? rows.slice(-Number(this.chartSessions)) : rows;
    this.latestReport = rows[rows.length - 1] ?? null;
    const close = this.latestReport?.closePrice, previous = this.latestReport?.previousClose;
    this.closingPrice = typeof close === 'number' && Number.isFinite(close) ? close : null;
    this.changePercent = this.closingPrice !== null && typeof previous === 'number' && previous > 0 && Number.isFinite(previous)
      ? (this.closingPrice - previous) / previous * 100 : null;
    this.chartData = { labels: this.chartRows.map(row => new Date(row.date).toLocaleDateString('en-GB', {day:'2-digit',month:'short'})),
      datasets: [{ label: 'Close (INR)', data: this.chartRows.map(row => typeof row.closePrice === 'number' && Number.isFinite(row.closePrice) ? row.closePrice : null),
        borderColor: '#15786b', backgroundColor: '#15786b12', fill: true, spanGaps: false, tension: 0, borderWidth: 2, pointRadius: 3, pointHoverRadius: 5 }] };
  }

  constructor(private route: ActivatedRoute, private companyService: CompanyService) { }

  ngOnInit(): void {
    this.getCompanyDetails();
  }

  clear(table: Table) {
    table.clear();
    (<HTMLInputElement>document.getElementById('search-value')).value = '';
  }

  getCompanyDetails(): void {
    let companyName = this.route.snapshot.queryParams['symbol'];
    if (!companyName || typeof companyName !== 'string' || !companyName.trim()) {
      this.errorMessage = 'Choose a company from the company list.'; this.loading = false; return;
    }
    this.loading = true; this.errorMessage = '';
    this.companyService.fetchCompany(companyName)
      .subscribe({ next: company => {
        if (!company) { this.errorMessage = 'No company data was returned.'; this.loading = false; return; }
        this.company = company;
        this.bhavInfos = this.company.bhavCopyInfos ?? [];
        this.bulkDeals = this.company.bulkDeals ?? [];
        this.availableSeries = [...new Set(this.bhavInfos.map(row => row.series))].sort();
        this.chartSeries = this.availableSeries.includes('EQ') ? 'EQ' : this.availableSeries[0] ?? 'EQ';
        this.updateChart();
        this.showBhavInfo = true;
        this.loading = false;
      }, error: () => { this.loading = false; this.errorMessage = 'Company data is unavailable. Try again or choose another symbol.'; } });
  }

  showBhavInfos() {
    this.showBhavInfo = true;
  }

  showBulkDeals() {
    this.showBhavInfo = false;
  }

  getStockAction(value: number) {
    return StockAction[value];
  }
}
