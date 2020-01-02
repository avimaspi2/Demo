// Find XXE (XML External Entity vulnerability) in Java
CxList inputs = Find_Interactive_Inputs();

CxList xxeData = Find_XXE_JAXP_DOM();
xxeData.Add(Find_XXE_JAXP_SAX());
xxeData.Add(Find_XXE_XMLReader());
xxeData.Add(Find_XXE_JAXB());
xxeData.Add(Find_XXE_StAX());
xxeData.Add(Find_XXE_DOM4J());
xxeData.Add(Find_XXE_XOM());
xxeData.Add(Find_XXE_Spring());

// All integers are sanitizers with the exception of Bytes
// because bytes are used to read XML
CxList sanitizers = Find_Integers();
sanitizers -= Find_Bytes();
result = xxeData.InfluencedByAndNotSanitized(inputs, sanitizers);