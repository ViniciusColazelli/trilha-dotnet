namespace UrlShortener.Api.ShortUrls;

/// <summary>Gera o código curto de uma nova URL.</summary>
public interface IShortCodeGenerator
{
    string Generate();
}

/// <summary>
/// TEMPORÁRIO: pega 7 caracteres de um GUID em hexadecimal.
/// Hexadecimal só usa 16 símbolos (0-9, a-f), então 7 caracteres dão 16^7 ≈ 268 milhões de combinações.
/// Na Etapa 2 você vai substituir isto por um gerador Base62 (62 símbolos: 62^7 ≈ 3,5 trilhões).
/// </summary>
public sealed class TemporaryShortCodeGenerator : IShortCodeGenerator
{
    public string Generate() => Guid.NewGuid().ToString("N")[..7];
}
