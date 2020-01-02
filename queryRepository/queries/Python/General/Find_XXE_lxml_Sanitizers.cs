/**************************
Objective: 
Return all the objects returned by the lxml.etree.XMLParser
that are affected by the argument "resolve_entities" set to false.
/**************************/

//get all methods
CxList methods = Find_Methods();
//filter the methods called SetParamEntityParsing
CxList xml_parsers = methods.FindByMemberAccess("etree.XMLParser", true);
//get all parameters to narrow searches 
CxList parameters = Find_Param();

//get all the paramers of XMLParser()
CxList params_of_xml_parsers = parameters.GetParameters(xml_parsers);
//get all the boolean falses (True, true, 0)
CxList falses = Find_BooleanLiteral().FindByShortName("false", false);
falses.Add(Find_IntegerLiterals().FindByShortName("0", false));
//get all values from resolve_entities parameters that are set to false
CxList values_of_resolve_entities = Find_By_Param_Name("resolve_entities") * falses;
//get all the resolve_entities elements that are parameters of XMLParser
CxList resolve_entities = values_of_resolve_entities.GetFathers() * params_of_xml_parsers;

CxList relevant_xml_parsers = resolve_entities.GetAncOfType(typeof(MethodInvokeExpr));

//the XMLParsers are assumed as the sanitizers
result = relevant_xml_parsers;