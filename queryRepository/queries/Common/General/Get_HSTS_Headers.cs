if(param.Length == 1){
	CxList nodes = (CxList) param[0];
	CxList methods = nodes.FindByType(typeof(MethodInvokeExpr));
	CxList firstParameter = All.GetParameters(methods, 0);	
	
	/*handle methods whose 1st parameter is "Strict-Transport-Security"
	               and the 2nd parameter is the header values*/
	CxList relevantMethods = methods.FindByParameters(firstParameter
		.FindByShortName("*Strict-Transport-Security*", false));
	result = All.GetParameters(relevantMethods, 1).FindByType(typeof(StringLiteral));
	
	/*handle methods with only 1 string parameter*/
	foreach(CxList m in methods){
		if(All.GetParameters(m).FindByType(typeof(Param)).Count == 1){
			CxList fstParam = All.GetParameters(m, 0).FindByType(typeof(StringLiteral));
			string paramStr = fstParam.GetName();
			if(paramStr.StartsWith("Strict-Transport-Security", StringComparison.InvariantCultureIgnoreCase)
			&& paramStr.Contains(":")){
				result.Add(fstParam);
			}
		}
	}	

	
	
	/*handle MemberAccess assigns*/
	result.Add(nodes.FindByType(typeof(MemberAccess)).GetAssigner()
		.FindByType(typeof(StringLiteral)));
	
	result.Add( Get_HSTS_Headers_Language_Specific(nodes));
	
}