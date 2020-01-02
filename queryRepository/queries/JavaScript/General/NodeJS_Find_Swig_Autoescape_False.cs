////////////////////////////////////////////////////////////////////////////////////////
// Query: NodeJS_Find_Swig_Autoescape_False
// Purpose: Find if SWIG output is not autoescaped and therefore prone to XSS
// Returns: The autoescape auto with false assignment. 
//          If result is empty then SWIG output is safe for XSS.
 
// Find if swig.init includes autoescape: false, in which case SWIG is vulnerable to XSS.
CxList swigInit = All.FindByShortName("swig").GetMembersOfTarget().FindByShortName("init");

CxList autoescape = All.FindByShortName("autoescape");
CxList _false = All.FindByShortName("false").FindByType(typeof(BooleanLiteral));

CxList aeFalse = autoescape.InfluencedBy(_false).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

result = aeFalse.InfluencingOn(swigInit);