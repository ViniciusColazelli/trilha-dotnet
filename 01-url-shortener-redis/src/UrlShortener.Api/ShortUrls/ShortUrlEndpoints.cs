using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using UrlShortener.Api.Data;

namespace UrlShortener.Api.ShortUrls;

public static class ShortUrlEndpoints
{
    private const int MaxCodeGenerationAttempts = 3;

    public static IEndpointRouteBuilder MapShortUrlEndpoints(this IEndpointRouteBuilder app)
    {
        var urls = app.MapGroup("/urls").WithTags("URLs");

        urls.MapPost("/", CreateAsync)
            .WithName("CreateShortUrl")
            .WithSummary("Cria uma URL curta");

        // maxlength: rejeita códigos absurdamente longos antes de chegar ao banco.
        // Rotas com segmentos literais (/urls, /scalar, /openapi) têm prioridade sobre este parâmetro.
        app.MapGet($"/{{code:maxlength({ShortUrl.CodeMaxLength})}}", RedirectAsync)
            .WithName("RedirectToOriginalUrl")
            .WithTags("Redirect")
            .WithSummary("Redireciona (302) para a URL original");

        return app;
    }

    private static async Task<Results<Created<ShortUrlResponse>, ValidationProblem>> CreateAsync(
        CreateShortUrlRequest request,
        AppDbContext db,
        IShortCodeGenerator codeGenerator,
        TimeProvider clock,
        HttpRequest httpRequest,
        CancellationToken ct)
    {
        if (!UrlValidator.IsValidHttpUrl(request.Url))
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                ["url"] = [$"Informe uma URL absoluta http ou https com até {ShortUrl.OriginalUrlMaxLength} caracteres."]
            });
        }

        var shortUrl = new ShortUrl
        {
            Code = codeGenerator.Generate(),
            OriginalUrl = request.Url,
            CreatedAt = clock.GetUtcNow()
        };
        db.ShortUrls.Add(shortUrl);

        // Colisão de código: o índice único do banco rejeita o INSERT e tentamos com outro código.
        // Checar antes com "SELECT ... WHERE code = x" não resolveria: duas requisições simultâneas
        // poderiam passar pela checagem ao mesmo tempo (race condition).
        for (var attempt = 1; ; attempt++)
        {
            try
            {
                await db.SaveChangesAsync(ct);
                break;
            }
            catch (DbUpdateException ex) when (IsUniqueViolation(ex) && attempt < MaxCodeGenerationAttempts)
            {
                shortUrl.Code = codeGenerator.Generate();
            }
        }

        var link = $"{httpRequest.Scheme}://{httpRequest.Host}/{shortUrl.Code}";
        var response = new ShortUrlResponse(shortUrl.Code, link, shortUrl.OriginalUrl, shortUrl.CreatedAt);

        return TypedResults.Created(link, response);
    }

    private static async Task<Results<RedirectHttpResult, NotFound>> RedirectAsync(
        string code,
        AppDbContext db,
        CancellationToken ct)
    {
        // AsNoTracking + Select: só leitura, busca apenas a coluna necessária.
        var originalUrl = await db.ShortUrls
            .AsNoTracking()
            .Where(x => x.Code == code)
            .Select(x => x.OriginalUrl)
            .FirstOrDefaultAsync(ct);

        if (originalUrl is null)
        {
            return TypedResults.NotFound();
        }

        // 302 (temporário), e não 301: o navegador não cacheia, então todo clique passa pela API.
        // Isso é essencial para o contador de cliques (Etapa 4) e o ranking (Etapa 6).
        return TypedResults.Redirect(originalUrl, permanent: false);
    }

    private static bool IsUniqueViolation(DbUpdateException ex) =>
        ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation };
}
