namespace UrlShortener.Api.ShortUrls;

// Contratos (DTOs) da API: o formato do JSON que entra e sai.
// Separados da entidade para que mudanças no banco não quebrem quem consome a API.

public record CreateShortUrlRequest(string Url);

public record ShortUrlResponse(string Code, string ShortUrl, string OriginalUrl, DateTimeOffset CreatedAt);
