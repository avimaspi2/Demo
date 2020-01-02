CxList methods = Find_Methods();

CxList pageCalls = Find_VF_Pages() * methods;
CxList apexFilesContent = Find_Apex_Files();

CxList actions = pageCalls
	- pageCalls.FindByShortName("get*")
	- pageCalls.FindByShortName("set*");

// Remove actions with XSRF token
actions -= Find_Anti_XSRF_Actions();

// Find the actions in the apex files
actions = apexFilesContent.FindDefinition(actions);

CxList allInputs = Find_Interactive_Inputs();
CxList actionsInputs = allInputs.GetByMethod(actions);

CxList db = Find_DB_Active() * apexFilesContent;

CxList xsrf = actionsInputs.InfluencingOnAndNotSanitized(db, Find_Test_Code() + Find_Anti_XSRF_Actions());

// Add the initializers that automatically fire a delete
CxList initializers = methods.GetByAncs(Find_VF_Pages().FindByName("*.apex.page.action"));
CxList initDeletes = apexFilesContent.FindDefinition(initializers);
CxList sObjectsInApex = Find_sObjects() * apexFilesContent;

CxList xsrf2 = db.GetByAncs(initDeletes);

xsrf2 -= xsrf2.DataInfluencingOn(xsrf2);
foreach (CxList xsrf1 in xsrf2)
{
	CxList source = Find_VF_Pages().FindAllReferences(All.GetMethod(xsrf1));
	CxList sourceMethod = source.GetAncOfType(typeof(MethodDecl)).FindByShortName("__*");
	source -= source.GetByAncs(sourceMethod);
	xsrf.Add(source.ConcatenateAllSources(xsrf1, true));
}
result = xsrf;

result -= Find_Test_Code();