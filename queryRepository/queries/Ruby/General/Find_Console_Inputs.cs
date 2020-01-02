CxList variables = All.FindByType(typeof(UnknownReference));
CxList methods = Find_Methods();
result = (variables + methods).FindByShortName("gets") +
	variables.FindByShortName("$_");