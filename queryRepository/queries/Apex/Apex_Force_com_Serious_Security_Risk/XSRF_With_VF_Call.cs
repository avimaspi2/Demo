// This query returns, in addition to the XSRF itself, also the place(s) in the VF pages where this method
// is called.
// It gives for every problem 2 results, so it's possible to track the problem.
CxList methods = Find_Methods();

CxList pageCalls = Find_VF_Pages() * methods;
CxList apexFilesContent = Find_Apex_Files();

CxList actions = pageCalls
	- pageCalls.FindByShortName("get*")
	- pageCalls.FindByShortName("set*")
	- pageCalls.FindByName("JSContainer_*");

// Remove actions with XSRF token
actions -= Find_Anti_XSRF_Actions();

// Find the actions in the apex files
actions = All.FindDefinition(actions) * apexFilesContent;

CxList allInputs = Find_Interactive_Inputs();
CxList actionsInputs = allInputs.GetByMethod(actions);

CxList db = Find_DB_Active() * apexFilesContent;

CxList xsrf = actionsInputs.InfluencingOnAndNotSanitized(db, Find_Test_Code() + Find_Anti_XSRF_Actions());

// Add the initializers that automatically fire a delete
CxList initializers = methods.GetByAncs(Find_VF_Pages().FindByName("*.apex.page.action"));
CxList initDeletes = apexFilesContent.FindDefinition(initializers);
CxList sObjectsInApex = Find_sObjects() * apexFilesContent;

xsrf.Add(db.GetByAncs(initDeletes));

CxList source = Find_VF_Pages().FindAllReferences(All.GetMethod(xsrf));
CxList sourceMethod = source.GetAncOfType(typeof(MethodDecl)).FindByShortName("__*");
source -= source.GetByAncs(sourceMethod);

result = xsrf + source;

result -= Find_Test_Code();