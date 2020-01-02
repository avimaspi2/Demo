CxList allMethods = Find_Methods();
CxList unkRefs = Find_UnknownReferences();

CxList mysqlParamSanitizer = allMethods.FindByShortNames(
	new List<string> { "bind_param","mysqli_stmt_bind_param","mysqli_bind_param", "mysqli_real_escape_string"}
	);

CxList mysqliTarget = unkRefs.FindByType("mysqli");
CxList mysqliMembersSanitizers = mysqliTarget.GetMembersOfTarget().FindByShortNames(
	new List<string> {"escape_string", "real_escape_string"}
	);

result.Add(mysqlParamSanitizer);
result.Add(mysqliMembersSanitizers);