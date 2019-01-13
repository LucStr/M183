const Nexmo = require('nexmo');
const express = require('express');
const path = require('path');
const bodyParser = require('body-parser');
var api_key = 'a818b8f5f45f8d4167c5826c5ed19ab3-060550c6-3bde8ea6';
var domain = 'sandboxab3a69e5a2654c82ac3bb968e892ee3f.mailgun.org';
var mailgun = require('mailgun-js')({apiKey: api_key, domain: domain});

const nexmo = new Nexmo({
  apiKey: '81ff5099',
  apiSecret: 'c5EE1xdbTjM37AWI'
});

const app = express();
app.use(bodyParser.json()); // support json encoded bodies
app.use(bodyParser.urlencoded({ extended: true })); // support encoded bodies

const USERNAME = 'User';
const PASSWORD = '123';
const TOKEN  = 'TokenToken';
const FROM = 'OTP Test'
const TO = '41793521525'

const OTP = {
    'SMS' : sendSMSToken,
    'MAIL' : sendMailToken
}

const PORT = 3000;

app.get('/', function (req, res) {
    res.sendFile(path.join(__dirname, 'auth.html'));
});

app.post('/auth', (req, res) => {
    var {username, password, otp} = req.body;
    console.log({username, password});
    if(username === USERNAME && password === PASSWORD){
        OTP[otp]();
        res.sendFile(path.join(__dirname, 'token.html'));
    } else 
        res.send('INVALID CREDENTIALS!');
});

app.post('/token', (req, res) => {
    var {token} = req.body;
    console.log({token});
    if(token === TOKEN)
        res.send('Valid Token');
    else
        res.send('Invalid Token');
});

function sendSMSToken(){
    var text = `Ihr Token lautet: ${TOKEN}`;
    nexmo.message.sendSms(FROM, TO, text)
}

function sendMailToken(){
    var data = {
        from: 'OTP User <me@samples.mailgun.org>',
        to: 'luca.strebel1@gmx.ch',
        subject: 'OTP Token',
        text: `Ihr Token lautet: ${TOKEN}`
      };
       
      mailgun.messages().send(data, function (error, body) {
        console.log(body);
      });
}

app.listen(PORT, () => console.log(`Listening on Port ${PORT}`));