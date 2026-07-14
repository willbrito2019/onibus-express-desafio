# OniBus Express — Desafio Técnico Backend (.NET)

Sistema de busca e reserva de passagens de ônibus, desenvolvido como MVP para o desafio técnico da OniBus Express.

## Stack

- .NET 9 (ASP.NET Core Web API)
- Entity Framework Core 9 + PostgreSQL
- xUnit + Shouldly (testes unitários)
- Docker + docker-compose

## Arquitetura

Clean Architecture com 4 camadas:

- **Domain**: entidades (`Rota`, `Viagem`, `Passageiro`, `Reserva`) e regras de negócio invariantes (ex: cancelamento só até 2h antes da partida). Sem dependência de nenhuma outra camada.
- **Application**: casos de uso auxiliares que não são invariantes de entidade — `CpfValidator` (validação de dígito verificador) e `GeradorCodigoReserva`.
- **Infrastructure**: `AppDbContext` (EF Core), repositórios (implementação das interfaces definidas no Domain).
- **Api**: Controllers, DTOs, configuração de DI.

### Decisão: interfaces de repositório no Domain

As interfaces de repositório (`IRotaRepository`, `IViagemRepository`, etc.) ficam no Domain — o domínio declara o contrato de persistência que precisa, e a Infrastructure implementa. 

### Decisão: Postgres em vez de SQL Server

Optei por PostgreSQL por ser mais leve em Docker 

### Decisão: SQLite para testes de integração

Testes de integração usam SQLite in-memory em vez do Postgres real, para isolamento e velocidade — não dependem do container estar de pé para rodar.

### Decisão: geração de código de reserva

O `GeradorCodigoReserva` gera um código no formato `ABC-12345`, mas a garantia de unicidade 
real é responsabilidade em duas camadas: o Controller tenta gerar e verificar existência (retry até 10x), 
e o banco garante com índice único em `CodigoReserva` como última linha de defesa.

## Como rodar

### Com Docker (completo)

\`\`\`bash
docker-compose up --build
\`\`\`

Sobe API + banco. Migration e seed de dados de teste são aplicados automaticamente na inicialização.

### Local (desenvolvimento)

\`\`\`bash
docker-compose up -d db
dotnet run --project src/OnibusExpress.Api/OnibusExpress.Api.csproj
\`\`\`

Swagger disponível em `http://localhost:5064/swagger` (a porta pode variar, verifique o log do console).

## Testes

\`\`\`bash
dotnet test
\`\`\`

Cobertura: validação de CPF, regra de assento já ocupado, regra de cancelamento (incluindo edge case do limite exato de 2h), geração de código único.

## Endpoints

| Método | Rota | Descrição |
|---|---|---|
| GET | /rotas | Lista todas as rotas |
| GET | /viagens?origem=&destino=&data= | Busca viagens |
| GET | /viagens/{id} | Detalhes da viagem (assentos livres/ocupados) |
| POST | /reservas | Cria reserva |
| GET | /reservas/{codigo} | Consulta reserva |
| DELETE | /reservas/{codigo} | Cancela reserva |

