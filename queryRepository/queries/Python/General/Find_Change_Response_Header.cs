// 1 - Find methods to change headers in http responses
CxList responses = Find_HTTP_Responses();

CxList responseMembers = responses.GetMembersOfTarget();
CxList headersMemberAccess = responseMembers.FindByShortName("headers");
headersMemberAccess.Add(All.FindAllReferences(headersMemberAccess.GetAssignee()));

CxList headerMethods = headersMemberAccess.GetMembersOfTarget().FindByShortNames(
	new List<string>(){"set","add"});

/* these aren't actually of type IndexerRef because in python IndexerRefs are internally converted to MemberAccess */
CxList headerIndexerRefs = headersMemberAccess.GetMembersOfTarget()
	.GetByAncs(Find_AssignExpr());

CxList responseConstructor = responses.GetAssigner().FindByMemberAccess("flask.Response");

// 2 - Request Handlers method send_headers also allow to change headers in reponses
// 2.1 - Find all objects in classes that inherits form HTTP 3 clases
CxList allClasses = Find_ClassDecl();

CxList inheritants = allClasses.InheritsFrom("BaseHTTPRequestHandler");
inheritants.Add(allClasses.InheritsFrom("CGIHTTPRequestHandler"));
inheritants.Add(allClasses.InheritsFrom("SimpleHTTPRequestHandler"));

CxList allInInheritClasses = All.GetByAncs(inheritants);

//2.2 - Find all this objects of classes that inherits from 3 HTTP classes
CxList thisOfInherits = allInInheritClasses.FindByType(typeof(ThisRef));

CxList membersOfThis = thisOfInherits.GetMembersOfTarget();

CxList sendHeaderMethods = membersOfThis.FindByShortName("send_header");

result.Add(headerMethods);
result.Add(headerIndexerRefs);
result.Add(All.GetByAncs(responseConstructor));
result.Add(sendHeaderMethods);