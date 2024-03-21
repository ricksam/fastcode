const morgan = require('morgan')
const logger = require('../utils/logger.util')

const stream = {
  write: (message) =>{
	  //logger.http(message)
  } 
}

const skip = () => {
  const env = process.env.NODE_ENV || 'development'
  return env !== 'development'
}

const morganMiddleware = morgan(
  //':method|:url|:status|:res[content-length]|:response-time|:req["x-forwarded-for"]|:authorization|:body|:response',
  ':method :url :status :res[content-length] - :response-time ms',
  { stream, skip }
)

module.exports = morganMiddleware
