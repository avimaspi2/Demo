result = Find_Integers();
CxList methods = Find_Methods();
result.Add(Find_Unarys().FindByShortName("!"));

CxList MemberAccess = Find_MemberAccesses();
result.Add(methods.FindByShortNames(new List<string>{"count","indexOf",
		"createTextNode","parseInt","parseFloat"}));
result.Add(methods.FindByShortNames(new List<string> {
		"escape", "*encode*"}, false));
result -= methods.FindByShortName("*unencode*");
CxList methodAccess = MemberAccess.Clone();
methodAccess.Add(methods);
result.Add(methodAccess.FindByShortNames(new List<string>{"Number","length"}));
result.Add(MemberAccess.FindByShortName("port"));
result.Add(Find_Replace_Param());

result.Add(JQuery_Sanitize());
result.Add(AngularJS_Find_Sanitize());
result.Add(Angular_Find_Sanitize());
//adds a sanitizer that is relevant to web messaging
//in case the origin is checked we can add the web messaging input to the sanitizers list.
CxList ifStmt = Find_Ifs();
CxList ifStmtCond = (All - Find_StatementCollection()).FindByFathers(ifStmt);
CxList origin = All.FindByShortName("origin").GetByAncs(ifStmtCond);
origin -= origin.GetMembersOfTarget().GetTargetOfMembers();
ifStmt = origin.GetAncOfType(typeof(IfStmt));
CxList wmi = Find_Web_Messaging_Inputs().GetByAncs(ifStmt);
result.Add(wmi);

// Regex Tests
List<string> regObjs = new List<string>{"regex","regexp","reg"};
CxList regexTests = methods.FindByShortName("test", false);
result.Add(regexTests.GetTargetOfMembers().FindByShortNames(regObjs, false).GetMembersOfTarget());
regexTests.Add(regexTests.FindByMemberAccess("RegExp.test"));

// The element's nodeValue attribute may contain a damaging payload.
// The createComment function disables it because commented scripts aren't interpreted by Typescript. 
result.Add(methods.FindByShortName("createComment"));