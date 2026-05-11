import React, { useEffect } from 'react';
import { usePortfolio } from '../../../Application/Handlers/usePortfolio';
import { useMarketData } from '../../../Application/Handlers/useMarketData';
import { ExecutionPanel } from '../../Components/Orders/ExecutionPanel';
import { CandlestickChart } from '../../Components/Charts/CandlestickChart';

/**
 * DashboardPage - Main view for market monitoring and trade execution.
 */
export const DashboardPage: React.FC = () => {
  // Real-time portfolio data (Balance, etc.)
  const { portfolio, isLoading: portfolioLoading } = usePortfolio();
  
  // Market data (Assets list, Tabs, Selection logic)
  const { 
    assets, 
    watchedAssets, 
    unwatchedAssets, 
    selectedAsset, 
    handleSelectAsset, 
    addToWatchlist 
  } = useMarketData();

  // Effect: If no asset is selected, automatically select the first one in your tabs
  useEffect(() => {
    if (!selectedAsset && watchedAssets.length > 0) {
      handleSelectAsset(watchedAssets[0]);
    }
  }, [watchedAssets, selectedAsset, handleSelectAsset]);

  if (portfolioLoading) {
    return (
      <div className="flex items-center justify-center h-full bg-gray-900 text-white">
        <div className="flex flex-col items-center gap-4">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500"></div>
          <p className="text-gray-400 font-medium">Syncing with secure vault...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="flex flex-col h-full bg-gray-900 text-white overflow-hidden font-sans">
      
      {/* --- TOP NAVIGATION: Tabs & Add Asset --- */}
      <div className="flex items-center gap-2 p-4 border-b border-gray-800 bg-gray-850 overflow-x-auto min-h-[73px] shadow-sm">
        <div className="flex items-center mr-4">
          <span className="text-[10px] font-black text-gray-500 uppercase tracking-[0.2em] border-r border-gray-700 pr-4 mr-2">
            Terminal
          </span>
        </div>
        
        {/* Active Tabs (Watched Assets) */}
        <div className="flex gap-2">
          {watchedAssets.map((asset) => (
            <button
              key={asset.symbol}
              onClick={() => handleSelectAsset(asset)}
              className={`px-5 py-2 rounded-md font-bold text-sm transition-all border ${
                selectedAsset?.symbol === asset.symbol 
                  ? 'bg-blue-600 border-blue-400 text-white shadow-[0_0_15px_rgba(37,99,235,0.4)]' 
                  : 'bg-gray-800 border-gray-700 text-gray-400 hover:bg-gray-750 hover:text-white'
              }`}
            >
              {asset.symbol}
            </button>
          ))}
        </div>

        {/* Add Asset Dropdown */}
        <div className="ml-4 relative">
          <select 
            className="appearance-none bg-emerald-600 hover:bg-emerald-500 text-white px-6 py-2 rounded-md cursor-pointer outline-none font-black text-sm shadow-md transition-all pr-10 uppercase"
            onChange={(e) => {
              const symbol = e.target.value;
              const asset = assets.find(a => a.symbol === symbol);
              if (asset) {
                addToWatchlist(symbol);
                handleSelectAsset(asset);
              }
            }}
            value=""
          >
            <option value="" disabled>+ Add Market</option>
            {unwatchedAssets.length > 0 ? (
              unwatchedAssets.map(asset => (
                <option key={asset.symbol} value={asset.symbol} className="bg-gray-800 text-white py-2">
                  {asset.symbol} — ${asset.currentPrice?.toLocaleString() ?? '0.00'}
                </option>
              ))
            ) : (
              <option disabled className="text-gray-500 bg-gray-800">All assets added</option>
            )}
          </select>
          <div className="absolute right-3 top-1/2 -translate-y-1/2 pointer-events-none text-white text-xs">
            ▼
          </div>
        </div>
      </div>

      {/* --- MAIN DASHBOARD VIEWPORT --- */}
      <div className="flex flex-1 overflow-hidden p-6 gap-6">
        
        {/* 1. LEFT COLUMN: Market Chart */}
        <div className="flex-1 bg-gray-850 rounded-xl p-6 flex flex-col shadow-2xl border border-gray-800 relative">
          <div className="flex justify-between items-start mb-8">
            <div>
              <div className="flex items-center gap-3 mb-1">
                <h2 className="text-3xl font-black text-white tracking-tight">
                  {selectedAsset?.symbol || 'No Selection'}
                </h2>
                <span className="text-[10px] bg-emerald-500/10 text-emerald-400 border border-emerald-500/20 px-2 py-0.5 rounded font-bold uppercase">
                  Live
                </span>
              </div>
              <p className="text-emerald-400 font-mono text-2xl font-medium">
                ${selectedAsset?.currentPrice?.toLocaleString(undefined, { minimumFractionDigits: 2 }) ?? '0.00'}
              </p>
            </div>
            
            <div className="text-right">
              <span className="text-[10px] text-gray-500 font-bold uppercase tracking-widest block mb-1">Market Volatility</span>
              <div className="flex gap-1 justify-end">
                <div className="w-1 h-3 bg-emerald-500 rounded-full"></div>
                <div className="w-1 h-3 bg-emerald-500 rounded-full"></div>
                <div className="w-1 h-3 bg-gray-700 rounded-full"></div>
              </div>
            </div>
          </div>
          
          <div className="flex-1 min-h-0 bg-gray-900/50 rounded-lg border border-gray-800/50">
            {selectedAsset?.symbol ? (
              <CandlestickChart symbol={selectedAsset.symbol} />
            ) : (
              <div className="h-full flex items-center justify-center text-gray-600 italic border-2 border-dashed border-gray-800 rounded-lg m-4">
                Pick a symbol from the top bar to initialize chart data
              </div>
            )}
          </div>
        </div>

        {/* 2. RIGHT COLUMN: Balance & Trading Panel */}
        <div className="w-96 flex flex-col gap-6">
          
          {/* Virtual Balance Card */}
          <div className="bg-gradient-to-br from-blue-600 via-blue-700 to-indigo-900 rounded-xl p-8 shadow-2xl border border-blue-400/20 relative overflow-hidden group">
            <div className="absolute -right-8 -top-8 w-32 h-32 bg-white/5 rounded-full blur-3xl group-hover:bg-white/10 transition-all"></div>
            
            <h3 className="text-blue-100/60 text-[10px] font-black uppercase tracking-[0.2em] mb-3">
              Buying Power
            </h3>
            <div className="flex items-baseline gap-1.5">
              <span className="text-blue-300 text-xl font-light">$</span>
              <span className="text-4xl font-black text-white tabular-nums tracking-tight">
                {portfolio?.virtualBalance?.toLocaleString(undefined, { 
                  minimumFractionDigits: 2, 
                  maximumFractionDigits: 2 
                }) ?? '0.00'}
              </span>
            </div>
          </div>

          {/* Execution Panel */}
          <div className="bg-gray-850 rounded-xl flex-1 shadow-2xl border border-gray-800 overflow-hidden flex flex-col">
            <div className="px-6 py-4 border-b border-gray-800 bg-gray-800/30 flex justify-between items-center">
              <h4 className="text-[10px] font-black text-gray-400 uppercase tracking-widest">Execution Engine</h4>
              <div className="w-2 h-2 rounded-full bg-emerald-500 animate-pulse"></div>
            </div>
            
            <div className="p-6 flex-1 overflow-y-auto">
              <ExecutionPanel 
                selectedSymbol={selectedAsset?.symbol || null} 
                availableBalance={portfolio?.virtualBalance || 0}
                currentPrice={selectedAsset?.currentPrice || 0}
              />
            </div>
          </div>

        </div>
      </div>
    </div>
  );
};

export default DashboardPage;
