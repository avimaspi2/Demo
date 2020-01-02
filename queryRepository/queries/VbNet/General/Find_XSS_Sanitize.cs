CxList methods = Find_Methods();

CxList repl = methods.FindByMemberAccess("String.Replace", false);
repl.Add(methods.FindByMemberAccess("Stringbuilder.Replace", false));
repl = All.GetParameters(repl, 0);

CxList guid= All.FindByType(typeof(ObjectCreateExpr)).FindByShortName("Guid", false); 

CxList htmlSafer = methods.FindByShortName("GetSafeHtmlFragment", false)
	.GetTargetOfMembers().GetMembersOfTarget();

result = Find_XSS_Replace();
result.Add(Find_Encode());
result.Add(Find_Integers()); 
result.Add(repl);
result.Add(guid);
result.Add(htmlSafer);
 result.Add(methods.FindByShortName("GetDateTime", false));