import { MatDateFormats } from "@angular/material/core";

export const DomainConstants = {
    StockAnalyzer_URL: 'https://localhost:7012/StockAnalyzer/',
    Filter_URL: 'https://localhost:7012/Filter/',
    Notebook_URL: 'https://localhost:7012/Notebook/',
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