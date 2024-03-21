const AccountService = require('../services/account.service')

module.exports = class AuthMiddleware{
  static async defaultUser (request, response, next) {
    try {
      const token = request.headers.authorization.replace("Bearer ", "");
      request.auth = await AccountService.isAuthenticated(token, false)
      next()
    } catch (error) {
      response.status(401).json({ message: error.message })
    }
  }

  static async adminUser (request, response, next) {
    try {
      const token = request.headers.authorization.replace("Bearer ", "");
      request.auth = await AccountService.isAuthenticated(token, true)
      next()
    } catch (error) {
      response.status(401).json({ message: error.message })
    }
  }
}



