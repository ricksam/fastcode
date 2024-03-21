const RequestService = require('../services/request.service')
module.exports = class RequestController {

  static async message(request, response, next) {
    try {
      await RequestService.insertMessage(request.body);
      response.json({ success: true })
    } catch (error) {
      response.status(400).json({ message: error.message })
    }
  }

  static async syncronize(request, response, next){
    try {
      const fmessage = await RequestService.syncronize();
      response.json({ success: true, data:fmessage })
    } catch (error) {
      response.status(400).json({ message: error.message })
    }
  }
}