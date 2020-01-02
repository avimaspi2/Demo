/***************************
Objective:
Return a path from any make_parser method (that is not sanitized)
to the parse or parseString methods (influenced by the makers and input).
The result are the parse and parseString methods that are vulnerable to XXE. 
***************************/

//find all methods
CxList methods = Find_Methods();
//find all the inputs
CxList inputs = Find_Inputs();
//find all the xml.parsers.expat.ParserCreate methods
CxList makeParsers = methods.FindByMemberAccess("sax.make_parser", true);
//find all the methods with name parse and parseString										
CxList parseMthds = methods.FindByShortName("parse", true);
parseMthds.Add(methods.FindByShortName("parseString", true));

//find the defintions of sax sanitizers
CxList saxSanitizers = All.FindDefinition(Find_XXE_Sax_Sanitizers());

//get all the assignments where make_parser methods are involved
CxList saxAssignments = makeParsers.GetAncOfType(typeof(AssignExpr));
//get the objects assigned with the result of a make_parser()
CxList saxObjectsViaMakers = All.GetByAncs(saxAssignments)
										.FindByAssignmentSide(CxList.AssignmentSide.Left);
//get the defintions
CxList saxObjectsViaMakersDefs = All.FindDefinition(saxObjectsViaMakers);

CxList relevantParseMethods = All.NewCxList();
foreach(CxList m in parseMthds){
	//get the definition of a parse method target element (as in target.member() )
	CxList saxObjDef = All.FindDefinition(m.GetTargetOfMembers());
	//if the definition of the target does not correspond to a sanitized sax_object and
	//it is a sax_object resulting from a sax.make_parser()
	//then we add it to the list of relevant unsanitized xml parsers.
	if( (saxObjDef * saxSanitizers).Count == 0 && (saxObjDef * saxObjectsViaMakersDefs).Count > 0 ){
		relevantParseMethods.Add(m);
	}
}

//now get all the parse methods influenced by inputs
relevantParseMethods = relevantParseMethods.DataInfluencedBy(inputs);
//finally obtain the flow from the make_parser to the vulnerable method
result = relevantParseMethods.DataInfluencedBy(makeParsers);