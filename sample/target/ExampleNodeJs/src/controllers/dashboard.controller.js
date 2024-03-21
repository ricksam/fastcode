const DashboardService = require('../services/dashboard.service')

module.exports = class DashboardController {
  static async list (request, response, next) {
    try {
      const data = await DashboardService.list()
      response.json(data)
    } catch (error) {
      response.status(400).json({ message: error.message })
    }
  }
}

