CxList allDataInfluencedByConn = All.FindAllReferences(Find_DB_Conn_SAP());
CxList methods = allDataInfluencedByConn.GetMembersOfTarget();

List<string> methodsNames = new List<string> {
		//Explicit functions
		"getResultSet*","fetch*","select*",
		//Cursor functions
		"next","previous","relative","absolute","first","last","current"
		};

result.Add(methods.FindByShortNames(methodsNames));

//Implicit fecth
result.Add(allDataInfluencedByConn.GetFathers().FindByType(typeof(ForEachStmt)));