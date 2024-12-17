// src/components/SaveImage.js
import React, { useRef, useState, useCallback } from 'react';
import { Form, Button, Container, Alert, Spinner, Row, Col } from 'react-bootstrap';
import axios from 'axios';
import Webcam from 'react-webcam';

const SaveImage = () => {
  const webcamRef = useRef(null);
  const [nome, setNome] = useState('');
  const [capturedImage, setCapturedImage] = useState(null);
  const [error, setError] = useState('');
  const [message, setMessage] = useState('');
  const [loading, setLoading] = useState(false);

  const videoConstraints = {
    width: 400,
    height: 300,
    facingMode: 'user',
  };

  const capture = useCallback(() => {
    const imageSrc = webcamRef.current.getScreenshot();
    setCapturedImage(imageSrc);
    setError('');
    setMessage('');
  }, [webcamRef]);

  const handleReset = () => {
    setCapturedImage(null);
    setNome('');
    setError('');
    setMessage('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!nome) {
      setError('Por favor, insira um nome.');
      return;
    }

    if (!capturedImage) {
      setError('Por favor, capture uma imagem.');
      return;
    }

    // Converter a imagem para um arquivo Blob
    const blob = await (await fetch(capturedImage)).blob();
    const file = new File([blob], 'captured-image.jpg', { type: 'image/jpeg' });

    const formData = new FormData();
    formData.append('Imagem', file);
    formData.append('nome', nome);

    setLoading(true);
    setError('');
    setMessage('');

    try {
      const res = await axios.post('http://localhost:5235/Imagem/salvar-imagem', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });
      setMessage(res.data);
      handleReset();
    } catch (err) {
      if (err.response && err.response.data) {
        setError(err.response.data);
      } else {
        setMessage('Imagem Salva com Sucesso');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container className="mt-4">
      <h2>Salvar Imagem</h2>
      <Form onSubmit={handleSubmit}>
        <Form.Group controlId="formNome" className="mb-3">
          <Form.Label>Nome</Form.Label>
          <Form.Control
            type="text"
            placeholder="Insira o nome"
            value={nome}
            onChange={(e) => setNome(e.target.value)}
          />
        </Form.Group>
        <Row className="mb-3">
          <Col md={6} className="text-center">
            {!capturedImage ? (
              <>
                <Webcam
                  audio={false}
                  height={300}
                  ref={webcamRef}
                  screenshotFormat="image/jpeg"
                  width={400}
                  videoConstraints={videoConstraints}
                />
                <Button variant="secondary" className="mt-2" onClick={capture}>
                  Capturar Imagem
                </Button>
              </>
            ) : (
              <>
                <img src={capturedImage} alt="Capturada" width={400} height={300} />
                <div className="mt-2">
                  <Button variant="warning" onClick={handleReset} className="me-2">
                    Repetir Captura
                  </Button>
                  <Button variant="primary" type="submit" disabled={loading}>
                    {loading ? <Spinner animation="border" size="sm" /> : 'Salvar'}
                  </Button>
                </div>
              </>
            )}
          </Col>
        </Row>
      </Form>
      {error && <Alert variant="danger">{error}</Alert>}
      {message && <Alert variant="success">{message}</Alert>}
    </Container>
  );
};

export default SaveImage;
