// Find unscrubbed sensitive data
// Make sure sensitive data such as password is overwritten (using secured type) before being released.
CxList vulnerableResults = All.NewCxList();
try{	
	CxList casesExpr = Find_Cases();
	
	CxList passwords = Find_Passwords();
	CxList passwordsDefinitions = All.FindDefinition(passwords);	
	
	string[] allowedTypes = new string[] {"NSMutableString*", "CFMutableString", 
		"CFMutableStringRef", "false", "char*", "CChar",  "char[]", "UniChar*", "UniChar[]"};
	
	vulnerableResults = passwordsDefinitions - passwordsDefinitions.FindByTypes(allowedTypes);
	vulnerableResults -= vulnerableResults.GetAssigner().FindByShortName("withUnsafeMutablePointer").GetAssignee();
	vulnerableResults -= vulnerableResults.GetAssigner().FindByType(typeof(BooleanLiteral)).GetAssignee();
	
	//remove methods from the results (as methods are here considered because 
	//their names are built from the concatenation of their parameters name.
	CxList toRemove = vulnerableResults.FindByType(typeof(MethodDecl));
	toRemove.Add(vulnerableResults.FindByType(typeof(ClassDecl)));	
	toRemove.Add(All.FindDefinition(passwords.FindByFathers(casesExpr)));
	vulnerableResults -= toRemove;
}
catch (Exception error)
{
	cxLog.WriteDebugMessage(error);
}
result = vulnerableResults;