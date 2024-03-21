const LogsService = require('../services/logs.service')

module.exports = async function logMiddleware (request, response, next) {
  try {
    const authorization = request.headers["authorization"]
    request.auth = await LogsService.isAuthenticated(authorization)
    next()
  } catch (error) {
    response.status(401).json({ message: error.message })
  }
}
