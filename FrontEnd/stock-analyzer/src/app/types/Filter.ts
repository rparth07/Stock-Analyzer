import { Company } from "./Company"

export type Filter = {
    filterName: string,
    series: string,
    filterType: string,
    criterias: FilterCriteria[]
}

export type FilterCriteria = {
    sequence: number,
    fieldName: string,
    changeType: string,
    logicalOperator: string,
    periodType: string,
    periodValue: number
}
// Symbol, series, date, Avg fieldName(periodValue'D/W/M/Y')^
export type FilterResult = {
    id: number,
    filterCriteria: FilterCriteria,
    calculationDate: Date,
    company: Company,
    value: number
}

export enum ChangeType {
    Increase = 'Increase',
    Decrease = 'Decrease'
}

export enum LogicalOperator {
    And = 'And',
    Or = 'Or'
}

export enum PeriodType {
    Days = 'Days',
    Weeks = 'Weeks',
    Months = 'Months',
    Years = 'Years',
}

export enum filterType {
    MovingAverage = "MovingAverage",
    Continuous = "Continuous"
}