const { validationResult } = require('express-validator')

module.exports = function validationMiddleware (request, response, next) {
  const errors = validationResult(request)
  if (!errors.isEmpty()) {
    response.status(400).json({
      message: errors
        .formatWith(({ location, msg, param, value, nestedErrors }) => {
          return msg
        })
        .mapped()
    })
  } else {
    next()
  }
}
