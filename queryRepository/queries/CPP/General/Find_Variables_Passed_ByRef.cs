CxList methodDecls = Find_Method_Declarations();
CxList methodCalls = Find_Methods();
CxList parameters = Find_Parameters();
CxList paramDecls = Find_ParamDecl();
CxList paramDeclCollections = Find_ParamDeclCollection();
CxList refs = Find_Unknown_References();
CxList relevantParams = All.NewCxList();

foreach(CxList m in methodDecls){
	CxList methodParams = paramDecls.FindByFathers(paramDeclCollections.FindByFathers(m));
	CxList methodRefs = methodCalls.FindAllReferences(m);
	foreach(CxList prm in methodParams ){
		ParamDecl p = prm.TryGetCSharpGraph<ParamDecl>();
		if(p != null && p.Direction == ParamDirection.Ref) {
			int paramIndex = prm.GetIndexOfParameter();
			relevantParams.Add(refs.GetParameters(methodRefs, paramIndex));
		}
	}
}

result = relevantParams;