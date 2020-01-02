/*
MISRA C RULE 4-1
------------------------------
This query searches for usage of escape sequences not defined in the ISO C standard

	The Example below shows code with vulnerability: 

use_char('\x0');    
use_char('\c');

*/

result = All.FindByName("*\x0127*").FindByType(typeof(CharLiteral));