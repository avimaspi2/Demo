/*
MISRA CPP RULE 8-4-1
------------------------------
This query searches for functions declared or defined using Ellipsis notation - variable number of arguments

	The Example below shows code with vulnerability: 

void foo(int arg1, ... );

*/

// find instances of the "..." operation
CxList varArgs = All.FindByRegex(@"\.\.\.", false, false, false, All.NewCxList());
result.Add(varArgs.GetAncOfType(typeof(ParamDeclCollection)).GetAncOfType(typeof(MethodDecl)));
result.Add(varArgs.FindByType(typeof(StatementCollection)).GetFathers().FindByType(typeof(MethodDecl)));