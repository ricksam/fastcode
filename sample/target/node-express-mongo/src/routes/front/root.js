const express = require('express')
const router = express.Router()

const APIUrl = process.env.API_URL || 'http://localhost:3000/api/'

router.get('/', (req, res) => {
  res.render('home.ejs', { APIUrl })
})

router.get('/Cliente', (req, res) => {  res.render('Cliente/index.ejs', { APIUrl })})
router.get('/Produto', (req, res) => {  res.render('Produto/index.ejs', { APIUrl })})
router.get('/Venda', (req, res) => {  res.render('Venda/index.ejs', { APIUrl })})
router.get('/Financeiro', (req, res) => {  res.render('Financeiro/index.ejs', { APIUrl })})
router.get('/Usuario', (req, res) => {  res.render('Usuario/index.ejs', { APIUrl })})

module.exports = router

