import { useEffect, useRef, memo, useCallback } from 'react';
import { createChart, ColorType, CrosshairMode, CandlestickSeries } from 'lightweight-charts';
import type { UTCTimestamp, IChartApi, Time } from 'lightweight-charts';
import type { CandlestickBar } from '../../../Domain/Entities/Asset';

interface CandlestickChartProps {
  candles: CandlestickBar[];
  symbol: string;
}

export const CandlestickChart = memo(function CandlestickChart({ candles, symbol }: CandlestickChartProps) {
  const containerRef = useRef<HTMLDivElement>(null);
  const chartRef = useRef<IChartApi | null>(null);

  useEffect(() => {
    if (!containerRef.current || candles.length === 0) return;

    const chart = createChart(containerRef.current, {
      autoSize: true,
      layout: {
        background: { type: ColorType.Solid, color: '#0a0a0b' },
        textColor: '#9ca3af',
      },
      grid: {
        vertLines: { color: '#1a1a1f' },
        horzLines: { color: '#1a1a1f' },
      },
      crosshair: { mode: CrosshairMode.Normal },
      rightPriceScale: { borderColor: '#2e303a' },
      timeScale: { borderColor: '#2e303a', timeVisible: true, secondsVisible: false },
    });

    chartRef.current = chart;

    const series = chart.addSeries(CandlestickSeries, {
      upColor: '#10b981',
      downColor: '#ef4444',
      borderVisible: false,
      wickUpColor: '#10b981',
      wickDownColor: '#ef4444',
    });

    series.setData(candles.map((c) => ({ ...c, time: c.time as UTCTimestamp })));

    const times = candles.map(c => c.time);
    const from = Math.min(...times);
    const to = Math.max(...times);
    const padding = Math.max((to - from) * 5, 300);
    chart.timeScale().setVisibleRange({
      from: (from - padding) as Time,
      to: (to + padding) as Time,
    });

    return () => {
      chartRef.current = null;
      chart.remove();
    };
  }, [candles, symbol]);

  const zoomIn = useCallback(() => {
    const chart = chartRef.current;
    if (!chart) return;
    const range = chart.timeScale().getVisibleRange();
    if (!range) return;
    const from = Number(range.from);
    const to = Number(range.to);
    const mid = (from + to) / 2;
    const halfSpan = (to - from) / 2 * 0.7;
    chart.timeScale().setVisibleRange({
      from: (mid - halfSpan) as Time,
      to: (mid + halfSpan) as Time,
    });
  }, []);

  const zoomOut = useCallback(() => {
    const chart = chartRef.current;
    if (!chart) return;
    const range = chart.timeScale().getVisibleRange();
    if (!range) return;
    const from = Number(range.from);
    const to = Number(range.to);
    const mid = (from + to) / 2;
    const halfSpan = (to - from) / 2 / 0.7;
    chart.timeScale().setVisibleRange({
      from: (mid - halfSpan) as Time,
      to: (mid + halfSpan) as Time,
    });
  }, []);

  const fitContent = useCallback(() => {
    chartRef.current?.timeScale().fitContent();
  }, []);

  if (candles.length === 0) {
    return (
      <div className="w-full h-full flex items-center justify-center text-neutral-600 text-sm">
        Select an asset to view the chart
      </div>
    );
  }

  return (
    <div className="relative w-full h-full">
      <div ref={containerRef} className="w-full h-full" />
      <div className="absolute top-2 right-2 flex gap-1">
        <button
          onClick={zoomIn}
          className="w-7 h-7 flex items-center justify-center bg-neutral-900/80 border border-neutral-700 rounded text-neutral-400 hover:text-neutral-200 text-xs font-mono transition-colors"
          title="Zoom in"
        >
          +
        </button>
        <button
          onClick={zoomOut}
          className="w-7 h-7 flex items-center justify-center bg-neutral-900/80 border border-neutral-700 rounded text-neutral-400 hover:text-neutral-200 text-xs font-mono transition-colors"
          title="Zoom out"
        >
          −
        </button>
        <button
          onClick={fitContent}
          className="w-7 h-7 flex items-center justify-center bg-neutral-900/80 border border-neutral-700 rounded text-neutral-400 hover:text-neutral-200 text-xs font-mono transition-colors"
          title="Fit to screen"
        >
          ⟷
        </button>
      </div>
    </div>
  );
});
