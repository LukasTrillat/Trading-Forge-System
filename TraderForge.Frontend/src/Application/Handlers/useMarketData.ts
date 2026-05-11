import { useEffect, useCallback, useRef } from 'react';
import { useMarketStore } from '../Store/marketStore';
import { MarketService } from '../../Infrastructure/Services/MarketService';
import type { Asset } from '../../Domain/Entities/Asset';

const marketService = new MarketService();

export function useMarketData() {
  const {
    assets, watchlist, selectedAsset, candles, orderBook, isLoading,
    setAssets, selectAsset, setCandles, setOrderBook, setLoading,
    addToWatchlist, removeFromWatchlist,
  } = useMarketStore();

  const isFetching = useRef(false);

  // Aggressive filtering to prevent empty lists
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
