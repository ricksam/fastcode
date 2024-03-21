const AccountService = require('../services/account.service')

module.exports = class AccountController {
  static async login(request, response, next) {
    try {
      const data = await AccountService.login(request.body);
      response.json(data)
    } catch (error) {
      response.status(400).json({ message: error.message })
    }
  }

  static async redefinePassword(request, response, next){
    try {
      const data = await AccountService.redefinePassword(request.body.email);
      response.json(data)
    } catch (error) {
      response.status(400).json({ message: error.message })
    }
  }

  static async getByRecoveryKey (request, response, next) {
    try {
      const data = await AccountService.getByRecoveryKey(request.params.recoveryKey)
      response.json(data)
    } catch (error) {
      response.status(400).json({ message: error.message })
    }
  }

  static async changePassword(request, response, next){
    try {
      const {recoveryKey, password}=request.body;
      const data = await AccountService.changePassword(recoveryKey, password);
      response.json(data)
    } catch (error) {
      response.status(400).json({ message: error.message })
    }
  }
}