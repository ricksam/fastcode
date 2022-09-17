const express = require('express')
const router = express.Router()

router.use('/Cliente', require('./Cliente'))
router.use('/Produto', require('./Produto'))
router.use('/Venda', require('./Venda'))
router.use('/Financeiro', require('./Financeiro'))
router.use('/Usuario', require('./Usuario'))

module.exports = router

