import type { Order } from '../../Domain/Entities/Order';
import { Result } from '../../Application/Common/Result';
import { httpClient } from '../Http/httpClient';

<<<<<<< HEAD
// Use trading-pair symbols so the mock data aligns with the market service names.
const MOCK_PRICES: Record<string, number> = {
  AAPL: 189.84, TSLA: 248.50, MSFT: 415.20, GOOGL: 175.60,
  AMZN: 192.30, NVDA: 875.40, META: 521.00, BTCUSDT: 63200, ETHUSDT: 4200,
  BNBUSDT: 620, ADAUSDT: 0.52, MATICUSDT: 0.88, SOLUSDT: 28.5, DOTUSDT: 5.10,
};

const MOCK_HISTORY: Order[] = [
  { id: '1', symbol: 'AAPL', side: 'Buy', type: 'Market', quantity: 10, price: 182.50, commission: 1.83, total: 1826.83, status: 'Filled', createdAt: '2026-04-28T10:30:00Z', filledAt: '2026-04-28T10:30:01Z' },
  { id: '2', symbol: 'TSLA', side: 'Buy', type: 'Limit', quantity: 5, price: 245.00, commission: 1.23, total: 1226.23, status: 'Filled', createdAt: '2026-04-29T14:15:00Z', filledAt: '2026-04-29T15:00:00Z' },
  { id: '3', symbol: 'BTCUSDT', side: 'Sell', type: 'Market', quantity: 0.05, price: 63200, commission: 31.60, total: 3158.40, status: 'Filled', createdAt: '2026-05-01T09:32:00Z', filledAt: '2026-05-01T09:32:01Z' },
];

/** Mock implementation — replace with real API calls when backend trading endpoints are ready. */
=======
interface BackendOrder {
  id: string;
  symbol: string;
  side: string;
  type: string;
  quantity: number;
  price: number;
  commission: number;
  total: number;
  status: string;
  createdAt: string;
  filledAt: string | null;
}

>>>>>>> eace40a (feat: connect backend)
export class TradingService {
  async getOrderHistory(): Promise<Result<Order[]>> {
    try {
      const { data } = await httpClient.get<BackendOrder[]>('/api/portfolio/orders');

      const orders: Order[] = data.map((o) => ({
        id: o.id,
        symbol: o.symbol,
        side: o.side as Order['side'],
        type: o.type as Order['type'],
        quantity: o.quantity,
        price: o.price,
        commission: o.commission,
        total: o.total,
        status: o.status as Order['status'],
        createdAt: o.createdAt,
        filledAt: o.filledAt ?? undefined,
      }));

      return Result.ok(orders);
    } catch (error) {
      const e = error as { response?: { data?: { error?: string } }; code?: string };
      if (e?.code === 'ERR_NETWORK' || !e?.response) return Result.fail('Cannot reach the server.');
      if (e?.response?.data?.error) return Result.fail(e.response.data.error);
      return Result.fail('Failed to load order history.');
    }
  }
}
