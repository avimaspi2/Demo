///////////////////////////////////////////////////////////////////////////////////
// Query Input_Not_Normalized
// Purpose: Find strings originated in Input and not Canonicalized (Normalized)
//          before comparing it
///////////////////////////////////////////////////////////////////////////////////
CxList integers = Find_Integers();
CxList methods = Find_Methods();

CxList strings = All.FindByType("String");
strings.Add(methods.FindAllReferences(Find_MethodDecls().FindByReturnType("String")));
CxList inputs = Find_Inputs();

// Find places where the string is compared
CxList compare = Find_String_Compare();
compare -= Find_Excludes_From_Path_Compare(compare);

// Map functions (get/put) have an implicit comparation
CxList mapGets = methods.FindByMemberAccess("*Map.get");
CxList mapPuts = methods.FindByMemberAccess("*Map.put");
CxList mapFuncs = All.NewCxList();
mapFuncs.Add(mapGets);
mapFuncs.Add(mapPuts);
CxList mapCompare = mapFuncs.FindByParameters(strings);

CxList mapSanitizers = All.GetByAncs(All.GetParameters(mapPuts, 1));
mapSanitizers.Add(mapFuncs.GetTargetOfMembers());

// Find string normalization
CxList sanitize = methods.FindByMemberAccess("Normalizer.normalize");
sanitize.Add(integers);

// Add Intern method as a sanitizer.
//Intern returns a canonical representation for the string object.
CxList intern = methods.FindByMemberAccess("String.intern");
sanitize.Add(intern);

sanitize -= compare;
sanitize -= mapCompare;

mapSanitizers.Add(sanitize);

result = inputs.InfluencingOnAndNotSanitized(mapCompare, mapSanitizers).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);

result.Add(inputs.InfluencingOnAndNotSanitized(compare, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow));