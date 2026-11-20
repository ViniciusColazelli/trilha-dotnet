# Trilha .NET: sistemas distribuídos na prática

Quatro projetos pequenos e independentes para aprender, na prática, **Redis**, **RabbitMQ**, **load balancing** e **Kubernetes** com C#, .NET 10 (LTS) e ASP.NET Core.

| # | Projeto | Foco | Status |
|---|---------|------|--------|
| 1 | [Encurtador de URL](01-url-shortener-redis/) | Redis: cache-aside, `INCR`, rate limiting, Sorted Sets | 🚧 em andamento |
| 2 | [Sistema de pedidos](02-pedidos-rabbitmq/) | RabbitMQ: exchanges, filas, DLQ, MassTransit, sagas | ⏳ |
| 3 | [API com load balancer](03-api-load-balancer/) | Nginx, YARP, health checks, SignalR + backplane, k6 | ⏳ |
| 4 | [Kubernetes](04-kubernetes/) | Deployments, Ingress, Helm, probes, HPA, CI/CD | ⏳ |

Depois dos quatro: um **mini e-commerce** juntando tudo.

## Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download) (a versão está fixada em [`global.json`](global.json))
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (projetos 1 a 3 sobem as dependências com Docker Compose)
- Visual Studio 2026 ou qualquer editor; cada projeto tem sua própria solution `.slnx`

## Organização

Cada pasta é um projeto independente, com sua própria solution, `docker-compose.yml` e README explicando a arquitetura, como rodar e o que aprendi.
