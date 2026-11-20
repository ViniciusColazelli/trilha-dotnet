using UrlShortener.Api.ShortUrls;

namespace UrlShortener.Tests.ShortUrls;

public class UrlValidatorTests
{
    [Theory]
    [InlineData("https://example.com")]
    [InlineData("http://example.com")]
    [InlineData("https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-10.0#routing")]
    [InlineData("https://localhost:5080/path")]
    public void Aceita_urls_http_e_https_absolutas(string url)
    {
        Assert.True(UrlValidator.IsValidHttpUrl(url));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("example.com")]               // sem esquema: relativa
    [InlineData("/caminho/relativo")]
    [InlineData("ftp://example.com/arquivo")]
    [InlineData("javascript:alert(1)")]       // XSS via redirect
    [InlineData("file:///C:/Windows/win.ini")]
    [InlineData("data:text/html,<script>alert(1)</script>")]
    public void Rejeita_urls_invalidas_ou_com_esquema_nao_permitido(string? url)
    {
        Assert.False(UrlValidator.IsValidHttpUrl(url));
    }

    [Fact]
    public void Rejeita_url_acima_do_tamanho_maximo()
    {
        var url = "https://example.com/" + new string('a', ShortUrl.OriginalUrlMaxLength);

        Assert.False(UrlValidator.IsValidHttpUrl(url));
    }
}
