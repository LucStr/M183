// server lib
var express = require('express');

var QRCode = require('qrcode');
var bodyParser = require('body-parser')

var app = express();

app.use( bodyParser.json() );       // to support JSON-encoded bodies
app.use(bodyParser.urlencoded({     // to support URL-encoded bodies
  extended: true
})); 

const secret = 'N5NFE4R6MJLGM3LNFQUVIYJIHFHG22KD';

app.get('/get', (req, res) => {
    /*var secret = speakeasy.generateSecret({length: 20});
    console.log(secret.base32); // Save this value to your DB for the user*/
    QRCode.toDataURL(secret, function(err, image_data) {
        res.send('<img src="' + image_data + '" />'); // A data URI for the QR code image
    });
});

app.get('/check', (req, res) => {
    res.sendFile(__dirname + '/check.html');
});

app.post('/checkp', (req, res) => {
    //{ username, password, token } = req.body;
    res.send('AHAHAH');
})

app.listen(3000, function () {
  console.log('Example app listening on port 3000!');
});