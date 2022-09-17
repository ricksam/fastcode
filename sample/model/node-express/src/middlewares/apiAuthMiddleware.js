const appConfig = require('../utils/appconfig')
const jwt = require('jsonwebtoken')

module.exports = class ApiAuthMiddleware{
  static async isAuthenticated(req, res, next){
    try {
      /*
                Some good middleware code :)
                */

      getUserAuthenticated(req).then(decoded => {
        res.user = decoded
        next()
      })
        .catch(err => {
          res.status(401).send(err)
        })
    } catch (err){
      next(err)
    }
  }
}

function getUserAuthenticated(req){
  return new Promise((resolve, reject) => {
    let token = req.cookies[appConfig.COOCKIE_NAME]
    if (!token){
      token = req.headers.token
    }
    // let token = req.headers.token;

    jwt.verify(token, appConfig.SECRET, (err, decoded) => {
      if (err){
        reject(err)
      } else if (decoded){
        resolve(decoded)
      }
    })
  })
}
