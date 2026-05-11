import type { Order } from '../../Domain/Entities/Order';
import { Result } from '../../Application/Common/Result';
import type { PlaceOrderCommand } from '../../Application/DTOs/Commands/PlaceOrderCommand';
import { httpClient } from '../Http/httpClient';

export class TradingService {
  
  async placeOrder(command: PlaceOrderCommand): Promise<Result<Order>> {
    try {
      // Direct the request to either BuyPositionCommand or SellPositionCommand endpoints
      const endpoint = command.side === 'Buy' ? '/api/portfolio/buy' : '/api/portfolio/sell';
      
      const response = await httpClient.post<Order>(endpoint, {
        assetSymbol: command.symbol,
        quantity: command.quantity,
        orderType: command.type,
        limitPrice: command.limitPrice
      });
      
      return Result.ok(response.data);
    } catch (error: any) {
      return Result.fail(error.response?.data?.message || `Failed to execute ${command.side} order`);
    }
  }

  async getOrderHistory(traderId: string): Promise<Result<Order[]>> {
    try {
      const response = await httpClient.get<Order[]>('/api/portfolio/transactions');
      return Result.ok(response.data);
    } catch (error: any) {
      return Result.fail(error.response?.data?.message || 'Failed to fetch order history');
    }
  }

  async cancelOrder(orderId: string): Promise<Result<void>> {
    try {
      await httpClient.post(`/api/portfolio/orders/${orderId}/cancel`);
      return Result.ok(undefined);
    } catch (error: any) {
      return Result.fail(error.response?.data?.message || 'Failed to cancel order');
    }
  }
}
