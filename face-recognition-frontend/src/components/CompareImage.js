import React, { useState } from 'react';
import { Form, Button, Container, Alert, Spinner } from 'react-bootstrap';
import axios from 'axios';

const CompareImage = () => {
  const [selectedFile, setSelectedFile] = useState(null);
  const [response, setResponse] = useState(null);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleFileChange = (e) => {
    setSelectedFile(e.target.files[0]);
    setResponse(null);
    setError('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!selectedFile) {
      setError('Por favor, selecione uma imagem.');
      return;
    }

    const formData = new FormData();
    formData.append('Imagem', selectedFile);

    setLoading(true);
    setError('');
    setResponse(null);

    try {
      const res = await axios.post('http://localhost:5000/Imagem/comparar-imagem', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });
      setResponse(res.data);
    } catch (err) {
      if (err.response && err.response.data) {
        setError(err.response.data);
      } else {
        setError('Erro ao conectar com o servidor.');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container className="mt-4">
      <h2>Comparar Imagem</h2>
      <Form onSubmit={handleSubmit}>
        <Form.Group controlId="formFile" className="mb-3">
          <Form.Label>Selecione uma imagem para comparar</Form.Label>
          <Form.Control type="file" accept="image/*" onChange={handleFileChange} />
        </Form.Group>
        <Button variant="primary" type="submit" disabled={loading}>
          {loading ? <Spinner animation="border" size="sm" /> : 'Comparar'}
        </Button>
      </Form>
      {error && <Alert variant="danger" className="mt-3">{error}</Alert>}
      {response && (
        <Alert variant="success" className="mt-3">
          <pre>{JSON.stringify(response, null, 2)}</pre>
        </Alert>
      )}
    </Container>
  );
};

export default CompareImage;
