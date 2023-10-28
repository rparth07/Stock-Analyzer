
export type Filter = {
    filterName: string,
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