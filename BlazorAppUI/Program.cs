using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 1. Fetch Connection String
var connectionString = builder.Configuration.GetConnectionString("Supabase")
    ?? throw new InvalidOperationException("Connection string 'Supabase' was not found.");

// 2. Register DbContextFactory for EF Core
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 3. Register DataAccess & Business Logic layers
builder.Services.AddScoped<IProductRepository, ProductRepositoryEF>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<BlazorAppUI.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();