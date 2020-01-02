/* Includes inputs to auraEnabled methods */

CxList custAttr = All.FindByType(typeof(CustomAttribute)).FindByShortName("auraenabled", false);

CxList matchedMethods = custAttr.GetAncOfType(typeof(MethodDecl));

result= All.FindByType(typeof(ParamDecl)).GetParameters(matchedMethods);