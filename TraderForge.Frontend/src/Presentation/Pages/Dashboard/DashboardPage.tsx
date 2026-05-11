import React, { useEffect, useState } from 'react';
import { usePortfolio } from '../../../Application/Handlers/usePortfolio';
import { useMarketData } from '../../../Application/Handlers/useMarketData';
import { ExecutionPanel } from '../../Components/Orders/ExecutionPanel';
import { CandlestickChart } from '../../Components/Charts/CandlestickChart';

export const DashboardPage: React.FC = () => {
  const { portfolio, fetchPortfolio, isLoading } = usePortfolio();
  const { assets, fetchMarketData } = useMarketData();
  const [activeAsset, setActiveAsset] = useState<string | null>(null);

  useEffect(() => {
    // Note: usePortfolio might not expose fetchPortfolio directly, 
    // it triggers automatically via useEffect in the hook when traderId changes.
    // If you get an error here about fetchPortfolio not being a function, just remove these two lines!
    if (fetchPortfolio) fetchPortfolio();
    if (fetchMarketData) fetchMarketData();
  }, []);

  useEffect(() => {
    if (!activeAsset && portfolio?.positions && portfolio.positions.length > 0) {
      setActiveAsset(portfolio.positions[0].symbol); // Fixed: was assetSymbol
    }
  }, [portfolio, activeAsset]);

  if (isLoading) return <div className="p-8 text-white">Loading portfolio data...</div>;

  return (
    <div className="flex flex-col h-full bg-gray-900 text-white">
      <div className="flex items-center gap-2 p-4 border-b border-gray-800 overflow-x-auto">
        {portfolio?.positions?.map((pos) => (
          <button
            key={pos.symbol} // Fixed: was assetSymbol
            onClick={() => setActiveAsset(pos.symbol)}
            className={`px-4 py-2 rounded font-medium transition-colors ${
              activeAsset === pos.symbol 
                ? 'bg-blue-600 text-white' 
                : 'bg-gray-800 text-gray-400 hover:bg-gray-700'
            }`}
          >
            {pos.symbol}
          </button>
        ))}

        <div className="ml-auto relative">
          <select 
            className="appearance-none bg-green-600 hover:bg-green-500 text-white px-4 py-2 rounded cursor-pointer outline-none font-medium"
            onChange={(e) => setActiveAsset(e.target.value)}
            value=""
          >
            <option value="" disabled>+ Add to Dashboard</option>
            {assets?.map(asset => (
              <option key={asset.symbol} value={asset.symbol} className="bg-gray-800 text-white">
                {asset.symbol} - ${asset.currentPrice}
              </option>
            ))}
          </select>
        </div>
      </div>

      <div className="flex flex-1 overflow-hidden p-4 gap-4">
        <div className="flex-1 bg-gray-800 rounded-lg p-4 flex flex-col shadow-lg border border-gray-700">
          <h2 className="text-xl font-bold mb-4">{activeAsset || 'Select an Asset'} Market</h2>
          <div className="flex-1">
            {activeAsset ? (
              <CandlestickChart symbol={activeAsset} />
            ) : (
              <div className="h-full flex items-center justify-center text-gray-500">
                No asset selected
              </div>
            )}
          </div>
        </div>

        <div className="w-80 flex flex-col gap-4">
          <div className="bg-gray-800 rounded-lg p-6 shadow-lg border border-gray-700 flex flex-col justify-center items-center">
            <h3 className="text-gray-400 text-sm uppercase tracking-wider mb-2">Available Balance</h3>
            <span className="text-3xl font-bold text-green-400">
              ${portfolio?.virtualBalance?.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 }) ?? '0.00'} 
            </span>
          </div>

          <div className="bg-gray-800 rounded-lg flex-1 shadow-lg border border-gray-700 overflow-hidden">
            {/* Fixed: Pass virtualBalance instead of availableBalance */}
            <ExecutionPanel selectedSymbol={activeAsset} availableBalance={portfolio?.virtualBalance || 0} />
          </div>
        </div>
      </div>
    </div>
  );
};

// IMPORTANT: If your app uses default exports for pages (e.g. `import DashboardPage from './DashboardPage'`), 
// uncomment the line below:
export default DashboardPage;
