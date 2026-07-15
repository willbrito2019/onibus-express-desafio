import { BrowserRouter, Routes, Route } from 'react-router-dom';
import BuscaPassagens from './pages/BuscaPassagens';
import SelecaoAssento from './pages/SelecaoAssento';
import ConfirmacaoReserva from './pages/ConfirmacaoReserva';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<BuscaPassagens />} />
        <Route path="/selecao-assento" element={<SelecaoAssento />} />   
        <Route path="/confirmacao" element={<ConfirmacaoReserva />} />     
      </Routes>
    </BrowserRouter>
  );
}

export default App;