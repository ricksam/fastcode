const express = require('express')
const cors = require('cors')
const dotenv = require('dotenv')
const logger = require('./utils/logger.util')
const morgan = require('./middlewares/morgan.middleware')
const swaggerUi = require('swagger-ui-express');
const swaggerDocument = require('./swagger_output.json');
const path = require('path')
const LogsService = require('./services/logs.service')

dotenv.config()

const app = express()

// view engine setup
app.set('views', path.join(__dirname, 'views'))
app.set('view engine', 'ejs');
app.set('etag', false)

/////////////////////////////////////////////////////////
// Rota para a documentação Swagger UI
swaggerDocument.host = process.env.SWAGGER_HOST;
swaggerDocument.schemes = [process.env.SWAGGER_SCHEMES];
app.use('/swagger', swaggerUi.serve, swaggerUi.setup(swaggerDocument));
/////////////////////////////////////////////////////

// Middleware personalizado para capturar dados da resposta
app.use((req, res, next) => {

  //':method|:url|:status|:res[content-length]|:response-time|:req["x-forwarded-for"]|:authorization|:body|:response'
  res.localTime = new Date();
  const send = res.send;
  res.send = function (data) {

    let localData = {
      ip: res.req.ip,
      method: res.req.method,
      url: res.req.originalUrl,
      auth: res.req.headers["authorization"] || res.req.headers["Authorization"] || '',
      body: JSON.stringify(res.req.body) || '{}',
      status: res.statusCode,
      date: new Date(),
      len: data.length,
      ms: new Date() - res.localTime,
      response: data
    }
    const clone = Object.assign({}, localData);
    if (clone.url != "/" && !clone.url.includes("/logs") && !clone.url.includes("/api/logs/")) {
      LogsService.addLog(clone);
    }

    send.call(this, data);
  };
  next();
});


app.use(morgan)
app.use(cors())

app.use(express.json())
app.use(express.urlencoded({ extended: false }))
app.use(express.static(path.join(__dirname, 'public')))
app.use(require('./routes'))

app.use(function (error, _, response, next) {
  response.locals.message = error.message

  response.locals.error = error

  logger.error(JSON.stringify(error))

  return response.status(error.status || 500).send({
    message: error.message,
    stack: error.stack
  })
})



module.exports = app
