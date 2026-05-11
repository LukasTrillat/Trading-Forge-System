import React from 'react';
import { usePortfolio } from '../../../Application/Handlers/usePortfolio';

export const PortfolioPage: React.FC = () => {
  const { portfolio, isLoading } = usePortfolio();

  if (isLoading) return <div className="text-white p-8">Loading...</div>;
  if (!portfolio) return <div className="text-white p-8">No portfolio found.</div>;

  return (
    <div className="p-8 text-white h-full bg-neutral-950">
      <h1 className="text-3xl font-bold mb-6">My Portfolio</h1>
      
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
        <div className="bg-neutral-900 p-6 rounded-lg border border-neutral-800">
          <h3 className="text-neutral-400">Total Value</h3>
          <p className="text-2xl font-bold">${portfolio.totalPortfolioValue?.toFixed(2)}</p>
        </div>
        <div className="bg-neutral-900 p-6 rounded-lg border border-neutral-800">
          <h3 className="text-neutral-400">Available Cash</h3>
          <p className="text-2xl font-bold">${portfolio.virtualBalance?.toFixed(2)}</p>
        </div>
      </div>

      <div className="bg-neutral-900 rounded-lg border border-neutral-800 overflow-hidden">
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-neutral-800 border-b border-neutral-700">
              <th className="p-4 font-semibold">Asset</th>
              <th className="p-4 font-semibold">Quantity</th>
              <th className="p-4 font-semibold">Average Price</th>
              <th className="p-4 font-semibold">Current Value</th>
            </tr>
          </thead>
          <tbody>
            {portfolio.positions?.map((pos) => (
              <tr key={pos.symbol} className="border-b border-neutral-800 hover:bg-neutral-800/50">
                <td className="p-4 font-bold">{pos.symbol}</td>
                <td className="p-4">{pos.quantity}</td>
                <td className="p-4">${pos.averageBuyPrice?.toFixed(2)}</td>
                <td className="p-4 text-green-400">${(pos.quantity * pos.currentPrice).toFixed(2)}</td>
              </tr>
            ))}
            {(!portfolio.positions || portfolio.positions.length === 0) && (
              <tr>
                <td colSpan={4} className="p-4 text-center text-neutral-500">No open positions.</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default PortfolioPage;
