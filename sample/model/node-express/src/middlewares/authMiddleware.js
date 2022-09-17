//const userService = require('../services/userService')
const appConfig = require('../utils/appconfig')

module.exports = class AuthMiddleware{
  static async restricted(req, res, next){
    try {
      const token = req.cookies[appConfig.COOCKIE_NAME]

      //if (!token){ return userService.logOut(res) }
      //const decoded = await userService.validate(token)
      //req.decoded = decoded
      //if (!decoded){ return userService.logOut(res) }

      next()
    } catch (err){
      console.error(err)
      return userService.logOut(res)
    }
  }
}
