const DashboardRepository = require('../repositories/dashboard.repository')

module.exports = class CompanyService {
  static async list () {
    return await DashboardRepository.list()
  }
}

