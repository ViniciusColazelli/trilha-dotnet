using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using UrlShortener.Api.Data;
using UrlShortener.Api.ShortUrls;

var builder = WebApplication.CreateBuilder(args);

// ---------- Serviços (injeção de dependência) ----------

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

var postgresConnectionString = builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException("Connection string 'Postgres' não configurada.");

builder.Services.AddDbContext<AppDbContext>(options => options
    .UseNpgsql(postgresConnectionString)
    // Tabelas/colunas em snake_case (short_urls, original_url), o padrão no PostgreSQL.
    .UseSnakeCaseNamingConvention());

// TimeProvider em vez de DateTimeOffset.UtcNow direto: permite "congelar o relógio" nos testes.
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<IShortCodeGenerator, TemporaryShortCodeGenerator>();

var app = builder.Build();

// ---------- Pipeline HTTP e endpoints ----------

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();               // documento OpenAPI em /openapi/v1.json
    app.MapScalarApiReference();    // UI para testar a API em /scalar
}

app.MapGet("/", () => "URL Shortener API").ExcludeFromDescription();
app.MapShortUrlEndpoints();

app.Run();
