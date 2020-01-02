/***************************
Objective:
Return a path from any XMLParser method (that is not sanitized)
to the various parsing methods (influenced by the XMLParsers and input).
The result are the parsing methods that are vulnerable to XXE.
***************************/

//find all methods
CxList methods = Find_Methods();
//find all the inputs
CxList inputs = Find_Inputs();
inputs.Add(inputs.GetMembersOfTarget());
inputs.Add(Find_DB_Out());

//find all the xml.parsers.expat.ParserCreate methods
CxList parser_creators = methods.FindByMemberAccess("etree.XMLParser", true);
//find all the parsing methods
CxList parse_mthds = methods.FindByMemberAccess("etree.XML", true);
parse_mthds.Add(methods.FindByMemberAccess("etree.parse", true));
parse_mthds.Add(methods.FindByMemberAccess("etree.parseid", true));
parse_mthds.Add(methods.FindByMemberAccess("etree.fromstring", true));
parse_mthds.Add(methods.FindByMemberAccess("etree.fromstringlist", true));
//find the parsing methods influenced by inputs
CxList relevant_parse_mthds = parse_mthds.DataInfluencedBy(inputs);
//Remove duplicates due to inputs.Add(inputs.GetMembersOfTarget())
relevant_parse_mthds = relevant_parse_mthds.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);

//collect method calls that have a single parameter
CxList implicitly_vulnerable_mthds = All.NewCxList();
foreach(CxList mthd in relevant_parse_mthds ) {
	MethodInvokeExpr mie = mthd.TryGetCSharpGraph<MethodInvokeExpr>();
	if(mie.Parameters.Count == 1){
		implicitly_vulnerable_mthds.Add(mthd);
	}
}

//find the sax sanitizers
CxList sax_sanitizers = Find_XXE_lxml_Sanitizers();

//finally obtain the parsing methods that are influenced by the non sanitized parsers
result = relevant_parse_mthds.InfluencedByAndNotSanitized(parser_creators, sax_sanitizers);
result.Add(implicitly_vulnerable_mthds);