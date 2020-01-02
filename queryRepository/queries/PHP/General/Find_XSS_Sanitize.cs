CxList methods = Find_Methods();

//Add normal sanitizers
result = Find_General_Sanitize();
result.Add(Find_Replace());
result.Add(Find_HTML_Encode());
result.Add(Find_Regex());		
result.Add(Find_File_Access());
result.Add(Find_WP_Sanitize());
result.Add(Find_Left_Side_Sanitize());
result.Add(Find_Zend_XSS_Sanitize());
result.Add(Find_Kohana_XSS_Sanitize());
result.Add(Find_Smarty_XSS_Sanitize());
result.Add(Find_Cake_XSS_Sanitize());
result.Add(Find_DB_In());

// Json_Encode is by default a xss sanitize, but if flag JSON_UNESCAPED_SLASHES is used, it is not sanitized
CxList json_encodeMethods = methods.FindByShortName("json_encode");
CxList secondParam = All.GetParameters(json_encodeMethods, 1).FindByType(typeof(UnknownReference));
CxList secondParamJSON_UNESCAPED_SLASHES = secondParam.FindByShortName("JSON_UNESCAPED_SLASHES", false);
CxList clean_json_encodeMethods = json_encodeMethods - json_encodeMethods.FindByParameters(secondParamJSON_UNESCAPED_SLASHES);
result.Add(clean_json_encodeMethods);

//some file access methods that read from file and automatically output the contents (which are not sanitizers)
//Remove these methods and their first arguments (added in Find_File_Access)
CxList non_sanitizer_filereaders = methods.FindByShortNames(new List<string>(){"readfile"});
non_sanitizer_filereaders.Add(All.GetParameters(non_sanitizer_filereaders, 0));
result -= non_sanitizer_filereaders;

	
// Some methods considered sanitizers in php
result.Add(methods.FindByShortNames(new List<string> {
		"urlencode", "strtotime","gmdate", "base64_encode", "rawurlencode",
		"number_format*","filter_var","fgetss","apply_filters","array_walk_recursive",
		"array_walk","parse_url","http_build_query","var_dump","getById","filter_var_array"}, false));
result.Add(methods.FindByShortName("*escape*"));

// Add the paramter of call_user functions
CxList call_user_func = methods.FindByShortNames(new List<string> {"call_user_func","call_user_func_array"});
CxList user_func_first_param = methods.GetParameters(call_user_func, 0);
user_func_first_param -= user_func_first_param.FindByShortName("array", false);
CxList parameters = All.GetParameters(call_user_func);
CxList user_func_params = parameters - parameters.GetByAncs(user_func_first_param);
result.Add(user_func_params);

// Find short query repository->findAll(condition)
CxList unknownRef = All.FindByType(typeof(UnknownReference)) + All.FindByType(typeof(MemberAccess));
CxList repository = methods.FindByShortNames(new List<string>{"getRepository","getEntityManager"});
repository.Add(unknownRef.FindByShortNames(new List<string> {"repository","entityManager"}));
repository.Add(All.FindByShortName("Doctrine*"));
CxList find = repository.GetMembersOfTarget();
result.Add(find.FindByShortNames(new List<string> {"find","findOneBy","findAll","findBy*"}));


CxList thisRef = All.FindByType(typeof(ThisRef));
result.Add(methods * ((Find_Framework_Views() * thisRef).GetMembersOfTarget()));


// Add all references of variables compared to string in an if statement
CxList strings = Find_Strings();
CxList fatherOfUnknown = unknownRef.GetFathers();
CxList equalsConditions = Find_Conditions().FindByShortName("==");
CxList stringInCondition = strings.FindByFathers(equalsConditions); // strings in condition statement
CxList relevantequalsConditions = fatherOfUnknown * equalsConditions; // condition statements with string
CxList relevantVars = unknownRef.FindByFathers(relevantequalsConditions); // vars in string relates condition statements

// For every variable compared to a string in a condition statement, look for all of its references inside
// this if statement
foreach (CxList relevantVar in relevantVars)
{
	try
	{
		CxList ifStmt = relevantVar.GetAncOfType(typeof(IfStmt));
		IfStmt g = ifStmt.data.GetByIndex(0) as IfStmt;
		if(g != null)
		{ // leave only the variables in the true statement of the if statements
			StatementCollection s = g.TrueStatements;
			CxList inIf = unknownRef.GetByAncs(All.FindById(s.NodeId));
			result.Add(inIf.FindAllReferences(relevantVar));
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

// and same thing for the != case
CxList diffConditions = Find_Conditions().FindByShortName("!=");
stringInCondition = strings.FindByFathers(diffConditions); // strings in condition statement
CxList relevantDiffConditions = fatherOfUnknown * diffConditions; // condition statements with string
relevantVars = unknownRef.FindByFathers(relevantDiffConditions); // vars in string relates condition statements

// For every variable compared to a string in a condition statement, look for all of its references inside
// this if statement
foreach (CxList relevantVar in relevantVars)
{
	try
	{
		CxList ifStmt = relevantVar.GetAncOfType(typeof(IfStmt));
		IfStmt g = ifStmt.data.GetByIndex(0) as IfStmt;
		if(g != null)
		{ // leave only the variables in the true statement of the if statements
			StatementCollection s = g.FalseStatements;
			CxList inIf = unknownRef.GetByAncs(All.FindById(s.NodeId));
			result.Add(inIf.FindAllReferences(relevantVar));
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}