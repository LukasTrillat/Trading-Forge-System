import React, { useState } from 'react';
import { useTrading } from '../../../Application/Handlers/useTrading';

interface ExecutionPanelProps {
  selectedSymbol: string | null;
  availableBalance: number;
}

export const ExecutionPanel: React.FC<ExecutionPanelProps> = ({ selectedSymbol, availableBalance }) => {
  const { placeOrder, isLoading, error } = useTrading();
  const [side, setSide] = useState<'Buy' | 'Sell'>('Buy');
  const [quantity, setQuantity] = useState<number>(0);
  const [orderType, setOrderType] = useState<'Market' | 'Limit'>('Market');

  const handleExecute = async () => {
    if (!selectedSymbol || quantity <= 0) return;
    
    await placeOrder({
      symbol: selectedSymbol,
      side,
      quantity,
      type: orderType,
    });
  };

  if (!selectedSymbol) {
    return (
      <div className="h-full flex items-center justify-center text-neutral-500 text-sm italic p-4 text-center">
        Select an asset from the dashboard to enable trading controls
      </div>
    );
  }

  return (
    <div className="flex flex-col gap-6">
      <div className="flex bg-neutral-950 p-1 rounded-lg border border-neutral-800">
        <button
          onClick={() => setSide('Buy')}
          className={`flex-1 py-2 rounded-md text-xs font-bold transition-all ${
            side === 'Buy' ? 'bg-emerald-600 text-white' : 'text-neutral-500 hover:text-white'
          }`}
        >
          BUY
        </button>
        <button
          onClick={() => setSide('Sell')}
          className={`flex-1 py-2 rounded-md text-xs font-bold transition-all ${
            side === 'Sell' ? 'bg-red-600 text-white' : 'text-neutral-500 hover:text-white'
          }`}
        >
          SELL
        </button>
      </div>

      <div className="space-y-4">
        <div>
          <label className="text-[10px] font-black text-neutral-500 uppercase tracking-widest block mb-2">Order Type</label>
          <select 
            value={orderType}
            onChange={(e) => setOrderType(e.target.value as any)}
            className="w-full bg-neutral-950 border border-neutral-800 rounded-lg px-4 py-2 text-sm text-white outline-none focus:border-blue-500 cursor-pointer"
          >
            <option value="Market">Market Order</option>
            <option value="Limit">Limit Order</option>
          </select>
        </div>

        <div>
          <label className="text-[10px] font-black text-neutral-500 uppercase tracking-widest block mb-2">Quantity ({selectedSymbol})</label>
          <input
            type="number"
            value={quantity || ''}
            onChange={(e) => setQuantity(parseFloat(e.target.value))}
            placeholder="0.00"
            className="w-full bg-neutral-950 border border-neutral-800 rounded-lg px-4 py-2 text-sm text-white outline-none focus:border-emerald-500 font-mono"
          />
        </div>
      </div>

      <div className="bg-neutral-950/50 p-4 rounded-lg border border-neutral-800 space-y-2">
        <div className="flex justify-between text-xs">
          <span className="text-neutral-500">Est. Total</span>
          <span className="text-white font-mono">${(currentPrice * quantity || 0).toFixed(2)}</span>
        </div>
        <div className="flex justify-between text-xs">
          <span className="text-neutral-500">Available Power</span>
          <span className="text-blue-400 font-mono">${availableBalance.toLocaleString(undefined, { minimumFractionDigits: 2 })}</span>
        </div>
      </div>

      <button
        disabled={isLoading || quantity <= 0}
        onClick={handleExecute}
        className={`w-full py-4 rounded-xl font-black text-sm shadow-xl transition-all active:scale-95 ${
          side === 'Buy' 
            ? 'bg-emerald-600 hover:bg-emerald-500 shadow-emerald-900/20' 
            : 'bg-red-600 hover:bg-red-500 shadow-red-900/20'
        } disabled:opacity-50 disabled:cursor-not-allowed text-white`}
      >
        {isLoading ? 'EXECUTING...' : `CONFIRM ${side} ${selectedSymbol}`}
      </button>

      {error && (
        <div className="bg-red-500/10 border border-red-500/20 p-3 rounded-lg">
          <p className="text-red-400 text-[10px] text-center font-bold uppercase tracking-tight">{error}</p>
        </div>
      )}
    </div>
  );
};
