const express = require('express')
const router = express.Router()

const AccountController = require('../../controllers/account.controller')
const AuthMiddleware = require('../../middlewares/auth.middleware')

router.post('/login', AccountController.login)
router.post('/redefinePassword', AuthMiddleware.defaultUser, AccountController.redefinePassword)
router.get('/getByRecoveryKey/:recoveryKey', AccountController.getByRecoveryKey)
router.post('/changePassword', AccountController.changePassword)

module.exports = router
