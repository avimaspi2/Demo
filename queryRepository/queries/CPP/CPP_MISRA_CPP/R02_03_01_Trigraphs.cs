/*
MISRA CPP RULE 2-3-1
------------------------------
This query searches for uses of Trigraphs

	The Example below shows code with vulnerability: 

use_char_ptr ( "??(" );   
use_char_ptr("??)"); 
use_char_ptr ( "??/??/" );

*/


result = All.FindByRegex(@"\?\?[\(\)<>!'=-]|\?\?/\?\?/", false, true, false, All.NewCxList());