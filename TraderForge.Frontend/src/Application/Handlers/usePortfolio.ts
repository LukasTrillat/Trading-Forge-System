import { useState, useEffect, useCallback } from 'react';
import { httpClient } from '../../Infrastructure/Http/httpClient';

interface Portfolio {
  virtualBalance: number;
  traderId: string;
  isActive: boolean;
}

export function usePortfolio() {
  const [portfolio, setPortfolio] = useState<Portfolio | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  const refreshPortfolio = useCallback(async () => {
    try {
      const { data } = await httpClient.get<Portfolio>('/api/portfolio/active');
      setPortfolio(data);
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
