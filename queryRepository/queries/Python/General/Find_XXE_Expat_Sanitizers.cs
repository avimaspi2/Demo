/**************************
Objective: 
Return all the objects returned by the xml.parsers.expat.ParseCreate
that are affected by the method SetParamEntityParsing with the flag 
parameter: XML_PARAM_ENTITY_PARSING_NEVER.
/**************************/

//get all methods
CxList methods = Find_Methods();
//filter the methods called SetParamEntityParsing
CxList setFeatMthd = methods.FindByShortName("SetParamEntityParsing");
//get the params that are a flag with specific name: XML_PARAM_ENTITY_PARSING_NEVER
CxList sanitFlag = Find_Param().FindByShortName("XML_PARAM_ENTITY_PARSING_NEVER", true);
//now filter the SetParamEntityParsing methods that have the sanit flags as parameter
CxList relevantSetFeatMthd = setFeatMthd.FindByParameters(sanitFlag);
//finally obtain the parsers from which these methods are members
//These parsers are the sanitizers!!
result = relevantSetFeatMthd.GetTargetOfMembers();