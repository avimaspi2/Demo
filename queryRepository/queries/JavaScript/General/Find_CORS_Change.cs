CxList responses = Find_HTTP_Responses();
CxList responseMembers = responses.GetMembersOfTarget();

CxList headerChangingMethods = responseMembers.FindByShortNames(
	new List<string>(){"writeHead","set"});

CxList parameters = All.GetParameters(headerChangingMethods);
CxList parametersOrigin = parameters.FindByRegex("Access-Control-Allow-Origin");
CxList parametersWildCard = parameters.FindByRegex("[\"\']\\*[\"\']");
CxList parametersVuln = parametersOrigin * parametersWildCard;

CxList require =  All.FindByShortNames(new List<string>(){"require"}).FindByType(typeof(MethodInvokeExpr));
CxList requireCors = All.GetParameters(require).FindByShortName("cors");

CxList optionsOrigin = Find_Declarators().FindByShortName("origin");
CxList optionsWildcard = optionsOrigin.FindByRegex("[\"\']\\*[\"\']");

CxList requireCorsData = All.DataInfluencedBy(requireCors);
CxList requireCorsDataCalls = All.FindAllReferences(requireCorsData);
CxList requireCorsDataCallsVuln = requireCorsDataCalls.InfluencedBy(optionsWildcard);

result = requireCorsDataCallsVuln;
result.Add(parametersVuln);