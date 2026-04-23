using Microsoft.EntityFrameworkCore;
using TraderForge.Application.Handlers;
using TraderForge.Infrastructure.Persistence;
using TraderForge.Domain.Interfaces;
using TraderForge.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();

builder.Services.AddTransient<GetMarketPricesQueryHandler>();

builder.Services.AddHttpClient<IMarketDataProvider, BinanceMarketService>();
builder.Services.AddSingleton<IMarketService, CacheMarketService>();
builder.Services.AddHostedService<BackgroundMarketPollingService>();

// Register database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

// Map your API endpoints or controllers here
app.Run();
