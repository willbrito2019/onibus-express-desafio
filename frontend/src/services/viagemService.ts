import api from './api';
import type { Viagem, ViagemDetalhes } from '../types';

export async function buscarViagens(origem?: string, destino?: string, data?: string): Promise<Viagem[]> {
  const response = await api.get<Viagem[]>('/viagens', {
    params: { origem, destino, data },
  });
  return response.data;
}

export async function obterDetalhesViagem(id: string): Promise<ViagemDetalhes> {
  const response = await api.get<ViagemDetalhes>(`/viagens/${id}`);
  return response.data;
}