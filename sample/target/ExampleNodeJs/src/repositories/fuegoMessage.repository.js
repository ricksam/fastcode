const Crud = require('../database/crud')

module.exports = class FuegoMessageRepository {
  static async get (id, transaction) {
    return await Crud.findBy({ id }, 'FuegoMessage', transaction)
  }

  static async list (transaction) {
    return await Crud.filterBy({  }, 'FuegoMessage', transaction)
  }

  static async save(entity, transaction){
    return await Crud.save(entity, 'FuegoMessage', transaction)
  }

  static async delete(id, transaction){
    return await Crud.delete({ id }, 'FuegoMessage', transaction)
  }
}

