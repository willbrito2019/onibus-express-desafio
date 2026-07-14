namespace OnibusExpress.Api.Dtos;

public record RotaResponse(Guid Id, string Origem, string Destino, TimeSpan DuracaoEstimada);

public record ViagemResumoResponse(Guid Id, string Origem, string Destino, DateTime DataHoraPartida, decimal PrecoBase, int AssentosDisponiveis);

public record ViagemDetalhesResponse(
    Guid Id, string Origem, string Destino, DateTime DataHoraPartida, decimal PrecoBase,
    int TotalAssentos, List<int> AssentosOcupados, List<int> AssentosLivres);

public record CriarReservaRequest(
    string Nome, string Cpf, string Email, DateOnly DataNascimento,
    Guid ViagemId, int NumeroAssento);

public record ReservaResponse(
    string CodigoReserva, string Status, int NumeroAssento,
    string PassageiroNome, Guid ViagemId, DateTime DataHoraPartidaViagem, DateTime CriadaEm);