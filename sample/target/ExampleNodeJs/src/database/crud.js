const database = require('./config.database')

module.exports = class Crud {
    static removeID(entity) {
        const copiaSemId = ({ id, ...resto }) => resto;
        return copiaSemId(entity);
    }

    static toCamelcase(obj) {
        if (typeof obj !== 'object' || obj === null) {
            return obj;
        }

        if (Array.isArray(obj)) {
            return obj.map(this.toCamelcase);
        }

        let novoObj = {};
        for (let key in obj) {
            const firstCharLower = key.charAt(0).toLowerCase();
            const camelCase = firstCharLower + key.substring(1);
            novoObj[camelCase] = obj[key];
        }
        //console.log("novoObj:", novoObj);
        return novoObj;
    }

    static async findBy(where, from, transaction) {
        try {
            const result = await (transaction ? transaction : database)
                .from(from)
                .where(where)
                .limit(1)
                .first()

            return this.toCamelcase(result)
        } catch (err) {
            console.log(err)
            throw err;
        }
    }

    static async filterBy(where, from, transaction) {
        try {
            const result = await (transaction ? transaction : database)
                .from(from)
                .where(where)

            return this.toCamelcase(result);
        } catch (err) {
            console.log(err)
            throw err;
        }
    }

    static async findById(id, from, transaction) {
        return await this.findByWhere({ id }, from, transaction);
    }

    static async insert(entity, from, transaction) {
        try {
            //const lastData = await transaction(from).max('id as id');
            //const lastDataId = lastData[0].id || 0;

            //entity.id = lastDataId + 1;
            entity = this.removeID(entity);

            const data = await (transaction ? transaction : database)(from)
                .insert(entity, "id")

            entity.id = data[0].id;
            return entity;
        } catch (err) {
            console.log(err)
            throw err;
        }
    }

    static async update(entity, from, transaction) {
        try {
            const id = entity.id;
            entity = this.removeID(entity);

            const updateCount = await (transaction ? transaction : database)(from)
                .where({ id })
                .update(entity)
            //console.log(updateCount);
            entity.id = id;
            return entity;
        } catch (err) {
            console.log(err)
            throw err;
        }
    }

    static async save(entity, from, transaction) {
        try {
            //console.log("save:", entity);
            if (!entity.id) {
                return await this.insert(entity, from, transaction);
            } else {
                return await this.update(entity, from, transaction);
            }
        } catch (err) {
            console.log(err)
            throw err;
        }
    }

    static async delete(where, from, transaction) {
        try {
            const result = await (transaction ? transaction : database)
                .from(from)
                .where(where)
                .del()

            return result
        } catch (err) {
            console.log(err)
            throw err;
        }
    }
}