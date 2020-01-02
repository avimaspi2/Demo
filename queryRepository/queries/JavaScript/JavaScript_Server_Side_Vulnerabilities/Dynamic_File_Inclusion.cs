/*
	Find all Interactive Inputs that influences on file inclusion and not sanitized
*/
CxList inputs = NodeJS_Find_Interactive_Inputs();
CxList sanitize = NodeJS_Find_Path_Traversal_Sanitize();

CxList methodInvoke = Find_Methods();
CxList onlyParams = Find_Parameters();
CxList mA = Find_MemberAccesses();
CxList urMA = Find_UnknownReference();
urMA.Add(mA);
//find all file inclusion
CxList allRequire = methodInvoke.FindByShortName("require");
CxList requireLibrary = onlyParams.GetParameters(allRequire, 0);

CxList potentialParams = urMA.GetByAncs(requireLibrary);
result = potentialParams.InfluencedByAndNotSanitized(inputs, sanitize);