# Stock Analyzer UI refresh - 28 October 2026

## Design
Applied the UI/UX Pro Max skill to the active Angular 17 frontend. Kept the teal identity and created a quiet research workspace with consistent navigation, typography, financial tables, source disclosure, meaningful page headings and responsive work surfaces. See DESIGN.md for the design decisions and tokens.

Removed placeholder contact information and the decorative stock-photo banners. Navigation now uses task names: Screener, Companies, Price history, Bulk deals and Participants. Active routes and keyboard focus are visible.

## Workflow improvements
- First-use screener guidance explains how to create a filter; empty, failed and loading states are distinct.
- The default trading date follows the latest actual price report. A manually selected date is retained.
- Filter dialogs have associated labels, focus trapping, Escape handling, validation, positive whole-number periods and explicit delete consequences. Failed saves retain inputs and focus the error summary. Duplicate writes are guarded.
- Dialog cancel no longer reloads the page. Report imports update the table and provenance in place and retain the chosen range; note drafts are not discarded.
- Optional imports use a disclosure so historical browsing remains the main task. Date inputs support keyboard entry.
- Notes load only when opened. The drawer is labeled shared, keeps failed-save drafts, shows save feedback and maintains focus after an asynchronous result.
- The company detail page includes an actual closing-price chart, series and available-session controls, last available close and valid previous-close percentage change. Missing numeric values remain unavailable; chart nulls are not replaced or forward-filled. Exact values remain in the table.
- Tables retain native sorting, search and pagination, with tabular numbers, clearer headers, internal horizontal scrolling and a sticky first column.

## Validation
- Production Angular build passed; existing CommonJS and Bootstrap selector warnings remain.
- Twelve focused Chrome Headless checks passed: recovery timeout/rate limits, actual response passthrough, safe write failures, timezone date handling, rendered symbol search, retained filter drafts, duplicate-submit prevention, period validation, selected-series/null-gap chart data, on-demand notes and draft retention, in-place range-preserving refresh.
- Browser checks: company search -> RELIANCE detail, actual price chart, switching to five available reports, first-use screener, filter form validation and focused error summary, Escape returning focus, shared notes and retained failed-save draft.
- Layout checks: 1280x900 desktop, 768x1024 tablet, 375x812 phone and 812x375 landscape. Measured page width matched viewport width in checked responsive states.
- Preview reads the genuine hosted API. All preview POST/DELETE requests are deliberately blocked, so browser save-failure checks did not create or modify shared filters or notes. Successful-save transitions are covered by isolated in-memory tests.
- No application console errors were captured in checked success flows. This is a focused regression audit, not a formal accessibility certification or exhaustive legacy-test-suite claim.

## Delivery
Source stays on codex/stock-analyzer-recovery. The original uncommitted StockAnalyzerController.cs matches its private baseline byte-for-byte and is excluded from UI commits.
The refreshed deploy archive is .netlify/stock-analyzer-frontend-ready.zip. It contains only the built frontend and SPA redirect. The existing Netlify project is rparth07-stock-explorer; no duplicate site or paid resource was created.
Publication still requires an authenticated upload to https://app.netlify.com/projects/rparth07-stock-explorer/deploys . Available Netlify MCP operations cannot upload, and CLI/dashboard access was unavailable in this session. Public frontend behavior must be verified after publication.


## 2026-10-28 table and footer corrections

Removed scrolling from the outer table card; only the data wrapper scrolls horizontally. All eight paginated tables append their page-size overlay outside the card. The workspace uses a full-height flex column so the footer reaches the viewport bottom on short pages and follows long content.

Verified locally at 726 x 1532 with genuine API data: Clear/search fully visible, 10/25/50 options uncut, selecting 25 renders 25 rows, no page-wide horizontal overflow, footer at viewport bottom for 10 rows and after the table for 25. Production build and all 12 focused regression tests pass. Refreshed Netlify ZIP awaits upload to the existing site.
