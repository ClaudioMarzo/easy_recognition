// src/App.js
import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import NavigationBar from './components/NavigationBar';
import CompareImage from './components/CompareImage';
import SaveImage from './components/SaveImage';

function App() {
  console.log("App component mounted");
  return (
    <Router>
      <NavigationBar />
      <Routes>
        <Route path="/" element={<Navigate to="/comparar-imagem" />} />
        <Route path="/comparar-imagem" element={<CompareImage />} />
        <Route path="/salvar-imagem" element={<SaveImage />} />
      </Routes>
    </Router>
  );
}

export default App;
