string br = "*bind_result";
List < string > fetchers = new List<string>(){"*fetch_row","*fetch_array","*fetch_assoc","*fetch_field","*fetch_object",
		"fetch_row","fetch_all","*fetch_field_direct",br};
//regular fetchers:
result = Find_MySQL_Related_Queries(fetchers);
//bound results:
CxList bindResult = result.FindByShortName(br);
result -= bindResult;
CxList declarators = All.FindByType(typeof(Declarator));
CxList parametersOfBindResult = All.GetParameters(bindResult);
//remove first parameter of procedural
CxList stmt = parametersOfBindResult.GetParameters(bindResult.FindByShortName("*stmt_bind_result"), 0);
parametersOfBindResult -= stmt;
result.Add((All - declarators - parametersOfBindResult).FindAllReferences(parametersOfBindResult));