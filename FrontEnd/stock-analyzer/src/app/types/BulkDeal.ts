export type BulkDeal = {
    clientName: string,
    dealDate: Date,
    companySymbol: string,
    companyFullName: string,
    stockAction: StockAction,
    quantity: Number,
    tradePrice: Number,
    remarks: string | null
}

export enum StockAction {
    Buy = 0,
    Sell = 1
}

/*companySymbol: string,
companyFullName: string,
date: Date,
series: string,
previousClose: Number,
openPrice: Number,
highPrice: Number,
lowPrice: Number,
lastPrice: Number,
closePrice: Number,
avgPrice: Number,
*/