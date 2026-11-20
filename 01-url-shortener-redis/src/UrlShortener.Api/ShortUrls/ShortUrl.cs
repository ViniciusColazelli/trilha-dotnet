namespace UrlShortener.Api.ShortUrls;

/// <summary>
/// Uma URL encurtada. O PostgreSQL é a fonte da verdade; o Redis (a partir da Etapa 3) é só cache.
/// </summary>
public class ShortUrl
{
    public const int CodeMaxLength = 16;
    public const int OriginalUrlMaxLength = 2048;

    public long Id { get; set; }

    /// <summary>Código curto usado na URL: https://host/{Code}. Único no banco.</summary>
    public required string Code { get; set; }

    public required string OriginalUrl { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
