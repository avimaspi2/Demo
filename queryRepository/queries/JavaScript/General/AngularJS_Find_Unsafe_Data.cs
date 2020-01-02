/*
 *	Find the potential unsafe AngularJS $sce and $sceDelegate modes (Strict Contextual Escaping), 
 *	when using 'trustAs' methods. The 'trustAs' methods returns a wrapped object that represents 
 *	the given value, that could be unsafe.
 */
 
if(cxScan.IsFrameworkActive("AngularJS"))
{
	CxList paramDecls = Find_ParamDecl();
	CxList methodDecls = Find_MethodDecls();
	CxList stringLiterals = Find_String_Literal();
	CxList unknownReferences = Find_UnknownReference();

	// find $sce and $sceDelegate mode (Strict Contextual Escaping)
	List<string> sceString = new List<string>(new string[]{"$sce","$sceDelegate"});
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
			sceRef.Add(unknownReferences.FindAllReferences(paramReference));
		}
	}

	// returns all potential unsafe $sce => trustAs(), trustAsHtml(), trustAsUrl(), trustAsResourceUrl(), trustAsJs()
	result = sceRef.GetMembersOfTarget().FindByShortName("trustAs*");
}