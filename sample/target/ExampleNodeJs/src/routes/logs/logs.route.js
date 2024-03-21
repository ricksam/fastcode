const express = require('express')
const router = express.Router()
const logMiddleware = require('../../middlewares/log.middleware');
const LogsController = require('../../controllers/logs.controller')

router.post('/login', LogsController.login)
router.get('/doWork',  LogsController.doWork)

router.get('/lasts', logMiddleware, LogsController.lasts)
router.get('/methods', logMiddleware, LogsController.methods)
router.get('/urls', logMiddleware, LogsController.urls)
router.get('/log/:id', logMiddleware, LogsController.log)
router.post('/search', logMiddleware, LogsController.search)







module.exports = router
