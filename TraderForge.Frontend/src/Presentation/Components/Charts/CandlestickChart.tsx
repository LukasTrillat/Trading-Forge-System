import React, { useMemo } from 'react';
import { useMarketData } from '../../../Application/Handlers/useMarketData';

interface CandlestickChartProps {
  symbol: string;
}

export const CandlestickChart: React.FC<CandlestickChartProps> = ({ symbol }) => {
  const { candles, isLoading } = useMarketData();

  const chartData = useMemo(() => {
    if (!candles || candles.length === 0) return null;

    const maxPrice = Math.max(...candles.map((c) => c.high));
    const minPrice = Math.min(...candles.map((c) => c.low));
    const range = maxPrice - minPrice;

    return { candles, maxPrice, minPrice, range };
  }, [candles]);

  if (isLoading || !chartData) {
    return (
      <div className="h-full w-full flex items-center justify-center bg-neutral-950 text-neutral-500 rounded-lg border border-neutral-800">
        <div className="flex flex-col items-center gap-2">
          <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-500"></div>
          <span>Loading chart data for {symbol}...</span>
        </div>
      </div>
    );
  }

  return (
    <div className="h-full w-full bg-neutral-950 p-4 rounded-lg border border-neutral-800">
      <div className="relative h-full flex items-end gap-1">
        {chartData.candles.map((candle, i) => {
          const isUp = candle.close >= candle.open;
          const height = ((Math.abs(candle.close - candle.open) / chartData.range) * 100) + 2;
          const bottom = ((Math.min(candle.close, candle.open) - chartData.minPrice) / chartData.range) * 100;
          
          return (
            <div key={i} className="flex-1 flex flex-col items-center group relative h-full">
              <div 
                className={`w-full rounded-sm transition-all ${isUp ? 'bg-green-500 hover:bg-green-400' : 'bg-red-500 hover:bg-red-400'}`}
                style={{ height: `${height}%`, marginBottom: `${bottom}%` }}
              />
              <div className="absolute bottom-full mb-2 hidden group-hover:block bg-neutral-800 text-xs p-2 rounded shadow-xl z-10 border border-neutral-700 whitespace-nowrap">
                Price: ${candle.close}
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
};
