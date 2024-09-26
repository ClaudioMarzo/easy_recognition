# Documentação de Pré-requisitos do Projeto de Reconhecimento Facial

Esta documentação lista todos os pré-requisitos necessários para rodar o projeto de reconhecimento facial em sua máquina.

---

## 1. Sistema Operacional

- **Ubuntu** (ou outra distribuição Linux compatível)

---

## 2. Python

- **Versão Recomendada**: Python **3.10.0**
- **Gerenciador de Versões**: **pyenv**
- **Ambiente Virtual**: **pyenv-virtualenv**

### 2.1. Instalação do pyenv e pyenv-virtualenv

#### Passos:

1. **Criar e ativar um ambiente virtual**:

   ```bash
   pyenv virtualenv 3.9.7 facial_recognition_env
   pyenv activate facial_recognition_env
   ```

---

## 3. Node.js

- **Versão Recomendada**: Node.js **14.x** ou superior
- **Gerenciador de Pacotes**: **npm** (geralmente instalado junto com o Node.js)

### 3.1. Instalação do Node.js e npm

#### Passos:

1. **Atualizar o cache de pacotes**:

   ```bash
   sudo apt update
   ```

2. **Instalar o Node.js e npm**:

   ```bash
   sudo apt install -y nodejs npm
   ```

3. **Verificar as versões instaladas**:

   ```bash
   node -v
   npm -v 
   ```

   Se a versão do Node.js for inferior à 14.x, considere instalar via Node Version Manager (nvm):

   ```bash
   curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.4/install.sh | bash
   exec $SHELL
   nvm install 14
   nvm use 14
   ```

---



## 4. Variáveis de Ambiente

### 4.1. Backend (Django)

As seguintes variáveis de ambiente devem ser configuradas para o backend:

- **`API_KEY`**: Sua **API Key** fornecida pelo **Face++**.
- **`API_SECRET`**: Seu **API Secret** fornecido pelo **Face++**.
- **`DEBUG`**: Define se o Django está em modo de depuração. Use `True` para desenvolvimento e `False` para produção.

#### Como Configurar:

Crie um arquivo `.env` no diretório do backend (onde está o `manage.py`):

```bash
cd easy_recognition/api/project
touch .env
```

Adicione as variáveis ao arquivo `.env`:

```dotenv
API_KEY=SUA_API_KEY_DO_FACE_PLUS_PLUS
API_SECRET=SEU_API_SECRET_DO_FACE_PLUS_PLUS
DEBUG=True
```

**Importante**:

- Substitua `SUA_API_KEY_DO_FACE_PLUS_PLUS` e `SEU_API_SECRET_DO_FACE_PLUS_PLUS` pelas suas credenciais reais.

### 4.2. Frontend (Node.js)

As seguintes variáveis de ambiente devem ser configuradas para o frontend:

- **`API_URL`**: A URL da API do backend. Por padrão, será `http://localhost:8000/recognize/`.

#### Como Configurar:

Crie um arquivo `.env` no diretório do frontend:

```bash
cd caminho/para/seu/projeto/frontend
touch .env
```

Adicione a variável ao arquivo `.env`:

```dotenv
API_URL=http://localhost:8000/recognize/
```
---

## 5. Links Úteis

- **Face++**: [https://www.faceplusplus.com/](https://www.faceplusplus.com/)
- **Pyenv**: [https://github.com/pyenv/pyenv](https://github.com/pyenv/pyenv)
- **NVM (Node Version Manager)**: [https://github.com/nvm-sh/nvm](https://github.com/nvm-sh/nvm)
