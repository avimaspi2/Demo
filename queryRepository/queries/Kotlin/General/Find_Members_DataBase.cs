result = All.NewCxList();
if (param.Length == 1){
	string dbName = param[0] as string;
	
	CxList methods = Find_Methods();
	CxList allInheritsFrom = All.InheritsFrom(dbName);
	foreach(CxList aux in allInheritsFrom){
		result.Add(methods.FindByMemberAccess(aux.GetName() + ".*"));
	}
	result.Add(methods.FindByMemberAccess(dbName + ".*"));

}