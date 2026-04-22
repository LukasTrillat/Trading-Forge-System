using Microsoft.EntityFrameworkCore;
using TraderForge.Application.Handlers;
using TraderForge.Domain.Interfaces;
using TraderForge.Infrastructure;
using TraderForge.Infrastructure.Persistence;
using TraderForge.Infrastructure.Repositories;
using TraderForge.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// -- Configure ASP.NET Core Identity -- //
builder.Services.AddIdentityCore<Account>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<ApplicationDbContext>();


// - Register Services for the DEPENDENCY INJECTION - //
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<ITraderRepository, TraderRepository>();
builder.Services.AddScoped<IAdministratorRepository, AdministratorRepository>();
builder.Services.AddTransient<RegisterTraderCommandHandler>(); 
builder.Services.AddTransient<LoginTraderQueryHandler>(); 
builder.Services.AddOpenApi();
builder.Services.AddControllers();


// -- Register database context -- //
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// -- Initialize app -- //
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


// Map your API endpoints or controllers here
app.MapControllers();

app.Run();
