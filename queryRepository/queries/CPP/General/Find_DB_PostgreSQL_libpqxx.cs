CxList methods = Find_Methods();
//Look for members of transaction classes (work)
CxList workMembers = methods.FindByMemberAccess("work.*");
List<string> executionMethods = new List<string> {"exec", "exec0", "exec1", "exec_n", "exec_params", "exec_params0"
		, "exec_params1", "exec_params_n", "exec_prepared", "exec_prepared0", "exec_prepared1", "exec_prepared_n"};

result = workMembers.FindByShortNames(executionMethods);