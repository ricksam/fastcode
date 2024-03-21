const jwt = require('jsonwebtoken')
const md5 = require('md5')
const uuid = require('uuid')

const UserRepository = require('../repositories/user.repository')
const MembershipRepository = require('../repositories/membership.repository')

module.exports = class AccountService {

  static async login(entity) {
    try {
      const user = await UserRepository.getByEmail(entity.email);
      if (!user) {
        throw new Error("Email não cadastrado");
      }

      const membership = await MembershipRepository.getByUser(user.id)||{};
      if (membership.password != md5(entity.password)) {
        throw new Error("Senha inválida");
      }

      const access_token = jwt.sign(user, process.env.JWT_SECRET);

      return { success: true, access_token, user };
    } catch (err) {
      return { success: false, message: err.message };
    }
  }


  static async isAuthenticated(token, filterAdmin) {
    return new Promise((resolve, reject) => {
      jwt.verify(token, process.env.JWT_SECRET, async (error, decoded) => {
        if (!error) {
          try {
            const foundUser = await UserRepository.getByEmail(decoded.email)
            if(filterAdmin && !foundUser.isAdmin){
              reject(new Error('Usuário não é admin'))
            }
            resolve(foundUser)
          } catch (error) {
            reject(new Error('Usuário não autorizado'))
          }
        } else {
          reject(new Error('Usuário não autorizado'))
        }
      })
    })
  }

  static async redefinePassword(email) {
    try{
      const user = await UserRepository.getByEmail(email)
      const membership = await MembershipRepository.getByUser(user.id)||{userId:user.id};
      membership.recoveryKey = md5(uuid.v1());
      await MembershipRepository.save(membership);
      return {success: true, recoveryKey: membership.recoveryKey} ;
    }catch{
      return { success: false, message: err.message };
    }
  }

  static async getByRecoveryKey(recoveryKey) {
    const membership = await MembershipRepository.getByRecoveryKey(recoveryKey);
    return await UserRepository.get(membership.userId);
  }

  static async changePassword(recoveryKey, password){
    try{
      const membership = await MembershipRepository.getByRecoveryKey(recoveryKey);
      membership.password=md5(password);
      membership.recoveryKey="";
      await MembershipRepository.save(membership);
      return {success: true } ;
    }catch{
      return { success: false, message: err.message };
    }
    
  }

}

