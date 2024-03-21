require('dotenv').config()

module.exports = {
  client: 'mssql',
  connection: {
    user: process.env.DATABASE_USER,
    password: process.env.DATABASE_PASSWORD,
    server: process.env.DATABASE_HOST,
    port: parseInt(process.env.DATABASE_PORT),
    database: process.env.DATABASE_NAME,
    encrypt: process.env.DATABASE_SSL.toLowerCase() === 'true'
  },
  migrations: {
    directory: './src/database/migrations',
    tableName: 'Migration'
  },
  seeds: {
    directory: './src/database/seeds'
  }
}
