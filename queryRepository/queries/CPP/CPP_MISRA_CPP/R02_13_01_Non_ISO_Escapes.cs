/*
MISRA CPP RULE 2-13-1
------------------------------
This query searches for usage of escape sequences not defined in the ISO C++ standard

	The Example below shows code with vulnerability: 

use_char('\c');

*/

result = All.FindByName("*\x0127*").FindByType(typeof(CharLiteral));
result -= All.FindByType(typeof(CharLiteral)).FindByRegex(@"\\x",false,false,false);