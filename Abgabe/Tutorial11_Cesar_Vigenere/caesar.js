function caesar(diff, text) {
  return text.split('').map(a => a.charCodeAt(0)).map(a => {
    var base = a < 91 && a >= 65 ? 65 : a >= 97 && a < 123 ? 97 : false;
    if (!base)
      return a;
    return base + ((a - base + diff) % 26)
  }).map(a => String.fromCharCode(a)).join('')
}