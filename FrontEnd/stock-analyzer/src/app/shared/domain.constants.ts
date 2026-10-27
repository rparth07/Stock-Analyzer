import { MatDateFormats } from "@angular/material/core";

export const DomainConstants = {
    StockAnalyzer_URL: 'https://rparth07-stock-analyzer-api.onrender.com/StockAnalyzer/',
    Filter_URL: 'https://rparth07-stock-analyzer-api.onrender.com/Filter/',
    Notebook_URL: 'https://rparth07-stock-analyzer-api.onrender.com/Notebook/',
};

export const MY_DATE_FORMATS: MatDateFormats = {
    parse: {
        dateInput: ['DD/MM/yyyy'],
    },
    display: {
        dateInput: 'DD/MM/yyyy',
        monthYearLabel: 'MMMM YYYY',
        dateA11yLabel: 'LL',
        monthYearA11yLabel: 'MMMM YYYY',
    },
};