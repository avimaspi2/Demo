/*
MISRA C RULE 16-1
------------------------------
This query searches for functions declared or defined to have variable number of arguments

	The Example below shows code with vulnerability: 

void foo(int arg1, ... );

*/

// find instances of the "..." operation
CxList varArgs = All.FindByRegex(@"\.\.\.", false, false, false, All.NewCxList());

result.Add(varArgs.GetAncOfType(typeof(ParamDeclCollection)).GetAncOfType(typeof(MethodDecl)));
result.Add(varArgs.FindByType(typeof(StatementCollection)).GetFathers().FindByType(typeof(MethodDecl)));

// Safety check for the violating h file
// (it is also a violation in itself)
CxList includes = All.FindByType(typeof(StringLiteral)).GetParameters(Find_Methods()
	.FindByShortName("CX_INCL"));
CxList hFile = includes.FindByShortName("stdarg.h");
if (hFile.Count > 0){
	// Non compliant functions;
	result.Add(hFile + 
		All.FindByType(typeof(MethodInvokeExpr)).FindByShortName("va_arg") +
		All.FindByType(typeof(MethodInvokeExpr)).FindByShortName("va_start") +
		All.FindByType(typeof(MethodInvokeExpr)).FindByShortName("va_end"));
}