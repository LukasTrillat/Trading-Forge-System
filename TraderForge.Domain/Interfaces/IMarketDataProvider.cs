<<<<<<< HEAD
﻿namespace TraderForge.Domain.Interfaces;
=======
namespace TraderForge.Domain.Interfaces;
>>>>>>> 1f749f1f578782d90b80008a7c1162a3cc9a80ba

public interface IMarketDataProvider
{
    Task<Dictionary<string, decimal>> GetPricesAsync();
}