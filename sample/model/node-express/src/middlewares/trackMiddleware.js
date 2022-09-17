//const LogService = require('../services/logService')

module.exports = class TrackMiddleware{
  static async tracking(req, res, next){
    try {
      req.body = RemoveCommands(req.body)
      req.params = RemoveCommands(req.params)
      addLog(req, res)
    } catch (err){
      next(err)
    }

    next()
  }
}

function RemoveCommands(container){
  if (container){
    const commands = ['select ', 'from ', 'where ', 'delete ', 'create ', 'insert ', 'update ', 'drop ', 'truncate ', '*', '+']
    const cols = Object.getOwnPropertyNames(container)
    for (const col of cols){
      for (const cmd of commands){
        const value = container[col]
        if (typeof value !== 'string'){
          break
        }
        if (value && value.toLowerCase().includes(cmd.toLowerCase())){
          const start = value.toLowerCase().indexOf(cmd.toLowerCase())
          const length = cmd.length
          const remove = value.slice(start, length)
          container[col] = container[col].replace(remove, '')
        }
      }
    }
  }
  return container
}

function addLog(req, res){
  const ip = req.connection.remoteAddress
  const id = res.user ? res.user.id : 0

  const log = {
    Id: 0,
    Method: req.method,
    URL: req.originalUrl,
    Params: JSON.stringify(req.params),
    Body: JSON.stringify(req.body),
    Created: new Date(),
    Ip: ip,
    UserId: id
  }

  //LogService.save(log)
}
