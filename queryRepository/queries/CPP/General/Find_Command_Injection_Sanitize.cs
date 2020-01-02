// Find all references of unknown references in conditions in this method and make them sanitizers, 
// because most likely they are well-checked now.
CxList methods = Find_Methods();
CxList unknownRefs = Find_UnknownReference();
result = unknownRefs.InfluencingOn(methods.FindByShortNames(new List<string>(){"regex_match","regex_search","regex_replace","isalnum","find"}));

CxList exec = Find_Cmd_Execute();
CxList relevantMethods = Find_MethodDecls().GetMethod(exec);
relevantMethods = methods.GetByAncs(relevantMethods) - exec;

CxList relevantConditions = Get_Conditions();
relevantConditions -= relevantMethods.GetByAncs(relevantMethods.FindByShortName("strncmp"));

result.Add(relevantMethods.FindAllReferences(relevantConditions));