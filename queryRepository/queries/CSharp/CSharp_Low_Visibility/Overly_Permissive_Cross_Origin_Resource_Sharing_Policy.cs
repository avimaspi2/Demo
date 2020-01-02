result = Common_Low_Visibility.Overly_Permissive_Cross_Origin_Resource_Sharing_Policy();

CxList methods = Find_Methods();
CxList unknown = Find_Unknown_References();
CxList methodDecl = Find_MethodDecls();
CxList paramList = Find_Param();
CxList relevantStrings = Find_Strings().FindByAbstractValue(abstractValue => abstractValue is StringAbstractValue strAbsVal && strAbsVal.Content.Equals("*"));

relevantStrings.Add(unknown.FindAllReferences(relevantStrings.GetAncOfType(typeof(Declarator)))); 

CxList unknownServiceCollection = unknown.FindByType("IServiceCollection");
CxList addCorslist = unknownServiceCollection.GetMembersOfTarget().FindByShortName("AddCors");
CxList lambdaCors = All.GetParameters(addCorslist).FindByType(typeof(LambdaExpr));
CxList methodsLamda = methods.GetByAncs(lambdaCors);
CxList methodsCors = methodsLamda.GetAncOfType(typeof(MethodInvokeExpr));
methodsCors.Add(methods.GetByAncs(methodDecl.FindDefinition(methodsCors)));

result.Add(methodsCors.FindByShortName("AllowAnyOrigin"));
CxList WithOrigins = methodsCors.FindByShortName("WithOrigins");
CxList filteredParams = All.GetByAncs(paramList);

foreach(CxList or in WithOrigins){
	CxList paramMe = filteredParams.GetParameters(or);
	if((paramMe*paramList).Count == 1 && (paramMe * relevantStrings).Count > 0 ){
		result.Add(or);
	}	
}