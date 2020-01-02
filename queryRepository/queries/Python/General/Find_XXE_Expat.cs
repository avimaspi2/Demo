/***************************
Objective:
Return a path from any ParserCreate method (that is not sanitized)
to the Parse methods (influenced by the creators and input).
The result are the Parse methods that are vulnerable to XXE.
NOTE: Expat is sanitized by default! 
***************************/

//find all the unsanitizers for Expat
CxList expatSanitizers = Find_XXE_Expat_Sanitizers();
//and now get the explicit sanitizers
CxList expatUnsanitizers = Find_XXE_Expat_Unsanitizers();
//get all the methods
CxList methods = Find_Methods();
//find all the inputs
CxList inputs = Find_Inputs();
//find all the xml.parsers.expat.ParserCreate methods
CxList parserCreators = methods.FindByMemberAccess("expat.ParserCreate", true);

//find all the methods with name Parse										
CxList parseMthds = methods.FindByShortName("Parse", true);
//filter those that are influenced by inputs
CxList relevantParseMethods = parseMthds.DataInfluencedBy(inputs);
//and those influenced by the unsanitizers
CxList unsanitized = relevantParseMethods.DataInfluencedBy(expatUnsanitizers);
//finally obtain the parsing methods that are influenced by the not sanitized parsers
//but only those that are explicitly unsanitized!
result = unsanitized.InfluencedByAndNotSanitized(parserCreators, expatSanitizers);