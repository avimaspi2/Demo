/*
	This query searches for X-FRAME-OPTIONS in login contexts, wich are set to allow
	framing of their respective forms, therefore becoming vulnerable to clickjacking.

	It is possible to embed these forms into a frame, iframe or object HTML elements
	if the X-FRAME_OPTION header is not set or setted to allow such framing.

	If the header option is not set, the query highlights the function encapsulating
	the login routine. In case the x-frame-option is set to allow all, the vulnerability
	is marked on the "ALLOW-ALL" parameter.
*/

CxList methodDecls = Find_MethodDecls();
methodDecls.Add(Find_LambdaExpr());
CxList methods = Find_Methods();
CxList unknownRefs = Find_UnknownReference();
CxList strings = Find_String_Literal();
CxList parameters = Find_ParamDecl();

CxList loginRequestPath = strings.FindByShortNames(new List<string>{"*login*", "*auth*", "*signin*"});
CxList methodsWithAuthRoute = methods.FindByParameters(loginRequestPath);

CxList safeXFrameValue = strings.FindByShortNames(new List<string>{"*deny*", "*sameorigin*", "*allow-from*", "*allow\\-from*"}, false);
CxList xFrameOptions = strings.FindByShortName("*x-frame-options*", false);
CxList sanitizers = All.NewCxList();
// Typically, Typescript serverside contains HTTP handlers holding requests and responses as parameters.
CxList requestParameters = parameters.FindByShortName("req*");
CxList responseParameters = parameters.FindByShortName("res*");
CxList expressParams = NodeJS_Find_Express_Callback_Params();
// Add Express params, as we can sanitize them also using set header
requestParameters.Add(expressParams);
responseParameters.Add(expressParams);

// Using GetFathers() two times as FindByParameters does not take into account lambdas
//CxList methodsWithRequestAndResponse = methodDecls.FindByParameters(responseParameters).FindByParameters(requestParameters);
CxList methodsWithRequestAndResponse = methodDecls * responseParameters.GetFathers().GetFathers();
CxList targetsOfResponses = unknownRefs.FindAllReferences(responseParameters.GetParameters(methodsWithRequestAndResponse)).GetMembersOfTarget();

// Both response.setHeader(headername, headervalue), 
// response.append(headername, headervalue) and set(headername, headervalue),
// allow setting a single header value.
CxList appendingHeaderMethods = targetsOfResponses.FindByShortNames(new List<string>{"setHeader", "append", "set"});
sanitizers.Add(appendingHeaderMethods.FindByParameters(xFrameOptions).FindByParameters(safeXFrameValue));
result.Add(appendingHeaderMethods.GetByAncs(methodsWithAuthRoute) - sanitizers);

// The X-Frame-Options header can also be asign using responde.headers['X-Frame-Options'] = ...
CxList headerAccesses = targetsOfResponses.FindByShortName("headers");
CxList safeXFrameOptions = headerAccesses.GetFathers() * safeXFrameValue.GetAssignee();
sanitizers.Add(headerAccesses.FindByFathers(safeXFrameOptions));

// Remove also other headers from results
CxList safeIndexerRefHeaders = headerAccesses.GetFathers() - xFrameOptions.GetFathers();
CxList safeMemberAccessHeaders = headerAccesses.GetMembersOfTarget() - xFrameOptions;
CxList notXFrameOptions = headerAccesses.FindByFathers(safeIndexerRefHeaders);
notXFrameOptions.Add(safeMemberAccessHeaders.GetTargetOfMembers());
sanitizers.Add(notXFrameOptions);

result.Add(headerAccesses - sanitizers);

// response.writeHead can be used to set multiple headers at the same time.
// EX: response.writeHead(200, {'header1' : 'valueofheader1', 'X-Frame-Options' : 'DENY'})
CxList frameFields = Find_FieldDecls().FindByShortName("X-Frame-Options", false);
CxList safeFrameFields = frameFields * safeXFrameValue.GetAncOfType(typeof(FieldDecl));
sanitizers.Add(safeFrameFields.GetByAncs(targetsOfResponses.FindByShortName("writeHead")));
result.Add(frameFields - sanitizers);

// Remove method declarations which contain a sanitizer in them.
CxList SanitizedMethods = methodsWithRequestAndResponse.GetMethod(sanitizers);
SanitizedMethods.Add(sanitizers.GetAncOfType(typeof(LambdaExpr)));
methodsWithRequestAndResponse -= SanitizedMethods;

CxList RequestHandlers = All.FindAllReferences(All.FindAllReferences(methodsWithRequestAndResponse).GetTargetOfMembers());

// Possible login paths
//Expressjs uses express().post or express().get to define routes to HTTP requests.
CxList requests = expressParams.GetAncOfType(typeof(MethodInvokeExpr));
CxList postMethods = methods.FindByParameters(loginRequestPath.GetParameters(requests, 0));

// Check if helmet is being used to sanitize Express app methods, using frameguard method
CxList expressRefs = Find_Require("express", 2);
CxList inflBySafeValues = methods.DataInfluencedBy(safeXFrameValue)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly).GetTargetOfMembers();
CxList helmetUsedInExpress = inflBySafeValues * expressRefs;
if (helmetUsedInExpress.Count > 0) {
	// Express methods are sanitized
	postMethods -= expressRefs.GetMembersOfTarget();
}

//The module.exports is often used to define routines as well.
CxList exportsMethods = All.FindByShortName("cxExports*").GetMembersOfTarget().GetAssigner() * methodsWithRequestAndResponse;

// Ensure the results are presented where the configuration is done
CxList UnsafeFrameFields = frameFields - safeFrameFields;
CxList exportMethodsWithXFrameOptions = methodsWithRequestAndResponse.GetMethod(UnsafeFrameFields);
exportMethodsWithXFrameOptions.Add(UnsafeFrameFields.GetAncOfType(typeof(LambdaExpr)));

result.Add(postMethods.FindByParameters(RequestHandlers));
result.Add(exportsMethods - exportMethodsWithXFrameOptions);