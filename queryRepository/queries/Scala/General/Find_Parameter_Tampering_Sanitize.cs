result = All.FindByMemberAccess("AccessReferenceMap.getDirectReference");

//Find data structure get methods
CxList getMethod = All.FindByMemberAccess(".get");
CxList dataStructureGet = getMethod.FindByMemberAccess("Attributes.get");
dataStructureGet.Add(getMethod.FindByMemberAccess("Collection.get"));
dataStructureGet.Add(getMethod.FindByMemberAccess("List.get"));
dataStructureGet.Add(getMethod.FindByMemberAccess("Map.get"));
dataStructureGet.Add(Find_Map_Collection_Get() * getMethod);
dataStructureGet.Add(getMethod.FindByMemberAccess("Table.get"));
dataStructureGet.Add(getMethod.FindByMemberAccess("Vector.get"));
dataStructureGet.Add(Find_Map_Collection_Get());
result.Add(dataStructureGet);


// Find conditions variables of if statements
CxList ifStmt = All.FindByType(typeof(IfStmt));
CxList conditions = All.NewCxList();
foreach (CxList singleIf in ifStmt)
{
	try
	{
		IfStmt stmt = singleIf.data.GetByIndex(0) as IfStmt;
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