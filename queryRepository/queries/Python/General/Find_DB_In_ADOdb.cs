CxList allDataInfluencedByConn = Find_DB_Conn_ADOdb();

CxList methods = All.FindAllReferences((allDataInfluencedByConn)).GetMembersOfTarget();

//Explicit functions
List<string> methodsNames = new List<string> {
		"Execute","SelecLimit","UpdateBlob*"};

result.Add(methods.FindByShortNames(methodsNames));