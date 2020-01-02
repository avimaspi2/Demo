// Detect VisualForce pages using controllers that do not specify "with sharing" and
// standard controllers that specify "without sharing"
////////////////////////////////////////////////////////////////////////////////////

CxList VFData = Find_VF_Pages();
CxList apexData = Find_Apex_Files();

CxList dbCalls = All.NewCxList();
CxList additionalDB = Find_DB();
int counter = 0;
while (additionalDB.Count > 0 && counter++ < 100)
{
	dbCalls.Add(additionalDB);
	additionalDB = Find_Methods().FindAllReferences(All.GetMethod(additionalDB)) - dbCalls;
}

CxList notWithSharing = (Find_Without_Sharing() + Find_Unknown_Sharing()) * apexData;

CxList noSharing = All.NewCxList();
foreach (CxList ac in notWithSharing)
{
	if (dbCalls.GetByAncs(ac).Count > 0)
	{
		noSharing.Add(ac);
	}
}

// We remove results that are in the header of a foreach statement, because these are copied in preprocessing
// to the beginning of the foreach block. When the preprocessing is changed, also this fix should be removed.
CxList inForEachHeader = All.FindByFathers(All.FindByType(typeof(ForEachStmt)));

// 1. global classes
CxList globals = noSharing.FindByFieldAttributes(Modifiers.Global);
CxList inGlobals = apexData.GetByAncs(globals);
CxList methodCalls = All.NewCxList();
CxList create = All.FindByType(typeof(ObjectCreateExpr));
CxList apexNotForeach = apexData - inForEachHeader;
foreach (CxList glob in globals)
{
	CxList inGlob = inGlobals.GetByAncs(glob);
	CxList allReferences = (apexNotForeach - inGlob).FindAllReferences(glob);
	methodCalls.Add(allReferences.GetMembersOfTarget().FindByType(typeof(MethodInvokeExpr)));
	methodCalls.Add(allReferences.GetByAncs(create));
}

CxList methodDefinitions = apexData.FindDefinition(methodCalls);
foreach (CxList mc in methodCalls)
{
	CxList target = methodDefinitions.FindDefinition(mc);
	result.Add(mc.Concatenate(target, true));
}

// 2. classes called by VF pages
CxList vfProblems = VFData.FindAllReferences(noSharing);
CxList sourceVF = vfProblems.GetAncOfType(typeof(MethodDecl)).FindByShortName("__*");
vfProblems -= vfProblems.GetByAncs(sourceVF);

CxList accessControl = vfProblems.GetAncOfType(typeof(ObjectCreateExpr));
CxList actions = accessControl.GetAncOfType(typeof(MethodInvokeExpr));
accessControl -= accessControl.GetByAncs(actions);

CxList webServices = apexData.FindByType(typeof(MethodDecl)).FindByFieldAttributes(Modifiers.WebService);
webServices = apexData.GetByAncs(webServices);
CxList wsProblems = webServices.FindAllReferences(noSharing);
accessControl.Add(wsProblems);
foreach (CxList ac in accessControl)
{
	CxList target = apexData.FindDefinition(VFData.FindByFathers(ac))
		+ apexData.FindDefinition(apexData * ac);
	result -= target;
	result.Add(ac.Concatenate(target, true));
}

// 3. classes that have "without sharing" but use DB.
CxList dbUsage = Find_DB();
result.Add(dbUsage.GetByAncs(notWithSharing).GetAncOfType(typeof(ClassDecl)));


result -= Find_Test_Code();