import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { BrowserRouter } from 'react-router-dom';
import { describe, it, expect, beforeEach } from 'vitest';
import ConfirmacaoReserva from './ConfirmacaoReserva';
import { useReservaStore } from '../store/reservaStore';

describe('ConfirmacaoReserva', () => {
  beforeEach(() => {
    useReservaStore.setState({
      viagemSelecionada: {
        id: '1',
        origem: 'São Paulo',
        destino: 'Rio de Janeiro',
        dataHoraPartida: '2026-08-01T10:00:00Z',
        precoBase: 150,
        assentosDisponiveis: 10,
        totalAssentos: 10,
        assentosOcupados: [],
        assentosLivres: [1, 2, 3],
      },
      assentoSelecionado: 1,
    });
  });

  it('deve exibir erros de validação ao submeter formulário vazio', async () => {
    const user = userEvent.setup();
    render(<BrowserRouter><ConfirmacaoReserva /></BrowserRouter>);

    await user.click(screen.getByRole('button', { name: 'Confirmar Reserva' }));

    expect(screen.getByText('Nome completo é obrigatório.')).toBeInTheDocument();
    expect(screen.getByText('CPF deve ter 11 dígitos.')).toBeInTheDocument();
  });
});