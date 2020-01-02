// Find the unprotected cookies in hapiJs
CxList declarators = Find_Declarators();
CxList associativeArrayExprs = Find_AssociativeArrayExpr();
CxList booleanLiterals = Find_BooleanLiteral();
CxList unknownReferences = Find_UnknownReference();
CxList assignExprsDeclarators = Find_AssignExpr();

CxList server = Hapi_Find_Server_Instance();
CxList responseObjects = Hapi_Find_Response_Objects();

server.Add(responseObjects);
CxList serverState = server.GetMembersOfTarget().FindByShortName("state");

CxList paramsInState = unknownReferences.GetParameters(serverState, 1);
CxList associativeArrays = associativeArrayExprs.GetByAncs(serverState);

// Assigns and declarators
assignExprsDeclarators.Add(declarators);

CxList associativeArraysInAssigns = All.FindAllReferences(paramsInState).GetByAncs(assignExprsDeclarators).GetAssigner()
	.FindByType(typeof(AssociativeArrayExpr));

associativeArrays.Add(associativeArraysInAssigns);

List<string> settingsFields = new List<string> {"isSecure", "isHttpOnly"};
CxList declaratorInArray = declarators.FindByShortNames(settingsFields);

CxList falseLiterals = booleanLiterals.FindByShortName("false");

CxList unSecureCookies = All.NewCxList();
foreach(CxList array in associativeArrays)
{
	CxList secured = declaratorInArray.GetByAncs(array);
	CxList underSecured = secured.GetByAncs(falseLiterals.GetFathers());
	if(secured.Count < 2 || underSecured.Count == 2 || underSecured.Count == 1){
		unSecureCookies.Add(underSecured);
	}	
}

result = unSecureCookies;