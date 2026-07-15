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

## Frontend — estrutura por responsabilidade

- **pages/**: as 4 telas do fluxo (Busca, Seleção de Assento, Confirmação, Consulta).
- **services/**: chamadas HTTP à API (Axios).
- **store/**: estado compartilhado entre telas (viagem e assento selecionados) via Zustand.
- **types/**: contratos TypeScript espelhando os DTOs do backend.

## Decisões técnicas

- **PostgreSQL em vez de SQL Server**: imagem menor, sobe mais rápido em Docker, evita questões de licença/EULA em container.
- **SQLite para testes de integração** (não usado ainda neste MVP — testes atuais são unitários).
- **Optei por Controllers chamando repositórios diretamente.**
- **CPF armazenado sem máscara**: normalizado (somente dígitos) antes de persistir, evitando duplicidade de passageiro por formatações diferentes do mesmo CPF.
- **Geração de código de reserva em 2 camadas**: o `GeradorCodigoReserva` tenta gerar e verifica existência (retry até 10x); o índice único no banco (`CodigoReserva`) é a garantia final contra colisões.
- **Campo Data de Nascimento no frontend**: o desafio pede Nome/CPF/E-mail na Tela 3, mas o backend exige `dataNascimento` para criar o `Passageiro`. Adicionei o campo ao formulário para fechar a integração ponta a ponta.
- **Zustand em vez de Context API**: solução mais simples para compartilhar estado entre as telas.

## Como rodar

### Opção A — Docker (recomendado)

```bash
docker-compose up --build
```

Isso sobe o banco (PostgreSQL), a API e o frontend juntos. As migrations e o seed de dados são executados automaticamente na inicialização da API.

- Frontend: http://localhost:3000
- API/Swagger: http://localhost:5000/swagger

### Opção B — Desenvolvimento local (backend e frontend separados)

#### 1. Subir apenas o banco

```bash
docker-compose up -d db
```

#### 2. Backend

```bash
dotnet run --project src/OnibusExpress.Api/OnibusExpress.Api.csproj
```

#### 3. Frontend (em outro terminal)

```bash
cd frontend
npm install
npm run dev
```

- Frontend: http://localhost:5173
- API/Swagger: http://localhost:5064/swagger

## Testes

### Backend

```bash
dotnet test
```

Cobertura:

- Validação de CPF
- Regra de assento já ocupado
- Regra de cancelamento (incluindo o limite exato de 2 horas)
- Regra de viagem já realizada
- Geração de código único de reserva

### Frontend

```bash
cd frontend
npm run test
```

Cobertura:

- Busca de viagens (preenchimento do formulário e listagem)
- Mapa de assentos (seleção e bloqueio de assentos ocupados)
- Validação do formulário de passageiro

## Endpoints da API

| Método | Endpoint | Descrição |
| ------- | -------- | --------- |
| GET | `/rotas` | Lista todas as rotas |
| GET | `/viagens?origem=&destino=&data=` | Busca viagens |
| GET | `/viagens/{id}` | Detalhes da viagem (assentos livres e ocupados) |
| POST | `/reservas` | Cria uma reserva |
| GET | `/reservas/{codigo}` | Consulta uma reserva |
| DELETE | `/reservas/{codigo}` | Cancela uma reserva |

## Fluxo do usuário

1. **Busca de Passagens** — formulário de origem, destino e data, retornando as viagens disponíveis.
2. **Seleção de Assento** — mapa visual de assentos (livre, ocupado e selecionado).
3. **Dados do Passageiro e Confirmação** — preenchimento dos dados, resumo da compra e geração do código de reserva.
4. **Consulta de Reserva (bônus)** — busca por código, exibição dos detalhes e possibilidade de cancelamento.
