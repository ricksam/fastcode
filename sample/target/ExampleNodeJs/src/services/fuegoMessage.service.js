const FuegoMessageRepository = require('../repositories/fuegoMessage.repository')





module.exports = class FuegoMessageService {
  static async get(id) {
    return await FuegoMessageRepository.get(id)
  }
  static async list() {
    return await FuegoMessageRepository.list()
  }
  
  static async save(entity, transaction) {
    try {
      return { success: true, data: await FuegoMessageRepository.save(entity, transaction) };
    } catch (err) {
      return { success: false, message: err.message };
    }
  }

  static async delete(id) {
    try {
      await FuegoMessageRepository.delete(id);
      return { success: true };
    } catch (err) {
      return { success: false, message: err.message };
    }
  }


}

