using Microsoft.EntityFrameworkCore;
using TraderForge.Application.Handlers;
using TraderForge.Infrastructure.Persistence;
using TraderForge.Domain.Interfaces;
using TraderForge.Domain.Repositories;
using TraderForge.Infrastructure.Repositories;
using TraderForge.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();

builder.Services.AddTransient<GetMarketPricesQueryHandler>();

builder.Services.AddHttpClient<IMarketDataProvider, BinanceMarketProvider>();
builder.Services.AddSingleton<IMarketService, CachedMarketService>();
builder.Services.AddHostedService<BackgroundMarketPollingService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IMarketAssetRepository, MarketAssetRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();