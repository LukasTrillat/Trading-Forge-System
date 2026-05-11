import type { Order } from '../../Domain/Entities/Order';
import { Result } from '../../Application/Common/Result';
import type { PlaceOrderCommand } from '../../Application/DTOs/Commands/PlaceOrderCommand';
import { httpClient } from '../Http/httpClient';

export class TradingService {
  
  async placeOrder(command: PlaceOrderCommand): Promise<Result<Order>> {
    try {
      // Corrected endpoints to match PortfolioController.cs exactly
      const endpoint = command.side === 'Buy' 
        ? '/api/portfolio/positions/buy' 
        : `/api/portfolio/positions/${command.symbol}/sell`; // Adjust based on if your sell logic uses symbol or id
      
      const response = await httpClient.post<Order>(endpoint, {
        assetSymbol: command.symbol,
        quantity: command.quantity,
        orderType: command.type,
        limitPrice: command.limitPrice
      });
      
      return Result.ok(response.data);
    } catch (error: any) {
      return Result.fail(error.response?.data?.error || `Order execution failed.`);
    }
  }

  async getOrderHistory(): Promise<Result<Order[]>> {
    try {
      const response = await httpClient.get<Order[]>('/api/portfolio/transactions');
      return Result.ok(response.data);
    } catch (error: any) {
      return Result.fail('Failed to fetch transaction history.');
    }
  }
}
