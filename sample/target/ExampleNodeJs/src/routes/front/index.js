const express = require('express')
const router = express.Router()

router.use('/', require('./root'))
router.use('/dev', require('../logs'))

module.exports = router
