CxList allDataInfluencedByConn = All.FindAllReferences(Find_DB_Conn_ADOdb());
CxList methods = allDataInfluencedByConn.GetMembersOfTarget();

//Explicit functions
List<string> methodsNames = new List<string> {
		"Get*","FetchRow","GetRowAssoc","FetchField","fields"};
result.Add(methods.FindByShortNames(methodsNames));

//Implicit fecth
result.Add(allDataInfluencedByConn.GetFathers().FindByType(typeof(ForEachStmt)));
result.Add(allDataInfluencedByConn.GetFathers().GetFathers().FindByType(typeof(IterationStmt)));