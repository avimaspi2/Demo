/*
MISRA C RULE 20-8
------------------------------
This query searches for usage of the signal handling facilities of <signal.h>

	The Example below shows code with vulnerability: 

#include <signal.h>           

void mc2_2008 ( void )
{
int16_t mc2_2008_signal;
mc2_2008_signal = SIGINT;
}

*/

// Safety check for the violating h file
// (it is also a violation in itself)
CxList includes = All.FindByType(typeof(StringLiteral)).GetParameters(Find_Methods()
	.FindByShortName("CX_INCL"));
CxList hFile = includes.FindByShortName("signal.h");
if (hFile.Count > 0){
	// The functions defined by signal.h
	result.Add(All.FindByType(typeof(MethodInvokeExpr)).FindByShortName("signal"));
	result.Add(All.FindByType(typeof(MethodInvokeExpr)).FindByShortName("raise"));

	// The macros defined by signal.h
	result.Add(All.FindByShortNames(new List<string>()
		{"SIG_DFL","SIG_ERR","SIG_IGN","SIGABRT","SIGFPE","SIGILL","SIGINT","SIGSEGV","SIGTERM"}));

	// Remove all locally defined instances
	CxList defs = All.FindDefinition(result);
	result -= result.FindAllReferences(defs);

	// the include
	result.Add(hFile);

	// The types
	result.Add(All.FindByType(typeof(TypeRef)).FindByShortName("sig_atomic_t"));
}