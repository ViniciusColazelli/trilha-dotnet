# Projeto 1: encurtador de URL com Redis

> 🚧 Em construção. Este README será completado ao fim do projeto, com a arquitetura e o que aprendi.

ASP.NET Core Minimal API + PostgreSQL (EF Core) + Redis (StackExchange.Redis).

## Como rodar

```bash
# 1. Subir Postgres, Redis e RedisInsight
docker compose up -d

# 2. Restaurar a ferramenta dotnet-ef (versão fixada em dotnet-tools.json) e criar as tabelas
dotnet tool restore
dotnet ef database update --project src/UrlShortener.Api

# 3. Rodar a API (ou abra UrlShortener.slnx no Visual Studio e pressione F5)
dotnet run --project src/UrlShortener.Api

# 4. Rodar os testes
dotnet test
```

Para testar os endpoints: abra a UI do Scalar em http://localhost:5080/scalar ou use o arquivo [`UrlShortener.Api.http`](src/UrlShortener.Api/UrlShortener.Api.http) no Visual Studio.

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/urls` | Cria uma URL curta. Corpo: `{ "url": "https://..." }`. Responde `201` ou `400` |
| `GET` | `/{code}` | Redireciona com `302` para a URL original, ou `404` |

| Serviço | Endereço |
|---------|----------|
| API | http://localhost:5080 |
| Scalar (UI da API) | http://localhost:5080/scalar |
| PostgreSQL | `localhost:5432` (usuário/senha/banco: `urlshortener`) |
| Redis | `localhost:6379` |
| RedisInsight | http://localhost:5540 |
