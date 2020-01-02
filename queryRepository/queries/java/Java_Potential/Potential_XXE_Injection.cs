// Find Potential XXE (XML External Entity vulnerability) in Java

CxList potential_inputs = Find_Potential_Inputs();

CxList allXXE = Find_XXE_JAXP_DOM();

allXXE.Add(Find_XXE_JAXP_SAX());
allXXE.Add(Find_XXE_XMLReader());
allXXE.Add(Find_XXE_JAXB());
allXXE.Add(Find_XXE_StAX());
allXXE.Add(Find_XXE_DOM4J());
allXXE.Add(Find_XXE_XOM());

result = allXXE.InfluencedBy(potential_inputs);