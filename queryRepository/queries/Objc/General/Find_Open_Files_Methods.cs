CxList methodInvokes = Find_Methods();

CxList openMethods = methodInvokes.FindByShortName("fopen");
openMethods.Add(methodInvokes.FindByShortName("open"));
openMethods.Add(methodInvokes.FindByShortName("creat"));
openMethods.Add(methodInvokes.FindByShortName("OpenDocumentFile"));
openMethods.Add(methodInvokes.FindByMemberAccess("CFile.Open")); //to open a file

CxList parameters = All.GetParameters(openMethods);
foreach (CxList openMethod in openMethods)
{
	int numParams = parameters.GetParameters(openMethod).Count;
	if ((numParams > 0) || (openMethod.GetTargetOfMembers().Count > 0))
	{
		result.Add(openMethod);
	}
}

CxList objCOpenMethods = methodInvokes.FindByShortName("fileHandleForUpdatingAtPath:*");

result.Add(objCOpenMethods);