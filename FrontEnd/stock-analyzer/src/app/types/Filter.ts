import { Company } from "./Company"

export type Filter = {
    filterName: string,
    series: string,
    criterias: FilterCriteria[]
}

export type FilterCriteria = {
    sequence: number,
    fieldName: string,
    changeType: ChangeType,
    logicalOperator: LogicalOperator,
    PeriodType: PeriodType,
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
    Increase,
    Decrease
}

export enum LogicalOperator {
    And,
    Or
}

export enum PeriodType {
    Days,
    Weeks,
    Months,
    Years,
}