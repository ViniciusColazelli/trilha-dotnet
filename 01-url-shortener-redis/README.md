# Projeto 1: encurtador de URL com Redis

> 🚧 Em construção. Este README será completado ao fim do projeto, com a arquitetura e o que aprendi.

ASP.NET Core Minimal API + PostgreSQL (EF Core) + Redis (StackExchange.Redis).

## Como rodar

```bash
# 1. Subir Postgres, Redis e RedisInsight
docker compose up -d

# 2. Rodar a API (ou abra UrlShortener.slnx no Visual Studio e pressione F5)
dotnet run --project src/UrlShortener.Api

# 3. Rodar os testes
dotnet test
```

| Serviço | Endereço |
|---------|----------|
| API | http://localhost:5080 |
| PostgreSQL | `localhost:5432` (usuário/senha/banco: `urlshortener`) |
| Redis | `localhost:6379` |
| RedisInsight | http://localhost:5540 |
