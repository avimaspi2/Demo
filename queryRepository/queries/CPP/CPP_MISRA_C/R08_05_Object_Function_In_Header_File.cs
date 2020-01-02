/*
MISRA C RULE 8-5
------------------------------
This query searches for definitions of objects or functions in a header file

n_octal_const = 017;                                  
n_octal_const = 00;                          
n_octal_const = 0017;                       
n_octal_escape = '\01';

*/


// header files are defined to be anything used in #include (not just h files)
CxList headerFiles = All.FindByType(typeof(StringLiteral)).GetParameters(Find_Methods()
	.FindByShortName("CX_INCL"));

// add object definitions (assignments)
CxList problemIfInHeader = Find_All_Declarators() -
	All.FindByType(typeof(ObjectCreateExpr)).GetFathers() -
	All.FindByType(typeof(ArrayCreateExpr)).GetFathers();
problemIfInHeader.Add(All.FindByType(typeof(AssignExpr)));

// Add method definitions
problemIfInHeader.Add(All.FindByType(typeof(StatementCollection)).GetAncOfType(typeof(MethodDecl)));

// remove redundent results
problemIfInHeader -= (problemIfInHeader.GetByAncs(problemIfInHeader) - problemIfInHeader);
problemIfInHeader -= problemIfInHeader.FindByShortName("INCLUDEREPLACE*");
problemIfInHeader -= All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetFathers();


// intersect what is not allowed with the contents of the header files
foreach (CxList cur in headerFiles){
	StringLiteral inclusion = cur.data.GetByIndex(0) as StringLiteral;
	if (inclusion.Value == null) {
		continue;
	}
	string headerFileName = "*" + inclusion.Value;
	result.Add(problemIfInHeader.FindByFileName(headerFileName));
}

foreach (CxList cur in result){
		result -= (result.GetByAncs(cur) - cur);
}