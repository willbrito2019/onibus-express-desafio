export interface Viagem {
  id: string;
  origem: string;
  destino: string;
  dataHoraPartida: string;
  precoBase: number;
  assentosDisponiveis: number;
}

export interface ViagemDetalhes extends Viagem {
  totalAssentos: number;
  assentosOcupados: number[];
  assentosLivres: number[];
}

export interface CriarReservaRequest {
  nome: string;
  cpf: string;
  email: string;
  dataNascimento: string;
  viagemId: string;
  numeroAssento: number;
}

export interface Reserva {
  codigoReserva: string;
  status: string;
  numeroAssento: number;
  passageiroNome: string;
  viagemId: string;
  dataHoraPartidaViagem: string;
  criadaEm: string;
}