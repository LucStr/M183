// server lib
var express = require('express');
var speakeasy = require('speakeasy');
var QRCode = require('qrcode');
var bodyParser = require('body-parser')

var app = express();
app.use(bodyParser.json());       // to support JSON-encoded bodies
app.use(bodyParser.urlencoded({     // to support URL-encoded bodies
  extended: true
})); 
var speakeasy = require('speakeasy');
var secret = speakeasy.generate_key();

const admin_username = "Yaca";
const admin_password = "Luis";

app.get('/get', (req, res) => {
    /*var secret = speakeasy.generateSecret({length: 20});
    console.log(secret.base32); // Save this value to your DB for the user*/
    QRCode.toDataURL(secret.otpauth_url, function(err, image_data) {
        res.send('<img src="' + image_data + '" />'); // A data URI for the QR code image
    });
});

app.get('/check', (req, res) => {
    res.sendFile(__dirname + '/check.html');
});

app.post('/checkp', (req, res) => {
    var { username, password, token } = req.body;    
    var verified = false;
    if(username == admin_username && password == admin_password){
        verified = speakeasy.totp.verify({
            secret: secret.base32,
            encoding: 'base32',
            token: token
        });
    }   
    res.send(`Verified: ${verified}`);
})

app.listen(3000, function () {
  console.log('Example app listening on port 3000!');
});