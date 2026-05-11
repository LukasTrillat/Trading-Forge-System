export interface PlaceOrderCommand {
  symbol: string;
  side: 'Buy' | 'Sell';
  quantity: number;
  type: 'Market' | 'Limit';
  limitPrice?: number;
}
