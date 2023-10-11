import { BhavInfo } from "./BhavInfo"
import { BulkDeal } from "./BulkDeal"

export type Company = {
    symbol: string,
    fullName: string,
    bhavCopyInfos: BhavInfo[],
    bulkDeals: BulkDeal[],
}