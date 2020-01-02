CxList plist = Find_Plist_Elements();

CxList dictionarySettings = plist.FindByMemberAccess("NSAppTransportSecurity.NSExceptionDomains");
CxList ifStatements = plist.FindByType(typeof(IfStmt));
result.Add(dictionarySettings.FindByFathers(ifStatements));

CxList booleanValueKeys = plist.FindByMemberAccess("NSAppTransportSecurity.NSAllowsArbitraryLoads");
booleanValueKeys.Add(plist.FindByMemberAccess("NSAppTransportSecurity.NSAllowsArbitraryLoadsForMedia"));
booleanValueKeys.Add(plist.FindByMemberAccess("NSAppTransportSecurity.NSAllowsArbitraryLoadsInWebContent"));

CxList booleanSettingValues = booleanValueKeys.GetAssigner();
CxList unsafeBooleanTrueValues = booleanSettingValues.FindByShortName("true", false);
result.Add(unsafeBooleanTrueValues.GetAssignee());