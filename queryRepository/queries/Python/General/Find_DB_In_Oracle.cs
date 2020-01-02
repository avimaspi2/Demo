CxList allDataInfluencedByConn = All.FindAllReferences(Find_DB_Conn_Oracle());
CxList methods = allDataInfluencedByConn.GetMembersOfTarget();

//Explicit functions
List<string> methodsNames = new List<string> {
		"execute*","callfunc","callproc"};

result.Add(methods.FindByShortNames(methodsNames));