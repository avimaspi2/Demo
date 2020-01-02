/**************************
Objective: 
Return all the objects returned by the xml.parsers.expat.ParseCreate
that are affected by the method SetParamEntityParsing with the flags
parameter: XML_PARAM_ENTITY_PARSING_ALWAYS and XML_PARAM_ENTITY_PARSING_UNLESS_STANDALONE
/**************************/

//get all methods
CxList methods = Find_Methods();
//filter the methods called SetParamEntityParsing
CxList setFeatMthd = methods.FindByShortName("SetParamEntityParsing");
CxList parameters = Find_Param();
//get the params that are a flag with specific name: XML_PARAM_ENTITY_PARSING_NEVER
CxList unsanitFlag = parameters.FindByShortName("XML_PARAM_ENTITY_PARSING_ALWAYS", true);
unsanitFlag.Add(parameters.FindByShortName("XML_PARAM_ENTITY_PARSING_UNLESS_STANDALONE", true));
//now filter the SetParamEntityParsing methods that have the unsanit flags as parameter
CxList relevantSetFeatMthd = setFeatMthd.FindByParameters(unsanitFlag);
//finally obtain the parsers from which these methods are members
//These parsers are the sanitizers!!
result = relevantSetFeatMthd.GetTargetOfMembers();