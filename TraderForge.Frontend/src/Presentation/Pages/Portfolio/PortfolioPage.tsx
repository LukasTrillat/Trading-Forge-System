import React, { useEffect } from 'react';
import { usePortfolio } from '../../../Application/Handlers/usePortfolio';

export const PortfolioPage: React.FC = () => {
  const { portfolio, isLoading } = usePortfolio();

  if (isLoading) return <div className="text-white p-8">Loading...</div>;
  if (!portfolio) return <div className="text-white p-8">No portfolio found.</div>;

  return (
    <div className="p-8 text-white h-full bg-gray-900">
      <h1 className="text-3xl font-bold mb-6">My Portfolio</h1>
      
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
        <div className="bg-gray-800 p-6 rounded-lg border border-gray-700">
          <h3 className="text-gray-400">Total Value</h3>
          {/* Fixed: was totalValue */}
          <p className="text-2xl font-bold">${portfolio.totalPortfolioValue?.toFixed(2)}</p>
        </div>
        <div className="bg-gray-800 p-6 rounded-lg border border-gray-700">
          <h3 className="text-gray-400">Available Cash</h3>
          {/* Fixed: was availableBalance */}
          <p className="text-2xl font-bold">${portfolio.virtualBalance?.toFixed(2)}</p>
        </div>
      </div>

      <div className="bg-gray-800 rounded-lg border border-gray-700 overflow-hidden">
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-gray-700 border-b border-gray-600">
              <th className="p-4 font-semibold">Asset</th>
              <th className="p-4 font-semibold">Quantity</th>
              <th className="p-4 font-semibold">Average Price</th>
              <th className="p-4 font-semibold">Current Value</th>
            </tr>
          </thead>
          <tbody>
            {portfolio.positions?.map((pos) => (
              <tr key={pos.symbol} className="border-b border-gray-700 hover:bg-gray-750">
                <td className="p-4 font-bold">{pos.symbol}</td>
                <td className="p-4">{pos.quantity}</td>
                {/* Fixed: was averagePurchasePrice */}
                <td className="p-4">${pos.averageBuyPrice?.toFixed(2)}</td>
                <td className="p-4 text-green-400">${(pos.quantity * pos.currentPrice).toFixed(2)}</td>
              </tr>
            ))}
            {portfolio.positions?.length === 0 && (
              <tr>
                <td colSpan={4} className="p-4 text-center text-gray-500">No open positions.</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default PortfolioPage;
