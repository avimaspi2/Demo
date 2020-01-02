/*
MISRA C RULE 20-6
------------------------------
This query searches for usage of the macro 'offsetof', in library <stddef.h>

	The Example below shows code with vulnerability: 

#include <stddef.h>

void mc2_2006 ( void )
{
	struct mc2_2006_s { int16_t mc2_2006_a; int16_t mc2_2006_b; };
  	uint32_t mc2_2006_offset;
}

*/

// Safety check for the violating h file
CxList includes = All.FindByType(typeof(StringLiteral)).GetParameters(Find_Methods()
	.FindByShortName("CX_INCL"));
CxList hFile = includes.FindByShortName("stddef.h");
if (hFile.Count > 0){
	// Start with all instances of offsetof
	CxList offsetofs = All.FindByRegex("offsetof");

	result = offsetofs;
}