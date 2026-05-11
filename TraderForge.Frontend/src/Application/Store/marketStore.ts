import { create } from 'zustand';
import type { Asset, CandlestickBar, OrderBook } from '../../Domain/Entities/Asset';

interface MarketState {
  assets: Asset[];
  watchlist: string[];
  selectedAsset: Asset | null;
  candles: CandlestickBar[];
  orderBook: OrderBook | null;
  isLoading: boolean;
  setAssets: (assets: Asset[]) => void;
  selectAsset: (asset: Asset) => void;
  setCandles: (candles: CandlestickBar[]) => void;
  setOrderBook: (orderBook: OrderBook) => void;
  setLoading: (loading: boolean) => void;
  addToWatchlist: (symbol: string) => void;
  removeFromWatchlist: (symbol: string) => void;
}

export const useMarketStore = create<MarketState>((set) => ({
  assets: [],
  watchlist: [], // MUST START EMPTY
  selectedAsset: null,
  candles: [],
  orderBook: null,
  isLoading: false,

  setAssets: (newAssets) => set((state) => {
    // 1. Log what we received from C#
    console.log("RECEIVING FROM BACKEND:", newAssets);

    if (!Array.isArray(newAssets)) {
      console.error("CRITICAL: Backend did not return an array!", newAssets);
      return state;
    }

    // 2. Map to ensure no duplicates
    const uniqueAssets = Array.from(
      new Map(newAssets.map((item) => [item.symbol.toUpperCase(), item])).values()
    );

    return { assets: uniqueAssets };
  }),

  selectAsset: (asset) => set({ selectedAsset: asset }),
  setCandles: (candles) => set({ candles }),
  setOrderBook: (orderBook) => set({ orderBook }),
  setLoading: (isLoading) => set({ isLoading }),

  addToWatchlist: (symbol) => set((state) => ({
    watchlist: [...new Set([...state.watchlist, symbol.toUpperCase()])]
  })),

  removeFromWatchlist: (symbol) => set((state) => ({
    watchlist: state.watchlist.filter((s) => s !== symbol.toUpperCase()),
  })),
}));
