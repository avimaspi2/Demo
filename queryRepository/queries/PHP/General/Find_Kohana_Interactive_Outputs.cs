/*
This query finds Kohana unique interactive outputs
*/
CxList methods = Find_Methods();
CxList trc = All.FindByType(typeof(TypeRefCollection));


//handle Kohana responses
CxList extend = All.FindByType(typeof(TypeRef)).GetByAncs(trc);
CxList extendController = extend.FindByShortName("*Controller*");
CxList controllerClass = extendController.GetAncOfType(typeof(ClassDecl));

CxList memberAccess = All.FindByType(typeof(MemberAccess));

CxList methodAndMemberAccess = methods + memberAccess;

CxList controllerResponse = methodAndMemberAccess.FindByShortNames(new List<string>
	{"body", "cookie", "headers","send_file","send_headers", "__toString"});

controllerResponse.Add(memberAccess.FindByShortNames(new List<string>{"body", "_cookies", "_header"}));

//methods that create response instance
CxList getInst = 
	All.FindByMemberAccess("Response.factory") +
	All.FindByMemberAccess("Request.create_response") + 
	All.FindByMemberAccess("Request.response");


CxList thisR = All.FindByType(typeof(ThisRef));

CxList target = controllerResponse.GetTargetOfMembers();
CxList getInstInfluences = target.DataInfluencedBy(getInst);

result.Add((getInst + getInstInfluences).GetMembersOfTarget() * controllerResponse);

CxList controllerFieldAccess = thisR.FindAllReferences(controllerClass).GetMembersOfTarget();
controllerFieldAccess = controllerFieldAccess.FindByShortNames(new List<string>
	{"output", "_response", "response"});
//	controllerFieldAccess.FindByShortName("output") +
//	controllerFieldAccess.FindByShortName("_response") + 
//	controllerFieldAccess.FindByShortName("response");
CxList controllerInputMethods = controllerFieldAccess.GetMembersOfTarget() * controllerResponse;
result.Add(controllerInputMethods);