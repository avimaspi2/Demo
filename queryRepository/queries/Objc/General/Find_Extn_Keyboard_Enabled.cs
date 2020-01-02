/// <summary>
///  This query recieves classes that implement the UIApplicationDelegate protocol
///	 	(by default - all classes that implement the protocol),
/// 	and check if they enable third party keyboards.
/// </summary>

CxList appDelegateClass = Find_AppDelegate_Class();

CxList applications = All.NewCxList();
if (param.Length == 1)
{
	try
	{
		applications = param[0] as CxList;
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex.Message);
		applications.Add(appDelegateClass);
	}
}
else 
{
	applications.Add(appDelegateClass);
}

CxList applicationElements = All.GetByAncs(applications);
CxList extentionPointerMethod = applicationElements.FindByShortName("application:shouldAllowExtensionPointIdentifier:");
CxList extentionPtrMethodElmnt = applicationElements.GetByAncs(extentionPointerMethod);

CxList extentionPtrReturn = extentionPtrMethodElmnt.FindByType(typeof(ReturnStmt));
CxList returnNo = extentionPtrReturn * All.FindByShortName("false").GetFathers();  // NO is parsed as false
result = applications - returnNo.GetAncOfType(typeof(ClassDecl));