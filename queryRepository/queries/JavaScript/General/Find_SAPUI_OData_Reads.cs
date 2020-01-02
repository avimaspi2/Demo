if(cxScan.IsFrameworkActive("SAPUI"))
{
	CxList methodInvokes = Find_Methods();
	CxList associativeArrays = Find_AssociativeArrayExpr();
	CxList fieldDecls = Find_FieldDecls();
	CxList stringLiterals = Find_String_Literal();
	CxList parameters = Find_Param();
	CxList binaryExpr = Find_BinaryExpr();
	CxList strLtrlsAndBnrExprs = stringLiterals.Clone();
	strLtrlsAndBnrExprs.Add(binaryExpr);
	
	CxList bindElementMethods = methodInvokes.FindByShortName("bindElement");
	CxList fieldDeclsInAssocArray = fieldDecls.GetByAncs(associativeArrays.GetParameters(bindElementMethods));
	CxList parametersFieldDecls = fieldDeclsInAssocArray.FindByShortName("parameters");
	CxList fieldDeclsInParametersAssocArray = fieldDecls.GetByAncs(associativeArrays.GetByAncs(parametersFieldDecls));
	CxList expandFieldDecl = fieldDeclsInParametersAssocArray.FindByShortName("expand");
	CxList expandStringLiterals = stringLiterals.GetByAncs(expandFieldDecl);
	
	List < string > expandEntities = new List<string>();
	
	foreach(CxList str in expandStringLiterals) {
		StringLiteral strLtrl = str.TryGetCSharpGraph<StringLiteral>();
		if(strLtrl != null && strLtrl.Value != null) {
			expandEntities.AddRange(strLtrl.Value.Split(','));
		}
	}
	expandEntities = expandEntities.Distinct().ToList();
	
	//get <something>.getModel().getProperty(<prop>) MethodInvokeExpr, with empty model
	CxList getModelInvokes = methodInvokes.FindByShortName("getModel");
	CxList getModelWithParams = parameters.GetParameters(getModelInvokes).GetAncOfType(typeof(MethodInvokeExpr));
	CxList getModelWithoutParams = getModelInvokes - getModelWithParams;
	CxList getPropertyInvokes = getModelWithoutParams.GetMembersOfTarget().FindByShortName("getProperty");
	
	//get <something>.getBindingContext().getPath() MethodInvokeExpr
	CxList getBindingContextInvokes = methodInvokes.FindByShortName("getBindingContext");
	CxList getPathInvokes = getBindingContextInvokes.GetMembersOfTarget().FindByShortName("getPath");
	
	//getProperty's influenced by getBindingContext().getPath()
	CxList getPropertyInfluenced =
		getPropertyInvokes.DataInfluencedBy(getPathInvokes).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	
	getPropertyInvokes = getPropertyInvokes - getPropertyInfluenced;
	
	foreach(CxList getProperty in getPropertyInvokes) {
		CxList parameter =
			strLtrlsAndBnrExprs.GetParameters(getProperty, 0);
		CxList parameterStrLtrls = stringLiterals.GetByAncs(parameter);
		foreach(CxList parameterStr in parameterStrLtrls) {
			string strLtrl = parameterStr.GetName();
			if(!String.IsNullOrEmpty(strLtrl)) {
				string[] splitedStr = strLtrl.Split('/');
				List<string> splitedStrList = new List<string>(splitedStr);
				if(expandEntities.Exists(x => splitedStrList.Contains(x)))
					result.Add(getProperty);
			}
		}
	}
	
	result.Add(getPropertyInfluenced);
}