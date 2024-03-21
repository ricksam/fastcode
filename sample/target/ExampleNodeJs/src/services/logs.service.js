const uuid = require('uuid')
const moment = require('moment')
const jwt = require('jsonwebtoken')

module.exports = class LogsService {
    static SECRET = "e77989ed21758e78331b20e477fc5582";
    static log_list = [];

    static isAuthenticated(authorization) {
        return new Promise((resolve, reject) => {
            const token = authorization.replace("Bearer ", "");
            jwt.verify(token, this.SECRET, async (error, decoded) => {
                if (!error) {
                    if (decoded.logged) {
                        resolve()
                    } else {
                        reject(new Error('Usuário não autorizado'))
                    }
                } else {
                    reject(new Error('Usuário não autorizado'))
                }
            })
        })
    }

    static addMorganLog(morganLog) {
        if (this.log_list.length > 2000) {
            this.log_list.shift();
        }
        const id = uuid.v1();
        const created = moment().format("YYYY-MM-DD HH:mm");
        const array = morganLog.split('|');
        const method = array[0];
        const url = array[1];
        const error = !array[2].includes('200');
        const len = array[3];
        const ms = array[4];
        const ip = array[5];
        const authorization = array[6];
        const body = array[7];
        this.log_list.push({ id, created, method, url, error, len, ms, ip, authorization, body });
    }

    static addLog(log) {
        if (this.log_list.length > 2000) {
            this.log_list.shift();
        }
        log.id = uuid.v1();
        log.created = moment().format("YYYY-MM-DD HH:mm");
        this.log_list.push(log);
    }

    static lasts() {
        return this.log_list;
    }

    static methods() {
        return [...new Set(this.log_list.map(q => q.method))];

    }

    static urls() {
        return [...new Set(this.log_list.map(q => q.url))];
    }

    static log(id) {
        return this.log_list.find(q => q.id == id)
    }

    static search(ip, date, method, url, others) {
        let list = this.log_list;

        if(date){
            list = list.filter(q=>q.created >= date);
        }

        if(ip){
            list = list.filter(q=>q.ip == ip);
        }

        if(method){
            list = list.filter(q=>q.method == method);
        }

        if(url){
            list = list.filter(q=>q.url == url);
        }

        if(others == "errors"){
            list = list.filter(q=>q.error);
        }

        if (others == "slowers") {
            return list.sort((a, b) => {
                return Number.parseFloat(b.ms) - Number.parseFloat(a.ms);
            });
        }
        else if (others == "fasters") {
            return list.sort((a, b) => {
                return Number.parseFloat(a.ms) - Number.parseFloat(b.ms);
            });
        }
        else {
            return list
        };
    }
}