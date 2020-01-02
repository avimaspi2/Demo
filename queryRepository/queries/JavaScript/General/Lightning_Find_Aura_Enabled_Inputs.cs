CxList allApex = Get_Apex();
 CxList auraEnabled = allApex.FindByType(typeof(CustomAttribute)).FindByShortName("auraenabled", false);
 CxList auraEnabledMethod = auraEnabled.GetAncOfType(typeof(MethodDecl));
result = allApex.GetParameters(auraEnabledMethod);