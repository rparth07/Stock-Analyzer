import { BulkDeal } from "./BulkDeal"

export type Client = {
    id: Number,
    name: string,
    deals: BulkDeal[]
}