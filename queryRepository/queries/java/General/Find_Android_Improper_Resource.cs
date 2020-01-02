//This query finds unreleased open resources under Android Activities. 
//The first parameter is the type of the resource, the secong the name of the starting method,
//the third is the releasing method name.
//The optional fourth parameter is the creating class type (when it is different than the first one -
//e.g. for SQLite access, the first param is SQLiteDatabase, the fourth is SQLiteOpenHelper).
if (param.Length > 2) 
{
	string resourceType = param[0] as string;
	string openString = param[1] as string;
	string closeString = param[2] as string;
	string createResourceType = resourceType;
	if (param.Length > 3)
	{
		createResourceType = param[3] as string;
	}
	CxList methods = Find_Methods();
	CxList androidResources = All.FindByType(resourceType);
		
	CxList createResources = All.NewCxList();
	createResources.Add(androidResources);
	
	androidResources -= androidResources.FindByShortName(resourceType);
	if (createResourceType != resourceType)
	{
		createResources = All.FindByType(createResourceType);
	}

	//Check if a resource was opened but never closed.
	CxList openResource = createResources.GetMembersOfTarget();
	openResource = openResource.FindByShortName(openString).GetTargetOfMembers();
	CxList tempOpenResource = openResource.Clone();
	openResource = openResource.GetFathers();
	openResource = androidResources.GetByAncs(openResource).FindByAssignmentSide(CxList.AssignmentSide.Left);
	//Add also open strings from objects (non-static method calls).
	tempOpenResource -= tempOpenResource.FindByShortName(resourceType);
	if (createResourceType != resourceType)
	{
		tempOpenResource -= createResources;
	}
	openResource.Add(tempOpenResource);

	foreach (CxList curResource in openResource) 
	{
		CxList refs = androidResources.FindAllReferences(curResource);
		refs = refs.GetMembersOfTarget().FindByShortName(closeString);
		if (refs.Count == 0) 
		{
			result.Add(curResource);
		}
	}

	//Check if an opened resource is not handled in the different life cycle options.
	CxList resourceClasses = androidResources.GetAncOfType(typeof(ClassDecl)).InheritsFrom("Activity");
	CxList methodDecls = Find_MethodDeclaration();
	CxList overridenMethods = methodDecls.FindByFieldAttributes(Modifiers.Override);
	CxList onPauseMethods = overridenMethods.FindByShortName("onPause");
	CxList onDestroyMethods = overridenMethods.FindByShortName("onDestroy");
	CxList onStopMethods = overridenMethods.FindByShortName("onStop");

	foreach (CxList curClass in resourceClasses) 
	{
		CxList curPause = onPauseMethods.GetByAncs(curClass);
		CxList curDestroy = onDestroyMethods.GetByAncs(curClass);
		CxList curStop = onStopMethods.GetByAncs(curClass);
		CxList curResource = openResource.GetByAncs(curClass);
		if (curPause.Count == 0 || curDestroy.Count == 0 || curStop.Count == 0) 
		{
			result.Add(openResource);
		}
		else 
		{
			//Find the methods containing the closeString matching the curResource.
			CxList refs = androidResources.FindAllReferences(curResource);
			refs = refs.GetMembersOfTarget().FindByShortName(closeString);
			refs = refs.GetAncOfType(typeof(MethodDecl));
			
			CxList allMethods = All.NewCxList();
			allMethods.Add(methods);
			allMethods.Add(methodDecls);
			
			refs = allMethods.FindAllReferences(refs);
			//Find the cycle methods containing the relevant method invokes
			curPause = refs.GetByAncs(curPause);
			curDestroy = refs.GetByAncs(curDestroy);
			curStop = refs.GetByAncs(curStop);
			if (curPause.Count == 0 || curDestroy.Count == 0 || curStop.Count == 0) 
			{
				result.Add(openResource);
			}
		}
	}
}