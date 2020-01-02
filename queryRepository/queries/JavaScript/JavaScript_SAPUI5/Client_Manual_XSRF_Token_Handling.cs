// Fiori application should not manually manage csrf tokens, but let the SAPUI5 framework handle them
// except for File upload scenario where this is currently unavoidable.
if(cxScan.IsFrameworkActive("SAPUI") && Find_SAP_Library().Count > 0)
{
	CxList declarators = Find_Declarators();
		
	List<string> csrfTokens = new List<string> {"x-csrf-token"};
	List<string> csrfHandlingMethods = new List<string> {"getResponseHeader"};
	CxList setHeadersMethodInvokes = Find_Methods().FindByMemberAccess("headers.set");
	CxList csrfTokenDeclarator = declarators.FindByShortNames(csrfTokens, false);
	CxList literalTokens = Find_String_Literal().FindByShortNames(csrfTokens, false);
		
	result = literalTokens.GetAncOfType(typeof(IndexerRef));
	result.Add(literalTokens.GetAncOfType(typeof(MethodInvokeExpr)).FindByShortNames(csrfHandlingMethods));
	result.Add(csrfTokenDeclarator.GetByAncs(declarators.FindByShortName("headers")));
	result.Add(literalTokens.GetByAncs(setHeadersMethodInvokes));
}