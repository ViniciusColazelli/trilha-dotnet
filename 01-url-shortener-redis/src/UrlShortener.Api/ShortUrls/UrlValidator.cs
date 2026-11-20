namespace UrlShortener.Api.ShortUrls;

public static class UrlValidator
{
    /// <summary>
    /// Aceita só URLs absolutas http/https. Bloquear outros esquemas (javascript:, file:, data:...)
    /// evita que o encurtador vire ferramenta para redirecionar vítimas a conteúdo malicioso.
    /// </summary>
    public static bool IsValidHttpUrl(string? url) =>
        !string.IsNullOrWhiteSpace(url)
        && url.Length <= ShortUrl.OriginalUrlMaxLength
        && Uri.TryCreate(url, UriKind.Absolute, out var uri)
        && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
        && !string.IsNullOrEmpty(uri.Host);
}
