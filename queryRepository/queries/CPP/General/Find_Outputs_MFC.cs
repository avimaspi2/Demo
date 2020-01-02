CxList methodDecl = Find_Method_Declarations();

CxList dispPropertyEx = All.GetByAncs(All.GetParameters(methodDecl.FindByShortNames(
	new List<string> {
		"DISP_PROPERTY_EX", 
		"DISP_PROPERTY_PARAM"}), 2));

CxList outputMethods = methodDecl.FindByShortName(dispPropertyEx);
CxList returnValue = All.GetByAncs(Find_ReturnStmt().GetByAncs(outputMethods));

result = All.FindAllReferences(returnValue);