const express = require('express')
const router = express.Router()
const moment = require('moment')

const APIUrl = process.env.API_URL || 'http://localhost:3000/api/'

router.get('/', (req, res) => {
    res.render('dev/index.ejs', { APIUrl })
})

router.get('/logs', (req, res) => {
  res.render('dev/logs.ejs', { APIUrl })
})

router.get('/logs/login', (req, res) => {
  res.render('dev/login.ejs', { APIUrl })
})

module.exports = router

