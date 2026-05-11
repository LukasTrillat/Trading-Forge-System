import { useEffect, useCallback, useRef } from 'react';
import { useMarketStore } from '../Store/marketStore';
import { MarketService } from '../../Infrastructure/Services/MarketService';
import type { Asset, CandlestickBar } from '../../Domain/Entities/Asset';

const marketService = new MarketService();
const DEFAULT_INTERVAL = '15m';

function mapBackendCandle(raw: any): CandlestickBar {
  return {
    time: raw.t ?? raw.time,
    open: raw.open,
    high: raw.high,
    low: raw.low,
    close: raw.close,
    volume: raw.volume,
  };
}

export function useMarketData() {
  const {
    assets, watchlist, selectedAsset, candles, orderBook, isLoading,
    setAssets, selectAsset, setCandles, setLoading,
    addToWatchlist, removeFromWatchlist,
  } = useMarketStore();

  const isFetching = useRef(false);
  const isFetchingCandles = useRef(false);

  const watchedAssets = assets.filter((a) => 
    watchlist.includes(a.symbol.toUpperCase())
  );
  
  const unwatchedAssets = assets.filter((a) => 
    !watchlist.includes(a.symbol.toUpperCase())
  );

  const refreshData = useCallback(async () => {
    if (isFetching.current) return;
    isFetching.current = true;

    const result = await marketService.getAssets();
    if (result.isSuccess && result.value) {
      setAssets(result.value);
    }
    
    isFetching.current = false;
    setLoading(false);
  }, [setAssets, setLoading]);

  useEffect(() => {
    setLoading(true);
    refreshData();
    const interval = setInterval(refreshData, 5000);
    return () => clearInterval(interval);
  }, [refreshData, setLoading]);

  const fetchCandles = useCallback(async (symbol: string) => {
    if (isFetchingCandles.current || !symbol) return;
    isFetchingCandles.current = true;

    setCandles([]);

    const result = await marketService.getCandles(symbol, DEFAULT_INTERVAL as any);
    if (result.isSuccess && result.value) {
      const mapped = result.value.map(mapBackendCandle);
      setCandles(mapped);
    }

    isFetchingCandles.current = false;
  }, [setCandles]);

  useEffect(() => {
    if (selectedAsset?.symbol) {
      fetchCandles(selectedAsset.symbol);
    }
  }, [selectedAsset?.symbol, fetchCandles]);

  const handleSelectAsset = useCallback((asset: Asset) => {
    selectAsset(asset);
  }, [selectAsset]);

  return {
    assets,
    watchedAssets,
    unwatchedAssets,
    selectedAsset,
    candles,
    orderBook,
    isLoading,
    handleSelectAsset,
    addToWatchlist,
    removeFromWatchlist,
  };
}
