if(cxScan.IsFrameworkActive("AngularJS"))
{
	CxList arrayInitializer = Find_ArrayInitializer();
	CxList paramDecls = Find_ParamDecl();
	CxList methodDecls = Find_MethodDecls();
	CxList stringLiterals = Find_String_Literal();
	CxList unknownReferences = Find_UnknownReference();

	// find $sce and $sceDelegate mode (Strict Contextual Escaping)
	List<string> sceString = new List<string>(new string[]{"$sce","$sceProvider"});
	CxList sceInstances = stringLiterals.FindByShortNames(sceString);
	sceInstances.Add(paramDecls.FindByShortNames(sceString));


	// find all $sce references
	CxList sceRef = All.NewCxList();
	foreach(CxList sceInstance in sceInstances) {

		// find $sce references in a controller => controller('ctrl', ['$scope','$sce', function(sco, sce){...}]);
		if(sceInstance.FindByType(typeof(StringLiteral)).Count > 0)
		{
	
			ArrayInitializer arrayInit = sceInstance.GetFathers().FindByType(typeof(ArrayInitializer))
				.TryGetCSharpGraph<ArrayInitializer>();
			StringLiteral cseLiteral = sceInstance.TryGetCSharpGraph<StringLiteral>();
			
			if(arrayInit != null && arrayInit.InitialValues != null && cseLiteral != null)
			{
				int position = arrayInit.InitialValues.IndexOf(cseLiteral);
				Expression expr = arrayInit.InitialValues[arrayInit.InitialValues.Count - 1];
				CxList func = All.NewCxList();
				if(expr is LambdaExpr) {
					func.Add(All.FindById(expr.NodeId));
				} 
				else if(expr is UnknownReference) {
					CxList unknownReference = All.FindById(expr.NodeId);
					unknownReference = unknownReference.FindByAbstractValue(_ => _ is FunctionAbstractValue);
					func.Add(unknownReferences.FindAllReferences(unknownReference).GetAssigner());
					func.Add(methodDecls.FindDefinition(unknownReference));
				}
				CxList parameter = paramDecls.GetParameters(func, position);
				sceRef.Add(parameter);
				sceRef.Add(unknownReferences.GetByAncs(func).FindAllReferences(parameter));
			}
		} 
			// find $sce parameter declaration references => filter('unsafe', function($sce) {...});
		else if(sceInstance.FindByType(typeof(ParamDecl)).Count > 0)
		{
			ParamDecl paramDecl = sceInstance.TryGetCSharpGraph<ParamDecl>();
			CxList paramReference = All.FindById(paramDecl.NodeId);
		
			if(paramReference.GetByAncs(arrayInitializer).Count == 0)
			{
				sceRef.Add(unknownReferences.FindAllReferences(paramReference));
			}
		}
	}

	CxList enabledParameters = All.GetParameters(sceRef.GetMembersOfTarget().FindByShortName("enabled"));
	CxList booleanParams = enabledParameters.FindByType(typeof(BooleanLiteral));

	result.Add(booleanParams.FindByShortName("false"));
	result.Add(enabledParameters.FindByAbstractValue(x => x is FalseAbstractValue));
}