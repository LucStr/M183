function onetimepad(text, key, decrypt) {
	console.log({text, key, decrypt})
	if (text.length != key.length)
		return 'invalid Key';
		
	if (text.length == 0)
		return 'invalid text';
	
	text = [...text].map(a => alphabetPosition(a.charCodeAt(0)));
	key = [...key].map(a => alphabetPosition(a.charCodeAt(0)).pos);

	return text
		.map((a, i) => a.base + ((26 + a.pos + key[i] * (decrypt ? -1 : 1)) % 26))
		.map(a => String.fromCharCode(a))
		.join('');

	function alphabetPosition(charCode) {
		var base = charCode < 91 && charCode >= 65 ? 65 : charCode >= 97 && charCode < 123 ? 97 : false;
		if (!base)
			return false;
		return { base: base, pos: charCode - base }
	}
}