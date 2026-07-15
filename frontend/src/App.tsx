import { BrowserRouter, Routes, Route } from 'react-router-dom';
import BuscaPassagens from './pages/BuscaPassagens';
import SelecaoAssento from './pages/SelecaoAssento';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<BuscaPassagens />} />
        <Route path="/selecao-assento" element={<SelecaoAssento />} />        
      </Routes>
    </BrowserRouter>
  );
}

export default App;