function caesar(diff, text){
	return text.split('').map(a => a.charCodeAt(0)).map(a => {
    var base = a < 91 && a >= 65 ? 65 : a >= 97 && a < 123 ? 97 : false;
    if(!base)
      return a;	
    return base + ((a - base + diff) % 26)
  }).map(a => String.fromCharCode(a)).join('')
}

function vigenere(key, text, decode){
	key = key.split('').map(a => a.charCodeAt(0)).map(alphabetPosition);
	if(!key.every(a => a))
		return console.log('invalid key');
	key = key.map(a => a.pos);
	return text.split('').map(a => a.charCodeAt(0)).map((a, i) => {
	var base = alphabetPosition(a);
	if(!base)
		return a;	
	base = base.base;
	return base + ((26 + a - base + key[i % key.length] * (decode ? -1 : 1)) % 26)
}).map(a => String.fromCharCode(a)).join('')

  function alphabetPosition(charCode) {
	 var base = charCode < 91 && charCode >= 65 ? 65 : charCode >= 97 && charCode < 123 ? 97 : false;
     if(!base)
       return false;
	 return {base: base, pos: charCode - base}	
  }
}
