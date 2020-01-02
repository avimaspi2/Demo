CxList methods = Find_Methods();
CxList inputs = Find_Interactive_Inputs();
CxList sanitize = Find_Sanitize();

// redirect_to and link_to
CxList redirectFunctions = 
	methods.FindByShortName("redirect_to", false) +
	methods.FindByShortName("link_to", false);
CxList secondParam = All.GetByAncs(All.GetParameters(redirectFunctions, 1));
CxList secondParamTrue = secondParam.FindByShortName("true");
CxList secondParamOnlyPath = secondParam.FindByShortName("only_path");
CxList onlyPathTrue = secondParamTrue.GetAncOfType(typeof(Param)) * secondParamOnlyPath.GetAncOfType(typeof(Param));
redirectFunctions -= redirectFunctions.FindByParameters(onlyPathTrue);

// URI_Parser
CxList URIParser = All.FindByType("URI.Parser");
CxList URIParse = methods.FindByMemberAccess("URI.parse");
URIParse.Add(URIParser.GetMembersOfTarget().FindByShortName("parse"));
redirectFunctions.Add(URIParse);

// update sanitizer to contain all but first param
CxList redirectParam = All.GetByAncs(All.GetParameters(redirectFunctions, 0));
sanitize.Add(All.GetByAncs(All.GetParameters(redirectFunctions)) - redirectParam);

result = redirectFunctions.InfluencedByAndNotSanitized(inputs, sanitize);