/*
This query finds Kohana unique interactive inputs
*/
CxList methods = Find_Methods();
CxList trc = All.FindByType(typeof(TypeRefCollection));


//handle Kohana requests and url params
CxList extend = All.FindByType(typeof(TypeRef)).GetByAncs(trc);
CxList extendController = extend.FindByShortName("*Controller*");
CxList controllerClass = extendController.GetAncOfType(typeof(ClassDecl));

CxList memberAccess = All.FindByType(typeof(MemberAccess));

CxList methodAndMemberAccess = methods + memberAccess;

CxList controllerRequest = methodAndMemberAccess.FindByShortNames(new List<string>
	{"body", "cookie", "headers","query","param","post","get","__toString"});

controllerRequest.Add(
	memberAccess.FindByShortNames(new List<string>
	{"_body", "_cookies", "_get", "_header", "_params", "_post"}));

//methods that create request instance
CxList getInst = 
	All.FindByMemberAccess("Request.factory") + 
	All.FindByMemberAccess("Request.initial") + 
	All.FindByMemberAccess("Request.current");

CxList thisR = All.FindByType(typeof(ThisRef));

CxList target = controllerRequest.GetTargetOfMembers();
CxList getInstInfluences = target.DataInfluencedBy(getInst);

result.Add((getInst + getInstInfluences).GetMembersOfTarget() * controllerRequest);

CxList controllerFieldAccess = thisR.FindAllReferences(controllerClass).GetMembersOfTarget();
controllerFieldAccess = controllerFieldAccess.FindByShortName("input") + controllerFieldAccess.FindByShortName("request");
CxList controllerInputMethods = controllerFieldAccess.GetMembersOfTarget() * controllerRequest;
result.Add(controllerInputMethods);