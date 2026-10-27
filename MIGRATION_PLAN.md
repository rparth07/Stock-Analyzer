# Stock Analyzer recovery - 27 October 2026

## Scope and preserved work
Existing checkout: `H:\Project\Stock Analyzer`; origin: `rparth07/Stock-Analyzer`.
Initial modified files: domain.constants.ts, StockAnalyzerController.cs, appsettings.json.
Original copies remain under ignored `.stock-local/original-modified-files`.
The user's original controller edits remain uncommitted and were excluded from deployment commits. Legacy SQL Server migrations remain in place and are excluded from the PostgreSQL build. No other repository was modified.

## Architecture and publication
Existing Netlify site `a2d94d1f-75c6-4fb1-8483-fcb3702a4855` -> dedicated Render Free .NET 10 API -> separate Neon Free PostgreSQL project `gentle-hill-52983116`, Singapore.
Production database branch: `br-crimson-wildflower-a11xpse5`; isolated validation branch: `br-fragrant-voice-a1f89g8t`.
Runtime uses pooled DATABASE_URL; migrations use direct MIGRATION_DATABASE_URL. Credentials exist only in server-side environment configuration.

Backend is live at https://rparth07-stock-analyzer-api.onrender.com .
Render service: `srv-dbgbircipgvc73f3hgl0`; live backend deploy: `dep-dbgbsed9fdbs73dbt7g0`, commit `4675d31`.
Source branch: `codex/stock-analyzer-recovery`. Later commits contain frontend fixes and this report; Render automatic deploy is off.

The existing Netlify site still serves its old frontend until upload. Upload `.netlify/stock-analyzer-frontend-ready.zip` to https://app.netlify.com/projects/rparth07-stock-explorer/deploys . The ZIP contains only static production assets and the SPA redirect rule.
Netlify MCP has no upload/deploy action, no CLI authentication was found, the in-app dashboard returns Access Denied, and native Edge access timed out. No duplicate Netlify site was created. The previous Netlify deployment is manual, with no build or Git commit attached.
Public frontend requests and nested-route refreshes need rechecking after publication.

## Genuine data and limits
NSE full bhavcopy archives provide raw, unadjusted end-of-day INR prices and volume; turnover is in lakhs. Missing numeric values remain null. Bulk deals come from NSE's daily CSV and retain the report's trading date.
A persistent ledger records source, trading date, fetch timestamp and row count. Successful dates are not downloaded again. Refresh attempts the recent 14 calendar days and the latest bulk snapshot on startup, then every six hours while the free instance runs. Holidays/unpublished reports never produce fabricated data.
Manual bhavcopy imports accept published dates within the past year. Arbitrary historical bulk snapshots are unsupported and return 422; previously imported days remain queryable.
Old notebooks, saved filters and older history require access to the old SQL Server database for recovery. The new database contains genuine recent reports only.
Render Free sleeps while idle; background refresh is not an always-on scheduler. UI requests allow 90 seconds for cold starts and leave navigation visible.
The original application has shared filters/notebooks and no account system. News, forecasts, trading, portfolios, alerts and exports are not implemented in active templates; no such data was fabricated or added.

## Verified fixes and checks
- Production Angular build and six focused Chrome Headless tests passed: real data passthrough, request timeout, rate limit, safe write failure, timezone date boundaries, rendered symbol search. The old scaffold test suite was not claimed healthy.
- .NET 10 publish passed; PostgreSQL migrations and EF pending-model check passed on the isolated validation branch.
- Live readiness 200; RELIANCE history; 38 clients; 70 bulk deals; exact Netlify CORS origin on a 204 preflight; repeat import returns zero new rows; unsupported historical bulk import returns 422.
- All twelve sampled RELIANCE quote/volume/delivery fields exactly matched NSE's 27 October archive.
- Company list returns 3,701 symbols with at most one latest price record each.
- A synthetic filter only on the validation branch matched a two-session average: 20MICRONS (179.84 + 188.69) / 2 = 184.265, displayed as 184.27.
- Synthetic notebook text persisted only on the isolated validation branch. Opening the notebook no longer saves; failed saves retain drafts and saving stays disabled until existing notes load.
- Browser checks passed symbol search, direct detail refresh, genuine screener results and desktop/390px layouts. No page-width overflow and no console errors in checked success paths.
- Corrected wrong search fields, trading-date label timezone shifts, clipped mobile links, scrollable tables, criterion sorting, missing bulk-deal rows and garbled trend arrows.
- Failed requests now settle local spinners, display actionable messages and offer retries; missing symbols and invalid date ranges do not crash.

## Remaining warnings and credential cleanup
Existing nullable/CommonJS warnings remain. Legacy AutoMapper 12 reports GHSA-rvv3-g6hj-g44x; mapping depth and JSON depth are bounded as mitigation, but the advisory remains. No paid-license dependency upgrade was introduced.
The embedded legacy SQL password was removed from publishable configuration and retained only in the private original backup. Rotate it because it remains in repository history. No secret values were printed or included in the frontend ZIP.

## Interface refresh - 28 October 2026
The frontend now includes a genuine closing-price chart and a coordinated research-workspace interface. Twelve focused tests and responsive browser checks passed. See UI_AUDIT.md for scope, evidence and publication status. The upload ZIP is refreshed with this UI; backend code and hosting resources were not changed during the UI refresh.
