//const Crud = require('../database/crud')

module.exports = class CacheRepository {
  static cache_list = [];

  static async get(id, transaction) {
    //return await Crud.findBy({ id }, 'Cache', transaction)
  }

  static async list(transaction) {
    //return await Crud.filterBy({}, 'Cache', transaction)
  }

  static async save(entity, transaction) {
    if (this.cache_list.length > 2000) {
      this.cache_list.shift();
    }
    this.cache_list.push(entity);
    //return await Crud.save(entity, 'Cache', transaction)
  }

  static async delete(id, transaction) {
    //return await Crud.delete({ id }, 'Cache', transaction)
  }

  static async getByKey(key, transaction) {
    return this.cache_list.find(q => q.key == key) // Com cache em memória
    //return null; //sem cache
    //return await Crud.findBy({ key }, 'Cache', transaction) // com cache no banco de dados
  }
}

