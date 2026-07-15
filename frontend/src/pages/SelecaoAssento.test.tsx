import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { BrowserRouter } from 'react-router-dom';
import { describe, it, expect, beforeEach } from 'vitest';
import SelecaoAssento from './SelecaoAssento';
import { useReservaStore } from '../store/reservaStore';

describe('SelecaoAssento', () => {
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
        assentosOcupados: [3],
        assentosLivres: [1, 2, 4, 5, 6, 7, 8, 9, 10],
      },
      assentoSelecionado: null,
    });
  });

  it('deve permitir selecionar um assento livre', async () => {
    const user = userEvent.setup();
    render(<BrowserRouter><SelecaoAssento /></BrowserRouter>);

    await user.click(screen.getByTestId('assento-5'));

    expect(useReservaStore.getState().assentoSelecionado).toBe(5);
  });

  it('deve bloquear seleção de assento ocupado', () => {
    render(<BrowserRouter><SelecaoAssento /></BrowserRouter>);

    expect(screen.getByTestId('assento-3')).toBeDisabled();
  });
});