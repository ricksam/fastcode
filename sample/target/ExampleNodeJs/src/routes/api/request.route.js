const express = require('express')
const router = express.Router()

const RequestController = require('../../controllers/request.controller')

router.post('/message', RequestController.message)
router.post('/syncronize', RequestController.syncronize)

module.exports = router
