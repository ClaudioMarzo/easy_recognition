# Aplicação de Reconhecimento Facial

Esta aplicação completa de Reconhecimento Facial consiste em um backend desenvolvido com .NET e um frontend criado com React. Além disso, utiliza o Elasticsearch para indexação e busca eficiente de dados faciais. A seguir, você encontrará instruções básicas para configurar e executar o projeto em sua máquina.

## Índice

- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Pré-requisitos](#pré-requisitos)
- [Instalação](#instalação)
  - [1. Configurando o Elasticsearch](#1-configurando-o-elasticsearch)
  - [2. Configurando o Backend (.NET)](#2-configurando-o-backend-net)
  - [3. Configurando o Frontend (React)](#3-configurando-o-frontend-react)
- [Executando a Aplicação](#executando-a-aplicação)
- [Uso](#uso)
- [Considerações Finais](#considerações-finais)

---

## Tecnologias Utilizadas

- **Backend:**
  - [.NET 6](https://dotnet.microsoft.com/download/dotnet/6.0)
  - [FaceRecognitionDotNet](https://github.com/takuyaa/FaceRecognitionDotNet)
  - [Elasticsearch](https://www.elastic.co/elasticsearch/)
- **Frontend:**
  - [React](https://reactjs.org/)
  - [Axios](https://axios-http.com/)
  - [React-Bootstrap](https://react-bootstrap.github.io/)
  - [React Router DOM](https://reactrouter.com/)

---

## Pré-requisitos

Antes de iniciar, certifique-se de ter as seguintes ferramentas instaladas em sua máquina:

- **.NET SDK:** [Download aqui](https://dotnet.microsoft.com/download)
- **Node.js e npm:** [Download aqui](https://nodejs.org/)
- **Elasticsearch:** [Download aqui](https://www.elastic.co/downloads/elasticsearch)
- **Git:** [Download aqui](https://git-scm.com/downloads)
- **Editor de Código:** [Visual Studio Code](https://code.visualstudio.com/) recomendado

---

## Instalação

### 1. Configurando o Elasticsearch

1. **Download e Instalação:**
   - Acesse a [página de downloads do Elasticsearch](https://www.elastic.co/downloads/elasticsearch) e baixe a versão apropriada para o seu sistema operacional.
   - Siga as instruções de instalação específicas para o seu sistema.

2. **Iniciando o Elasticsearch:**
   - **Windows:**
     ```bash
     cd caminho/para/elasticsearch/bin
     elasticsearch.bat
     ```
   - **macOS/Linux:**
     ```bash
     cd caminho/para/elasticsearch/bin
     ./elasticsearch
     ```

3. **Verificando a Instalação:**
   - Abra o navegador e vá para [http://localhost:9200](http://localhost:9200). Você deverá ver uma resposta JSON com informações sobre o cluster.

4. **Configurando o Índice para Reconhecimento Facial:**
   - Utilize o `curl` ou ferramentas como [Postman](https://www.postman.com/) para criar o índice com as configurações necessárias.
   - **Exemplo com `curl`:**
     ```bash
     curl -X PUT "localhost:9200/faces" -H 'Content-Type: application/json' -d'
     {
       "settings": {
         "index": {
           "knn": true
         }
       },
       "mappings": {
         "properties": {
           "name": {
             "type": "text"
           },
           "position": {
             "type": "text"
           },
           "face_embeding": {
             "type": "knn_vector",
             "dimension": 128
           }
         }
       }
     }
     '
     ```
   - **Nota:** Certifique-se de que o campo `face_embeding` tenha a mesma dimensão dos vetores gerados pela biblioteca de reconhecimento facial utilizada.

### 2. Configurando o Backend (.NET)

1. **Clonando o Repositório:**
   ```bash
   git clone https://github.com/seu-usuario/seu-repositorio.git
   cd seu-repositorio/API
   ```

2. **Instalando Dependências:**
   - Navegue até a pasta do projeto backend e restaure as dependências:
     ```bash
     cd API
     dotnet restore
     ```

3. **Configurando o Projeto:**
   - **Ajuste os caminhos para salvar as imagens:**
     - Abra o arquivo `ImagemController.cs` localizado em `API/Controllers/`.
     - Altere os caminhos das pastas para onde as imagens serão salvas, conforme seu sistema operacional. Exemplo:
       ```csharp
       var imagePath = Path.Combine("C:/Users/SeuUsuario/Pictures/reconhecimento_teste", nomeDaImagem);
       ```
   - **Certifique-se de que o diretório exista ou crie-o manualmente.**

4. **Executando o Backend:**
   ```bash
   dotnet run
   ```
   - A API estará disponível em [http://localhost:5000](http://localhost:5000).

### 3. Configurando o Frontend (React)

1. **Navegando para a Pasta do Frontend:**
   ```bash
   cd ../face-recognition-frontend
   ```

2. **Instalando Dependências:**
   ```bash
   npm install
   ```

3. **Configurando Variáveis de Ambiente (Opcional):**
   - Caso deseje alterar os endpoints da API, você pode configurar variáveis de ambiente no arquivo `.env`.

4. **Executando o Frontend:**
   ```bash
   npm start
   ```
   - A aplicação React estará disponível em [http://localhost:3000](http://localhost:3000).

---

## Executando a Aplicação

1. **Inicie o Elasticsearch:** Certifique-se de que o Elasticsearch está rodando conforme as instruções acima.
2. **Inicie o Backend (.NET):**
   ```bash
   cd API
   dotnet run
   ```
3. **Inicie o Frontend (React):**
   ```bash
   cd ../face-recognition-frontend
   npm start
   ```
4. **Acesse a Aplicação:** Abra [http://localhost:3000](http://localhost:3000) no seu navegador para interagir com a interface de reconhecimento facial.

---

## Uso

A aplicação possui duas funcionalidades principais acessíveis através da interface web:

1. **Comparar Imagem:**
   - Navegue até a página "Comparar Imagem".
   - Selecione uma imagem do seu computador.
   - Clique em "Comparar" para enviar a imagem ao backend.
   - O sistema irá analisar a imagem e retornar informações sobre possíveis correspondências faciais armazenadas no Elasticsearch.

2. **Salvar Imagem:**
   - Navegue até a página "Salvar Imagem".
   - Insira um nome associado à imagem.
   - Selecione uma imagem do seu computador.
   - Clique em "Salvar" para enviar a imagem e o nome ao backend.
   - A imagem será processada e suas informações faciais serão armazenadas no Elasticsearch para futuras comparações.


---

**Autor:** Cláudio Marzo Santarém Santana  
**Contato:** [Email- Claudio Marzo](claudio.m.santarem.16@exemplo.com)  

---