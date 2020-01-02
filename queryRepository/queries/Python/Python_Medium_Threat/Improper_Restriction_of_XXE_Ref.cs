/************************
Objective: 
Find all the XML parser readers that are not 
sanitized for the XXE vulnerability.
************************/
result = Find_XXE_Expat();
result.Add(Find_XXE_Sax());
result.Add(Find_XXE_SaxSanitized_XMLReaders());
result.Add(Find_XXE_lxml());