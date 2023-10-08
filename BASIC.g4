grammar BASIC;options
{language=CSharp;}

program: declaration (',' declarationWithoutMod)* EOF;

declaration: accessModifier identifier 'As' dataType ('=' constant)?;
declarationWithoutMod: identifier 'As' dataType ('=' constant)?;

accessModifier: 'Dim' | 'Public' | 'Private';
identifier: LETTER (LETTER | DIGIT)*;
dataType: 'Double' | 'Integer';
constant: DIGIT+ ('.' DIGIT*)?;

LETTER: [A-Za-z_];
DIGIT: [0-9];

// Пробельные символы и символы перевода строки игнорируются
WS: [ \t\r\n]+ -> skip;
