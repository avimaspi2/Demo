/*
MISRA CPP RULE 6-6-5
------------------------------
This query searches for methods with more than one return, or a return not covering the exit path

	The Example below shows code with vulnerability: 

int32_t f(void){ 
	...	
	if(a==b){ 
		return(1);  
	} 
	i++;
}

*/

CxList methods = All.FindByType(typeof(MethodDecl));
CxList returns = All.FindByType(typeof(ReturnStmt));
CxList controls = All.FindByType(typeof(IfStmt)) + All.FindByType(typeof(IterationStmt)) + All.FindByType(typeof(SwitchStmt));
CxList returnsInControl = returns.GetByAncs(controls);

foreach(CxList cur in methods){
	CxList descReturns = returns.GetByAncs(cur);
	CxList descReturnsInControls = returnsInControl.GetByAncs(cur);
	
	// more than a single return statement is always a violation
	if (descReturns.Count >= 2){
		result.Add(descReturns);
	}
	
	// returns not in end of method - wrapped by control object
	if (descReturnsInControls.Count > 0){
		result.Add(descReturnsInControls);
	}
}