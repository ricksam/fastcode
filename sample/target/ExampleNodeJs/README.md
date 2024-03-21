# template-node-api 
Template de projeto api, com arquitetura simples, utilizando:
  - Node
  - ExpressJS
  - SQL Server

### Libs
  - Dotenv
  - JWT
  - *Database*
    - MSSQL
  - *Framework*
    - Knex
    - Express
      - Express Validator
    - Multer
    - Body Parser
    - Cors
  - *Logs*
    - Morgan
    - Winston
      - Winston Rotate File

### Instalação
Para realizar a instalação da aplicação:
 - Executar o comando `npm install`
### Executar em desenvolvimento
Para realizar a execução da aplicação em desenvolvimento:
 - Executar o comando `npm run dev`
 - A aplicação rodará na porta **3000**
### Execução em produção
Para realizar a execução da aplicação em produção:
 - Executar o comando `npm start`
### Lint Check
Para realizar a verificação do ESLINT:
 - Executar o comando `npm run lint`
### Lint Fix
Para forçar o ajuste do ESLINT:
 - Executar o comando `npm run lint:fix`
### Criar migration
Para realizar a criação de uma migration para banco de dados:
 - Executar o comando `npx knex migrate:make create_user_table`
### Rodar migrations
Para realizar a execução das migrations para banco de dados:
 - Executar o comando `npm run migrations`
### Criar seed
Para realizar a criação de um seed para banco de dados:
 - Executar o comando `npx knex seed:make 01_user_table`
### Rodar seeds
Para realizar a execução dos seeds para banco de dados:
 - Executar o comando `npm run seeds`
### Gerar documentação do swagger
Para gerar a documentação do swagger utilize o comando
 - Executar o comando `node src/swagger.js`