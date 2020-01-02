// Query Potential_Ad_Hoc_Ajax
// The purpose of the query is to detect heuristic Ad Hoc Ajax vulnerability

CxList methodsEval = Find_Methods().FindByShortName("eval");
CxList xhrResponse = Find_XHR_Response();
result = methodsEval.DataInfluencedBy(xhrResponse);