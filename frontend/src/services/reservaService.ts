import api from './api';
import type { CriarReservaRequest, Reserva } from '../types';

export async function criarReserva(request: CriarReservaRequest): Promise<Reserva> {
  const response = await api.post<Reserva>('/reservas', request);
  return response.data;
}

export async function consultarReserva(codigo: string): Promise<Reserva> {
  const response = await api.get<Reserva>(`/reservas/${codigo}`);
  return response.data;
}

export async function cancelarReserva(codigo: string): Promise<void> {
  await api.delete(`/reservas/${codigo}`);
}