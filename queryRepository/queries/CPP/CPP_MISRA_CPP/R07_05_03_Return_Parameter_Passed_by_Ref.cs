/*
 MISRA CPP RULE 7-5-3
 ------------------------------
 This query searches for functions that return a reference or a pointer to a parameter
 that is passed by reference or const reference.
 
 The Example below shows code with vulnerability: 

	int * fn1 (int & x) {
		return (&x); 		//Non-compliant
	}
	const int * fn3 (const int & x){
		return (&x); 		//Non-compliant
	}
	int & fn4 (int & x) {
		return (x); 		//Non-compliant
	}  
	const int & fn5 (const int & x) {
		return (x); 		//Non-compliant
	}  

*/

//Find parameter passed by reference or const reference.
CxList refParams = All.FindByType(typeof(ParamDecl));
refParams = All.FindByRegex(@"(?<=\&\s*)\w", false, false,false) * refParams;
//Find if the parameter is returned.
CxList dataFromParams = All.FindAllReferences(refParams);
CxList retStmt = All.GetByAncs(All.FindByType(typeof(ReturnStmt)));
retStmt = dataFromParams * retStmt;
retStmt -= retStmt.GetByAncs(retStmt.GetAncOfType(typeof(MethodInvokeExpr)));

result = retStmt.DataInfluencedBy(refParams);