const Crud = require('../database/crud')

module.exports = class RequestRepository {
  static async get (id, transaction) {
    return await Crud.findBy({ id }, 'Request', transaction)
  }

  static async list (transaction) {
    return await Crud.filterBy({  }, 'Request', transaction)
  }

  static async save(entity, transaction){
    return await Crud.save(entity, 'Request', transaction)
  }

  static async delete(id, transaction){
    return await Crud.delete({ id }, 'Request', transaction)
  }

  static async first (id, transaction) {
    return await Crud.findBy({  }, 'Request', transaction)
  }
}

