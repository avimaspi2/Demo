/**************************
Objective: 
Return all the objects returned by the xml.parsers.expat.make_parser
that are affected by the method setFeature with the flag 
parameters: xml.sax.handler.feature_external_pes and xml.sax.handler.feature_external_ges
set to false.
/**************************/

//get all methods
CxList methods = Find_Methods();
//filter the methods called SetParamEntityParsing
CxList set_feat_mthd = methods.FindByShortName("setFeature", true);
//get all parameters to narrow searches 
CxList parameters = Find_Param();
//get all the params that are a flag with specific name: *handler.feature_external_pes or _ges
CxList sanit_flags = parameters.FindByName("*handler.feature_external_pes", false);
sanit_flags.Add(parameters.FindByName("*handler.feature_external_ges", false));
//now filter the setFeature methods that have the sanit flags as parameter
CxList relevant_set_feat_mthd = set_feat_mthd.FindByParameters(sanit_flags);

//get the boolean falses (any of True, true or 0)
CxList falses = Find_BooleanLiteral().FindByShortName("false", false);
falses.Add(Find_IntegerLiterals().FindByShortName("0", false));
falses.Add(Find_UnknownReference().FindByShortName("false", false));

//find the setFeature methods that have false as argument
relevant_set_feat_mthd = relevant_set_feat_mthd.FindByParameters(falses);

//finally obtain the parsers from which these methods are members
//These parsers are the sanitizers!!
result = relevant_set_feat_mthd.GetTargetOfMembers();