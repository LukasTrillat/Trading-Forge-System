import { useState, useEffect, useCallback } from 'react';
import { httpClient } from '../../Infrastructure/Http/httpClient';
import type { Portfolio } from '../../Domain/Entities/Portfolio';

function mapPortfolio(raw: any): Portfolio {
  return {
    traderId: raw.traderId ?? '',
    virtualBalance: raw.virtualBalance ?? 0,
    totalPortfolioValue: raw.totalPortfolioValue ?? raw.virtualBalance ?? 0,
    totalPnL: raw.totalPnL ?? 0,
    totalPnLPercent: raw.totalPnLPercent ?? 0,
    positions: (raw.positions ?? []).map((p: any) => ({
      symbol: p.symbol ?? '',
      assetName: p.assetName ?? p.symbol ?? '',
      quantity: p.quantity ?? 0,
      averageBuyPrice: p.averageBuyPrice ?? p.entryPrice ?? 0,
      currentPrice: p.currentPrice ?? p.entryPrice ?? 0,
      unrealizedPnL: p.unrealizedPnL ?? 0,
      unrealizedPnLPercent: p.unrealizedPnLPercent ?? 0,
      totalValue: p.totalValue ?? (p.quantity ?? 0) * (p.currentPrice ?? p.entryPrice ?? 0),
    })),
  };
}

export function usePortfolio() {
  const [portfolio, setPortfolio] = useState<Portfolio | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  const refreshPortfolio = useCallback(async () => {
    try {
      const { data } = await httpClient.get('/api/portfolio/active');
      setPortfolio(mapPortfolio(data));
    } catch (error) {
      console.error('Failed to fetch portfolio balance');
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    refreshPortfolio();
  }, [refreshPortfolio]);

  return {
    portfolio,
    isLoading,
    refreshPortfolio
  };
}
