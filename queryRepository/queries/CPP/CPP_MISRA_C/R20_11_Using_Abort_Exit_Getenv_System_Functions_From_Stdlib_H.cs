/*
MISRA C RULE 20-11
------------------------------
This query searches for usage of the library functions 'abort', 'exit', 'getenv' and 'system' from library <stdlib.h>

	The Example below shows code with vulnerability: 

#include <stdlib.h>

void mc2_2011 ( void )
{
  int32_t mc2_2011_status;
  char_t * mc2_2011_env;

  mc2_2011_env = getenv ( "path" );
}

*/

// Safety check for the violating h file
CxList includes = All.FindByType(typeof(StringLiteral)).GetParameters(Find_Methods()
	.FindByShortName("CX_INCL"));
CxList hFile = includes.FindByShortName("stdlib.h");
if (hFile.Count > 0){
	// Start with all instances of the functions
	CxList methodInvokes = All.FindByType(typeof(MethodInvokeExpr));
	CxList funcs = methodInvokes.FindByShortNames(new List<string>(){"abort","exit","getenv","system"});

	// Remove all locally defined instances
	CxList defs = All.FindDefinition(funcs);
	funcs -= funcs.FindAllReferences(defs);

	result = funcs;
}