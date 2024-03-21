const jwt = require('jsonwebtoken')
//const uuid = require('uuid')
const LogsService = require('../services/logs.service')
const AccountService = require('../services/account.service')

module.exports = class LogsController {
    static doWork(request, response, next){
        response.status(200).send("ok")
    }

    static async login(request, response, next){
        const {login, password}=request.body;
        /*
        // crie um usuário e senha ou capture de um banco de dados
        if(login=="p6C8KzcTsn" && password=="oftpkmZCkI"){ 
            const token=jwt.sign({login, password, logged:true}, LogsService.SECRET);
            const data={
                token:token
            };
            
            response.status(200).send(data);
        }else{
            response.status(401).send("login ou senha inválidos")
        }*/
        const data = await AccountService.login({email:request.body, password});
        response.json(data)
    }

    static lasts(request, response, next){
        response.status(200).send(LogsService.lasts());
    }
    
    static methods(request, response, next){
        response.status(200).send(LogsService.methods());
    }
    
    static urls(request, response, next){
        response.status(200).send(LogsService.urls());
    }
    
    static log(request, response, next){
        const {id}=request.params;
        response.status(200).send(LogsService.log(id));
    }

    static search(request, response, next){
        const { ip, date, method, url, others}=request.body;
        response.status(200).send(LogsService.search(ip, date, method, url, others));
    }
}