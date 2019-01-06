//Client-ID: 580650817377-s9fosr2vun7dg50ag9u6pvr0il7u1fip.apps.googleusercontent.com
//Secret: JC-YCdTR2ASrrUYvrs7M-3O7

const path = require('path');
const bodyParser = require('body-parser');

const PORT = 3000;


var express = require('express');
var app = express();
app.use(bodyParser.json()); // support json encoded bodies
app.use(bodyParser.urlencoded({ extended: true })); // support encoded bodies

app.get('/', function (req, res) {
    res.sendFile(path.join(__dirname, 'index.html'));
});

app.post('/auth', (req, res) => {
    console.log(req.body);
    res.sendStatus(200);
})

app.listen(PORT, a => console.log(`Listening on Port ${PORT}`));