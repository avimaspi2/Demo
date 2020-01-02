/*
MISRA CPP RULE 5-18-1
------------------------------
This query searches for uses of the comma operator

	The Example below shows code with vulnerability: 

use_uint64 ( 9223372036854775808 );
use_uint32 ( 0x80000000 );

*/

// commas appear as:
// seperating paramaters to functions
// seperating variables declarations
// seperating enum member declarations
// seperating paramaters to array initialization
// comma operator

CxList commas = All.FindByRegex(@"[^\s\r\n]\s*,[^']", false, false, false);
commas = commas.FindByType(typeof(Expression)) + commas.FindByType(typeof(ExprStmt));

// remove paramater calls/definitions
commas -= commas.GetByAncs(All.FindByType(typeof(Param)));
commas -= commas.GetByAncs(All.FindByType(typeof(ParamCollection)));
commas -= commas.GetByAncs(All.FindByType(typeof(ParamDeclCollection)));
commas -= commas.GetByAncs(All.FindByType(typeof(MethodInvokeExpr)));
commas -= commas.GetByAncs(All.FindByType(typeof(MethodInvokeExpr)).GetFathers());

// remove declarations
commas -= commas.FindByType(typeof(ClassDecl));
commas -= commas.GetByAncs(All.FindByType(typeof(FieldDecl)));
commas -= commas.GetByAncs(All.FindByType(typeof(VariableDeclStmt)));
commas -= commas.GetByAncs(All.FindByType(typeof(EnumMemberDecl)));

// array intialization
commas -= commas.GetByAncs(All.FindByType(typeof(ArrayInitializer)));

result = commas;