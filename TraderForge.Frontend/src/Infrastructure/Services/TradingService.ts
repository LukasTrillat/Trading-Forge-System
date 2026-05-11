import type { Order } from '../../Domain/Entities/Order';
import { Result } from '../../Application/Common/Result';
import type { PlaceOrderCommand } from '../../Application/DTOs/Commands/PlaceOrderCommand';
import { httpClient } from '../Http/httpClient';

export class TradingService {
  
  async placeOrder(command: PlaceOrderCommand, currentPrice: number): Promise<Result<Order>> {
    try {
      if (command.side === 'Buy') {
        return await this.executeBuy(command, currentPrice);
      } else {
        return await this.executeSell(command);
      }
    } catch (error: any) {
      return Result.fail(error.response?.data?.error || `Order execution failed.`);
    }
  }

  private async executeBuy(command: PlaceOrderCommand, currentPrice: number): Promise<Result<Order>> {
    const entryPrice = command.type === 'Limit' && command.limitPrice ? command.limitPrice : currentPrice;

    const { data } = await httpClient.post<Order>('/api/portfolio/positions/buy', {
      symbol: command.symbol,
      quantity: command.quantity,
      entryPrice,
    });

    return Result.ok(data);
  }

  private async executeSell(command: PlaceOrderCommand): Promise<Result<Order>> {
    const positionId = await this.resolvePositionId(command.symbol);
    if (!positionId) {
      return Result.fail(`No open position found for ${command.symbol}.`);
    }

    const { data } = await httpClient.post<Order>(`/api/portfolio/positions/${positionId}/sell`, {
      quantity: command.quantity,
    });

    return Result.ok(data);
  }

  private async resolvePositionId(symbol: string): Promise<string | null> {
    try {
      const { data } = await httpClient.get<any>('/api/portfolio/active');
      const position = (data.positions ?? []).find(
        (p: any) => p.symbol?.toUpperCase() === symbol.toUpperCase()
      );
      return position?.id ?? null;
    } catch {
      return null;
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
