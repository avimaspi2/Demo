CxList variables = All.FindByType(typeof(Dom.UnknownReference));
variables = variables.FindByShortNames(new List<string>	{"_POST", "_GET", "_REQUEST","_COOKIE","_FILES"});
//	variables.FindByShortName("_POST") + 
//	variables.FindByShortName("_GET") +
//	variables.FindByShortName("_REQUEST") +
//	variables.FindByShortName("_COOKIE") +
//	variables.FindByShortName("_FILES");
	
//for zend left side sanitize
CxList zii = Find_Zend_Interactive_Inputs();
//Check for IndexerRef assignments
CxList findType = variables;
findType = (findType + zii).GetFathers().FindByType(typeof(AssignExpr));//check for assignment father

//get IndexerRef sons that are on the left
findType = All.FindByType(typeof(IndexerRef)).FindByAssignmentSide(CxList.AssignmentSide.Left).GetByAncs(findType);
findType = findType.FindByShortName(variables);
//go back to the original UnknownReferences
findType = All.FindByType(typeof(UnknownReference)).GetByAncs(findType.GetFathers().FindByType(typeof(AssignExpr)));

findType = findType * variables + (findType * zii);
result = findType;