using TraderForge.Application.Common;
using TraderForge.Application.DTOs.Queries;
using TraderForge.Application.DTOs.Responses;
using TraderForge.Domain.Repositories;

namespace TraderForge.Application.Handlers;

public class GetCandlesQueryHandler
{
    private readonly IPriceSnapshotRepository _snapshotRepository;

    private static readonly Dictionary<string, (int Count, int Seconds)> IntervalMap = new()
    {
        ["1m"]  = (150, 60),
        ["5m"]  = (130, 300),
        ["15m"] = (110, 900),
        ["1h"]  = (100, 3600),
        ["4h"]  = (90, 14400),
        ["1d"]  = (60, 86400),
    };

    public GetCandlesQueryHandler(IPriceSnapshotRepository snapshotRepository)
    {
        _snapshotRepository = snapshotRepository;
    }

    public async Task<ResultGeneric<List<CandlestickResponse>>> HandleAsync(GetCandlesQuery query)
    {
        try
        {
            if (!IntervalMap.TryGetValue(query.Interval, out var config))
                return ResultGeneric<List<CandlestickResponse>>.Failure($"Unsupported interval: {query.Interval}");

            var now = DateTime.UtcNow;
            var from = now.AddSeconds(-config.Count * config.Seconds);

            var snapshots = await _snapshotRepository.GetBySymbolAsync(query.Symbol, from, now);
            if (snapshots.Count == 0)
                return ResultGeneric<List<CandlestickResponse>>.Success(new List<CandlestickResponse>());

            var candles = AggregateCandles(snapshots, config.Seconds, now, config.Count);
            return ResultGeneric<List<CandlestickResponse>>.Success(candles);
        }
        catch (Exception ex)
        {
            return ResultGeneric<List<CandlestickResponse>>.Failure(ex.Message);
        }
    }

    private static List<CandlestickResponse> AggregateCandles(
        List<Domain.Entities.PriceSnapshot> snapshots, int windowSeconds, DateTime now, int expectedCount)
    {
        var candles = new List<CandlestickResponse>();
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        for (int i = expectedCount; i >= 0; i--)
        {
            var windowEnd = now.AddSeconds(-i * windowSeconds);
            var windowStart = windowEnd.AddSeconds(-windowSeconds);

            var inWindow = snapshots
                .Where(s => s.RecordedAt >= windowStart && s.RecordedAt < windowEnd)
                .ToList();

            if (inWindow.Count == 0 && candles.Count > 0)
            {
                var prev = candles[^1];
                candles.Add(new CandlestickResponse
                {
                    Time = (long)(windowEnd - epoch).TotalSeconds,
                    Open = prev.Close,
                    High = prev.Close,
                    Low = prev.Close,
                    Close = prev.Close,
                });
                continue;
            }

            if (inWindow.Count == 0)
                continue;

            var first = inWindow[0];
            var last = inWindow[^1];
            candles.Add(new CandlestickResponse
            {
                Time = (long)(windowEnd - epoch).TotalSeconds,
                Open = first.Price,
                High = inWindow.Max(s => s.Price),
                Low = inWindow.Min(s => s.Price),
                Close = last.Price,
            });
        }

        return candles;
    }
}
