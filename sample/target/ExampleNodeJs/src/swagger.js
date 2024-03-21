const swaggerAutogen = require('swagger-autogen')();

const outputFile = './swagger_output.json';
const endpointsFiles = ['./app.js']; // Substitua pelo caminho real do seu arquivo principal

swaggerAutogen(outputFile, endpointsFiles);
