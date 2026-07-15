import { create } from 'zustand';
import type { ViagemDetalhes } from '../types';

interface ReservaState {
  viagemSelecionada: ViagemDetalhes | null;
  assentoSelecionado: number | null;
  setViagemSelecionada: (viagem: ViagemDetalhes) => void;
  setAssentoSelecionado: (assento: number) => void;
  limpar: () => void;
}

export const useReservaStore = create<ReservaState>((set) => ({
  viagemSelecionada: null,
  assentoSelecionado: null,
  setViagemSelecionada: (viagem) => set({ viagemSelecionada: viagem }),
  setAssentoSelecionado: (assento) => set({ assentoSelecionado: assento }),
  limpar: () => set({ viagemSelecionada: null, assentoSelecionado: null }),
}));