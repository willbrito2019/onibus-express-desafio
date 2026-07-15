# OniBus Express — Desafio Técnico Backend (.NET)

Sistema de busca e reserva de passagens de ônibus, desafio técnico da OniBus Express.

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
- **Api**: Controllers, DTOs, configuração de DI, CORS.

### Frontend — estrutura por responsabilidade

- **pages/**: as 4 telas do fluxo (Busca, Seleção de Assento, Confirmação, Consulta).
- **services/**: chamadas HTTP à API (Axios).
- **store/**: estado compartilhado entre telas (viagem e assento selecionados) via Zustand.
- **types/**: contratos TypeScript espelhando os DTOs do backend.

## Decisões técnicas

- **PostgreSQL em vez de SQL Server**: imagem menor, sobe mais rápido em Docker, evita questões de licença/EULA em container.
- **SQLite para testes de integração** (não usado ainda neste MVP — testes atuais são unitários).
- **Optei por Controllers chamando repositórios diretamente.
- **CPF armazenado sem máscara**: normalizado (só dígitos) antes de persistir, evitando duplicidade de passageiro por formatação diferente do mesmo CPF.
- **Geração de código de reserva em 2 camadas**: o `GeradorCodigoReserva` tenta gerar e verifica existência (retry até 10x); o índice único no banco (`CodigoReserva`) é a garantia final contra colisão.
- **Campo Data de Nascimento no formulário do frontend**: o desafio pede Nome/CPF/E-mail na Tela 3, mas o backend exige `dataNascimento` para criar o `Passageiro`. Adicionei o campo ao formulário para fechar a integração ponta a ponta.
- **Zustand em vez de Context API**.

## Como rodar

### Opção A — Docker (recomendado, sobe tudo com 1 comando)

\`\`\`bash
docker-compose up --build
\`\`\`

Isso sobe banco (Postgres), API e frontend juntos. Migration e seed de dados de teste são aplicados automaticamente na inicialização da API.

- Frontend: http://localhost:3000
- API/Swagger: http://localhost:5000/swagger

### Opção B — Desenvolvimento local (backend e frontend separados)

\`\`\`bash
# sobe só o banco
docker-compose up -d db

# backend
dotnet run --project src/OnibusExpress.Api/OnibusExpress.Api.csproj

# frontend (em outro terminal)
cd frontend
npm install
npm run dev
\`\`\`

- Frontend: http://localhost:5173
- API/Swagger: http://localhost:5064/swagger

## Testes

### Backend
\`\`\`bash
dotnet test
\`\`\`
Cobertura: validação de CPF, regra de assento já ocupado, regra de cancelamento (incluindo edge case do limite exato de 2h), regra de viagem já realizada, geração de código único.

### Frontend
\`\`\`bash
cd frontend
npm run test
\`\`\`
Cobertura: componente de busca (preenchimento e resultado), mapa de assentos (seleção e bloqueio de ocupados), validação do formulário de passageiro.

## Endpoints da API

| Método | Rota | Descrição |
|---|---|---|
| GET | /rotas | Lista todas as rotas |
| GET | /viagens?origem=&destino=&data= | Busca viagens |
| GET | /viagens/{id} | Detalhes da viagem (assentos livres/ocupados) |
| POST | /reservas | Cria reserva |
| GET | /reservas/{codigo} | Consulta reserva |
| DELETE | /reservas/{codigo} | Cancela reserva |

## Fluxo do usuário (Frontend)

1. **Busca de Passagens** — formulário de origem/destino/data, lista viagens disponíveis.
2. **Seleção de Assento** — mapa visual de assentos (livre/ocupado/selecionado).
3. **Dados do Passageiro e Confirmação** — formulário validado, resumo da compra, código de reserva ao final.
4. **Consulta de Reserva** (bônus) — busca por código, exibe detalhes, permite cancelamento.
