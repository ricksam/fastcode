const database = require('../database/config.database')
const RequestRepository = require('../repositories/request.repository')
const FuegoMessageService = require('../services/fuegoMessage.service')

const ApplicationService = require('../services/application.service')
const ChatService = require('../services/chat.service')
const TagService = require('../services/tag.service')

const MessageTagRepository = require('../repositories/messageTag.repository')
const MessageRepository = require('../repositories/message.repository')

module.exports = class RequestService {
  static async get(id) {
    return await RequestRepository.get(id)
  }
  static async list() {
    return await RequestRepository.list()
  }
  static async save(entity) {
    try {
      return { success: true, data: await RequestRepository.save(entity) };
    } catch (err) {
      return { success: false, message: err.message };
    }
  }
  static async delete(id) {
    try {
      await RequestRepository.delete(id);
      return { success: true };
    } catch (err) {
      return { success: false, message: err.message };
    }
  }

  static async insertMessage(subscribeData) {
    
    const base64String = subscribeData.message.data;
    if(base64String!="string"){
      this.save({ data: Buffer.from(base64String, 'base64').toString('utf-8') });
    }
    this.triggerSyncronize();
  }

  static extractHashtag(texto) {
    const hashtags = texto.match(/#\w+/g);
    return hashtags || [];
  }

  static async syncronizeRequest(request) {

    const requestData = JSON.parse(request.data);
    const entity = {
      customerSlug: requestData.customerSlug,
      senderType: requestData.senderType,
      customerExternalAddress: requestData.customerExternalAddress,
      chatId: requestData.chatId,
      messageId: requestData.messageId,
      createTime: requestData.createTime,
      updateTime: requestData.updateTime,
      appId: requestData.appId,
      content: requestData.content,
      creationDate: requestData.creationDate,
      isNoteMessage: requestData.isNoteMessage,
      isPrivateMessage: requestData.isPrivateMessage,
      isTemplate: requestData.isTemplate,
      name: requestData.name,
      sendToWhatsapp: requestData.sendToWhatsapp,
      senderUserId: requestData.senderUserId,
      source: requestData.source,
      status: requestData.status,
      statusId: requestData.statusId,
    }

    const applicationId = (await ApplicationService.getOrCreate(entity.appId)).id;
    const chatId = (await ChatService.getOrCreate(applicationId, entity.chatId)).id;

    const hashtags = this.extractHashtag(entity.content);
    const hashtagIds = [];

    for (const hashtag of hashtags) {
      const id = (await TagService.getOrCreate(hashtag)).id;
      if (!hashtagIds.includes(id)) {
        hashtagIds.push(id);
      }
    }

    const transaction = await database.transaction()
    try {

      const fuegoMessage = (await FuegoMessageService.save(entity, transaction)).data;
      const messageId = (await MessageRepository.save({ chatId, fuegoMessageId: fuegoMessage.id, creationDate:fuegoMessage.creationDate }, transaction)).id;
      for (const tagId of hashtagIds) {
        await MessageTagRepository.save({ tagId, messageId }, transaction);
      }
      await RequestRepository.delete(request.id, transaction);
      await transaction.commit()

      return fuegoMessage;
    } catch (err) {
      if (!transaction.isCompleted()) {
        await transaction.rollback();
      }

      throw err;
    }
  }



  static async syncronize() {
    try {
      let hasProcess = true;
      const fuegoMessageIds = [];
      while (hasProcess) {
        const request = await RequestRepository.first();
        if (request) {
          fuegoMessageIds.push((await this.syncronizeRequest(request)).id);
        } else {
          hasProcess = false;
        }
      }
      return fuegoMessageIds;
    } catch (err) {
      console.log(err);
    }
  }

  static sincronizing = false;

  static async triggerSyncronize() {
    if (!this.sincronizing) {
      try {
        this.sincronizing = true;
        await this.syncronize();
      }
      catch(err){
        console.log(err);
      } 
      finally {
        this.sincronizing = false;
      }
    }
  }
}

