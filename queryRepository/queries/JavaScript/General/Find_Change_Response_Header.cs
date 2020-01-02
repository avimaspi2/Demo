CxList responses = Find_HTTP_Responses();
CxList responseMembers = responses.GetMembersOfTarget();

CxList headerChangingMethods = responseMembers.FindByShortNames(
	new List<string>(){"writeHead","set","setHeader","append","header"});
result = headerChangingMethods;