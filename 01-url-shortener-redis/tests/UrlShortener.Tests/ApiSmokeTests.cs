using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace UrlShortener.Tests;

/// <summary>
/// Teste de fumaça: sobe a API inteira em memória (sem abrir porta de rede)
/// e garante que ela inicializa e responde.
/// </summary>
public class ApiSmokeTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Api_inicializa_e_responde()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
