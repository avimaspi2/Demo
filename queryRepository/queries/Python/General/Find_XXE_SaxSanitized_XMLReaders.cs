/***************************
for: Minidom and Pulldom readers that are XML-DOM parsers that may take a 
SAX parser for conveying sanitization. 
Objective:
Return a path from any make_parser (Sax parser) method (that is not sanitized)
to the parse or parseString methods (Minidom and Pulldom)
In this case, the make_parser object shall be argument of these XML readers. 
The result are the parse and parseString methods that are vulnerable to XXE. 
***************************/

//find all methods
CxList methods = Find_Methods();
//find all the inputs
CxList inputs = Find_Inputs();
//find all the sax sanitizers
CxList sax_sanitizers = Find_XXE_Sax_Sanitizers();
	
//find all the xml.parsers.expat.ParserCreate methods
CxList make_parsers = methods.FindByMemberAccess("sax.make_parser", true);
//find all the methods with name parse and parseString from minidom and pulldom										
CxList parse_mthds = methods.FindByMemberAccess("minidom.parse", true);
parse_mthds.Add(methods.FindByMemberAccess("minidom.parseString", true));
parse_mthds.Add(methods.FindByMemberAccess("pulldom.parse", true));
parse_mthds.Add(methods.FindByMemberAccess("pulldom.parseString", true));

//get all the parsing methods that are influenced by inputs
CxList relevant_parse_mthds = parse_mthds.DataInfluencedBy(inputs);

//collect method calls that have a single parameter
CxList implicitly_vulnerable_mthds = All.NewCxList();
foreach(CxList mthd in relevant_parse_mthds.GetCxListByPath() ) {
	MethodInvokeExpr mie = mthd.TryGetCSharpGraph<MethodInvokeExpr>();
	if(mie.Parameters.Count == 1){
		implicitly_vulnerable_mthds.Add(mthd);
	}
}

//get the relevant methods that are influenced by sax sanitizers 
CxList mthds_sax_sanitzed = relevant_parse_mthds.DataInfluencedBy(sax_sanitizers)
								.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
//get the remaining relevant methods
CxList mthds_not_sax_sanitized = relevant_parse_mthds - mthds_sax_sanitzed; 

//finally obtain the parsing methods that are influenced by the non sanitized parsers
result = mthds_not_sax_sanitized.DataInfluencedBy(make_parsers);
//and add those that have only one parameter (which are always vulnerable)
result.Add(implicitly_vulnerable_mthds);