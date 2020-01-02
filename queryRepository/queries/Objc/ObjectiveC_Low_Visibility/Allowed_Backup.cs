CxList methods = Find_Methods();

CxList allUnknowReferences = Find_UnknownReference();
CxList allBooleanLiterals = Find_BooleanLiteral();
CxList allNSURLIsExcludedFromBackupKey = allUnknowReferences.FindByName("NSURLIsExcludedFromBackupKey");

//Revelant Methods to find
CxList createFileAtPathMethods = methods.FindByShortName("createFileAtPath:contents:attributes:");
CxList fileURLWithPathMethods = methods.FindByShortName("fileURLWithPath*");
fileURLWithPathMethods.Add(methods.FindByShortName("fileURL*"));
CxList PathsUrl = All.GetParameters(fileURLWithPathMethods, 0);

//Get setResourceValue methods that contain the NSURLIsExcludedFromBackupKey Key and have the first parameter set to false
CxList setResourceValueMethods = methods.FindByShortName("setResourceValue:forKey:*");

//Get all Booleans set to False  
CxList falseValues = allBooleanLiterals.FindByAbstractValue(abstractValue => abstractValue is FalseAbstractValue);
CxList firstParams = allUnknowReferences.GetParameters(setResourceValueMethods, 0);
falseValues.Add(firstParams);
falseValues.Add(methods.FindByMemberAccess("NSNumber.numberWithBool:").FindByParameters(falseValues));

setResourceValueMethods = setResourceValueMethods.FindByParameters(falseValues).FindByParameters(allNSURLIsExcludedFromBackupKey);

//Get the first parameters of CreationFile methods 
CxList createFileAtPathMethodsParams = All.GetParameters(createFileAtPathMethods, 0);
createFileAtPathMethodsParams -= createFileAtPathMethodsParams.FindByType(typeof(Param));

//Find Items without any configuration for backup
foreach (CxList source in createFileAtPathMethodsParams){
	if( (All.FindDefinition(source) * All.FindDefinition(PathsUrl)).Count == 0){
		result.Add(source.ConcatenatePath(methods.FindByParameters(source), false));
	}
}

//Find Items with ExcludedFromBackup set to No
CxList setResourceValueFlows = PathsUrl.DataInfluencingOn(setResourceValueMethods);

foreach (CxList setResourceValueFlow in setResourceValueFlows.GetCxListByPath()){
	//Get the first node of the flow 
	CxList source = setResourceValueFlow.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
	CxList creation = createFileAtPathMethodsParams.FindAllReferences(All.FindDefinition(source));
	//Check if any creationFile method is influencing the setResourceValueFlow
	if(creation.Count > 0){
		//Concatenate the creationFile method with the setResourceValueFlow
		result.Add(creation.ConcatenatePath(methods.FindByParameters(creation), false).ConcatenatePath(setResourceValueFlow, false));
	}
	else {
		//Otherwise add the setResourceValueFlow as a result
		result.Add(setResourceValueFlow);
	}
}