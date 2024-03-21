const express = require('express')
const path = require('path')
const cors = require('cors')
//const cookieParser = require('cookie-parser')
const nocache = require('nocache')
const mongoose = require('mongoose');

const app = express()

app.use(cors());

app.use(nocache())

// view engine setup
app.set('views', path.join(__dirname, 'views'))
app.set('view engine', 'html')
app.set('etag', false)

app.use(express.json())
app.use(express.urlencoded({ extended: false }))
console.log(__dirname)
app.use(express.static(path.join(__dirname, 'public')))
//app.use(cookieParser())

// routes
app.use(require('./routes'))

// error handler
app.use((err, req, res, next) => {
  res.locals.message = err.message

  res.locals.error = err

  console.error(err)

  return res.status(err.status || 500).send({
    message: err.message,
    stack: err.stack
  })
})

// remove cache
app.use((req, res, next) => {
  res.set('Cache-Control', 'no-store, no-cache, must-revalidate, private')
  next()
})

const server = require('http').createServer(app)

//const io = require('socket.io')(server)

//require('./sockets')(io) // setup socket emitters

const port = process.env.PORT || '3000'

server.listen(port, () => {
  console.log(`listening on port: ${port}`)
})

mongoose.connect(process.env.DB || "mongodb://127.0.0.1/fast_code",
  {
    useNewUrlParser: true,
    useUnifiedTopology: true,
    //useCreateIndex: true
  });

var db = mongoose.connection;
db.on("error", console.error.bind(console, "connection error"));
db.once("open", function (callback) {
  console.log("Mongo connection Succeeded");
});

module.exports = app
