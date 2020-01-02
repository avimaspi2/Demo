/*
MISRA C RULE 20-10
------------------------------
This query searches for usage of the library functions 'atof', 'atoi' and 'atol' from library <stdlib.h>

	The Example below shows code with vulnerability: 

#include <stdlib.h>

void  mc2_2010 ( void )
{
  float64_t mc2_2010_a_to_float_result;
  int32_t   mc2_2010_a_to_int_result;
  int64_t   mc2_2010_a_to_long_result;

  mc2_2010_a_to_float_result = atof ( "123.5" );
  mc2_2010_a_to_int_result = atoi("12345");  
  mc2_2010_a_to_long_result = atol("12345");

*/

// Safety check for the violating h file
CxList includes = All.FindByType(typeof(StringLiteral)).GetParameters(Find_Methods()
	.FindByShortName("CX_INCL"));
CxList hFile = includes.FindByShortName("stdlib.h");
if (hFile.Count > 0){
	// Start with all instances of the functions
	CxList methodInvokes = All.FindByType(typeof(MethodInvokeExpr));
	CxList funcs = methodInvokes.FindByShortName("atof") +
		methodInvokes.FindByShortName("atoi") +
		methodInvokes.FindByShortName("atol");

	// Remove all locally defined instances
	CxList defs = All.FindDefinition(funcs);
	funcs -= funcs.FindAllReferences(defs);

	result = funcs;
}