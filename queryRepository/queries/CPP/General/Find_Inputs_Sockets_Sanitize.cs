CxList methods = Find_Methods();

CxList interestingMethods = methods.FindByShortName("recv");
interestingMethods.Add(methods.FindByShortName("recvfrom"));

CxList sizeOfMethods = methods.FindByShortName("sizeof");
CxList binaryExprs = Find_BinaryExpr().GetByBinaryOperator(Checkmarx.Dom.BinaryOperator.Subtract);

foreach(CxList method in interestingMethods){
	CxList thirdParam = All.GetByAncs(All.GetParameters(method, 2));
	//Get third parameter that has sizeof
	CxList thirdParamSizeOf = thirdParam * sizeOfMethods;
	
	if(thirdParamSizeOf.Count > 0){
		CxList secondParam = All.GetParameters(method, 1);
		CxList secondParamDefinition = All.FindDefinition(secondParam);
		CxList sizeOfParam = All.GetParameters(thirdParamSizeOf, 0);
		CxList sizeOfParamDefinition = All.FindDefinition(sizeOfParam);
		
		//if the definition is the same then continue filter
		//this include inheritance
		if((secondParamDefinition * sizeOfParamDefinition).Count >= 1){
			CxList binExpr = binaryExprs * thirdParam;
			BinaryExpr bin = binExpr.TryGetCSharpGraph<BinaryExpr>();
			if(bin != null){
				int num = 0;
				CSharpGraph rightNum = bin.Right;
				if(rightNum != null){
					bool parsed = int.TryParse(rightNum.ShortName, out num);
					if(parsed && num > 0){
						//add second param to sanitizers, the method is not included in flow
						result.Add(secondParam);
					}
				}
			}
		}
	}	
}