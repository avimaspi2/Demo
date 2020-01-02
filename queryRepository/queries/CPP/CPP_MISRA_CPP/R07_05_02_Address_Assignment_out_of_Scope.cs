/*
 MISRA CPP RULE 7-5-2
 ------------------------------
 This query searches for address assignments of auto-storage variables outside of their scope.

 The Example below shows code with vulnerability: 

      void foo (){
			int * pl;
			{
				int local_auto;
				pl = &local_auto 	//Non-Compliant
			}

*/

//Find auto-storage variables.
CxList decs = Find_All_Declarators();
CxList vars = decs;
CxList nonAuto = (All - All.FindByType(typeof(MethodDecl)));
nonAuto = nonAuto.FindByFieldAttributes(Modifiers.Extern) + nonAuto.FindByFieldAttributes(Modifiers.Static);
nonAuto = decs.GetByAncs(nonAuto);
vars -= decs;
CxList refs = All.FindByType(typeof(Reference));
CxList addresses = All.NewCxList();
//Getting address assignments.
CxList unarys = All.FindByType(typeof(UnaryExpr));
foreach (CxList unary in unarys) {
	if( ((UnaryExpr) unary.data.GetByIndex(0)).Operator == UnaryOperator.Address &&
	unary.GetAncOfType(typeof(Param)).Count == 0) {
		addresses.Add(All.FindById(((UnaryExpr) unary.data.GetByIndex(0)).Right.NodeId));
	}
}
vars = All.FindDefinition(addresses.FindAllReferences(vars));
refs = refs.DataInfluencedBy(vars);
decs = decs.FindDefinition(refs);
CxList classes = refs.FindByType(typeof(ClassDecl));
foreach(CxList auto in vars) {
	//Get auto's scope
	CxList scope = auto.GetAncOfType(typeof(StatementCollection));
	if (scope.Count == 0){
		scope = auto.GetAncOfType(typeof(ClassDecl));
		if (scope.Count == 0){
			scope = auto.GetAncOfType(typeof(StructDecl));
		}
		else if ( ((ClassDecl) scope.data.GetByIndex(0)).Name.Contains("checkmarx_default_classname")){
			continue;
		}
	}
	scope.Add(classes.GetByAncs(scope));
	//Get references with address assignment in their path.
	CxList calls = refs.DataInfluencedBy(auto);
	calls -= calls.InfluencedByAndNotSanitized(auto, addresses);
	foreach(CxList call in calls) {
		
		CxList dec = decs.FindDefinition(call);
		if (dec.Count == 0 ) {//No definition.
			continue;
		}
		
		//Get call's scope, check if within auto's scope.
		bool isIn = false;
		CxList callScope = dec;
		CxList prevScope = dec;
		do {
			prevScope = callScope;
			CxList check = scope * callScope;
			if (check.Count > 0) {
				isIn = true;
			}
			CxList oldScope = callScope;
			callScope = oldScope.GetAncOfType(typeof(StatementCollection));
			if (callScope.Count == 0){
				callScope = oldScope.GetAncOfType(typeof(ClassDecl));
				if (callScope.Count == 0){
					callScope = oldScope.GetAncOfType(typeof(StructDecl));
				}
			}
			
		} while (callScope.Count > 0 && !(callScope == prevScope) && isIn == false);
				
		if(!isIn){
			result.Add(call.DataInfluencedBy(auto));
		}
	}
}