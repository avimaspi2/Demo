result = All.FindByMemberAccess("AccessReferenceMap.getDirectReference");

//Find data structure get methods
CxList getMethod = All.FindByMemberAccess(".get");
CxList dataStractureGet = getMethod.FindByMemberAccess("Attributes.get");
dataStractureGet.Add(getMethod.FindByMemberAccess("Collection.get"));
dataStractureGet.Add(getMethod.FindByMemberAccess("List.get"));
dataStractureGet.Add(getMethod.FindByMemberAccess("Map.get"));
dataStractureGet.Add(getMethod.FindByMemberAccess("Table.get"));
dataStractureGet.Add(getMethod.FindByMemberAccess("Vector.get"));
result.Add(dataStractureGet);

// Find conditions variables of if statements
CxList ifStmt = base.Find_Ifs();
CxList conditions = All.NewCxList();
foreach (CxList singleIf in ifStmt)
{
	try
	{
		IfStmt stmt = singleIf.TryGetCSharpGraph<IfStmt>();
		if (stmt.Condition != null)
		{
			conditions.Add(stmt.Condition.NodeId, stmt.Condition);
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

CxList conditionsVars = All.GetByAncs(conditions);
result.Add(All.FindAllReferences(conditionsVars));