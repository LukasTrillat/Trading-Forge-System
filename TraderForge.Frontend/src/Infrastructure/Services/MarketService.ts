import type { Asset, CandlestickBar, OrderBook } from '../../Domain/Entities/Asset';
import { Result } from '../../Application/Common/Result';
import type { CandleInterval } from '../../Domain/Interfaces/IMarketService';
import { httpClient } from '../Http/httpClient';

export class MarketService {
  async getAssets(): Promise<Result<Asset[]>> {
    try {
      const { data } = await httpClient.get<Asset[]>('/api/prices');
      return Result.ok(data);
    } catch (error) {
      return Result.fail('Failed to fetch market assets.');
    }
  }

  async getAssetBySymbol(symbol: string): Promise<Result<Asset>> {
    try {
      const { data } = await httpClient.get<Asset>(`/api/prices/${symbol}`);
      return Result.ok(data);
    } catch (error) {
      return Result.fail(`Asset ${symbol} not found.`);
    }
  }

  async getCandles(symbol: string, interval: CandleInterval): Promise<Result<CandlestickBar[]>> {
    try {
      const { data } = await httpClient.get<CandlestickBar[]>(`/api/prices/${symbol}/candles`, {
        params: { interval }
      });
      return Result.ok(data);
    } catch (error) {
      return Result.fail(`Failed to fetch candlestick data for ${symbol}.`);
    }
  }

  async getOrderBook(symbol: string): Promise<Result<OrderBook>> {
    try {
      const { data } = await httpClient.get<OrderBook>(`/api/prices/${symbol}/orderbook`);
      return Result.ok(data);
    } catch (error) {
      return Result.fail(`Failed to fetch order book for ${symbol}.`);
    }
  }

  async searchAssets(query: string): Promise<Result<Asset[]>> {
    try {
      const { data } = await httpClient.get<Asset[]>('/api/prices/search', {
        params: { q: query }
      });
      return Result.ok(data);
    } catch (error) {
      return Result.fail('Search failed.');
    }
  }
}
