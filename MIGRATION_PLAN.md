# Stock Analyzer recovery — 27 October 2026

## Scope and preserved work
Existing checkout: `H:\Project\Stock Analyzer`; origin: `rparth07/Stock-Analyzer`.
Initial modified files: domain.constants.ts, StockAnalyzerController.cs, appsettings.json.
Private originals are preserved under ignored `.stock-local/original-modified-files`.
Legacy SQL Server migrations remain in place, excluded from the PostgreSQL build.
No other repository has been modified.

## Architecture
Existing Netlify site `a2d94d1f-75c6-4fb1-8483-fcb3702a4855` -> dedicated Render Free .NET 10 API -> separate Neon Free PostgreSQL project `gentle-hill-52983116`, Singapore.
Production branch: `br-crimson-wildflower-a11xpse5`; isolated validation branch: `br-fragrant-voice-a1f89g8t`.
Runtime uses pooled DATABASE_URL; migrations use direct MIGRATION_DATABASE_URL. Credentials belong only in server-side environment variables.

## Data
Genuine NSE full bhavcopy archives provide raw, unadjusted end-of-day INR prices and volume; turnover is in lakhs. Missing numeric values remain null. Bulk deals come from the NSE daily CSV and retain their actual report date.
A persistent import ledger records source, trading date, fetch timestamp and row count. Successful dates are not downloaded again. Refresh attempts the recent 14 calendar days and today's bulk snapshot at startup, then every six hours while the free instance is running. Holidays/unpublished archives return no report, never fabricated data.
Free instances sleep while idle; background refresh is not an always-on scheduler. The UI allows 90 seconds for cold starts and displays provenance.
Old notebooks, saved filters and older history cannot be recovered until the previous SQL Server database becomes accessible. No fake prices, news, metrics, forecasts or portfolios are seeded.

## Verification so far
- Existing live Netlify frontend loads, but its Somee API hostname no longer resolves.
- Original .NET 6 build passed; migrated .NET 10/PostgreSQL build passed.
- Five focused Angular tests passed: successful data passthrough, timeout, rate limit, safe write failure, trading-date timezone boundaries.
- Production Angular build passed. Existing bundle/CommonJS warnings remain.
- Isolated schema migration and readiness passed; genuine RELIANCE prices/history and 38 bulk-deal clients returned successfully.
- A synthetic filter exists only on the validation branch; its two-session close average was verified (20MICRONS: (179.84 + 188.69) / 2 = 184.265).
- Legacy AutoMapper 12 reports advisory GHSA-rvv3-g6hj-g44x; mapping depth and JSON depth are bounded as mitigation. The paid-license upgrade has not been introduced.

## Publication
Pending final validation and deployment. Original modified legacy controller remains uncommitted and will not be included in a deployment commit.
