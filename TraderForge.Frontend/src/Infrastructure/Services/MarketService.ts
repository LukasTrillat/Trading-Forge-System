import type { Asset, CandlestickBar, OrderBook } from '../../Domain/Entities/Asset';
import { Result } from '../../Application/Common/Result';
import type { CandleInterval } from '../../Domain/Interfaces/IMarketService';
import { httpClient } from '../Http/httpClient';

export class MarketService {
  
  async getAssets(): Promise<Result<Asset[]>> {
    try {
      const response = await httpClient.get<Asset[]>('/api/prices');
      return Result.ok(response.data);
    } catch (error: any) {
      return Result.fail(error.response?.data?.message || 'Failed to fetch market assets');
    }
  }

  async getAssetBySymbol(symbol: string): Promise<Result<Asset>> {
    try {
      const response = await httpClient.get<Asset>(`/api/prices/${symbol}`);
      return Result.ok(response.data);
    } catch (error: any) {
      return Result.fail(error.response?.data?.message || `Failed to fetch asset ${symbol}`);
    }
  }

  async getCandles(symbol: string, interval: CandleInterval): Promise<Result<CandlestickBar[]>> {
    try {
      const response = await httpClient.get<CandlestickBar[]>(`/api/prices/${symbol}/candles`, {
        params: { interval }
      });
      return Result.ok(response.data);
    } catch (error: any) {
      return Result.fail(error.response?.data?.message || 'Failed to fetch candlestick data');
    }
  }

  async getOrderBook(symbol: string): Promise<Result<OrderBook>> {
    try {
      const response = await httpClient.get<OrderBook>(`/api/prices/${symbol}/orderbook`);
      return Result.ok(response.data);
    } catch (error: any) {
      return Result.fail(error.response?.data?.message || 'Failed to fetch order book');
    }
  }

  async searchAssets(query: string): Promise<Result<Asset[]>> {
    try {
      const response = await httpClient.get<Asset[]>('/api/prices/search', {
        params: { q: query }
      });
      return Result.ok(response.data);
    } catch (error: any) {
      return Result.fail(error.response?.data?.message || 'Search failed');
    }
  }
}
