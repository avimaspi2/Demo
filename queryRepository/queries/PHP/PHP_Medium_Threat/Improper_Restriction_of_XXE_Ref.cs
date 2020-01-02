CxList inputs = Find_Interactive_Inputs();
CxList xxe_results = Find_XXE_SimpleXml() + Find_XXE_XmlDOM() + Find_XXE_XMLReader();
CxList xxe_results_vulnerable = xxe_results - Find_XXE_Sanitizers();

result = xxe_results_vulnerable.DataInfluencedBy(inputs);