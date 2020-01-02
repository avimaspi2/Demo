if (param.Length == 1)
{
	CxList activeRecord = param[0] as CxList;

	CxList methods = Find_Methods() + All.FindByType(typeof(MemberAccess));

	CxList activeRecordsMethods = methods.GetByAncs(activeRecord);
	activeRecordsMethods -= activeRecordsMethods.GetTargetOfMembers().GetMembersOfTarget();
	CxList potentials = activeRecordsMethods;

	CxList activeRecordsRef = All.FindAllReferences(activeRecord);
	activeRecordsRef = activeRecordsRef.GetMembersOfTarget();

	CxList allAndNew = 
		//activeRecordsRef.FindByShortName("all") + 
		activeRecordsRef.FindByShortName("new");
	activeRecordsRef -= allAndNew;
	
	CxList assign = allAndNew.GetAncOfType(typeof(AssignExpr));
	allAndNew = All * All.GetByAncs(assign).FindByAssignmentSide(CxList.AssignmentSide.Left).DataInfluencedBy(allAndNew);
	//result.Add(allAndNew); // this is only DB In, not DB out

	activeRecordsRef.Add(All.FindAllReferences(allAndNew));
	potentials.Add(activeRecordsRef);
	
	CxList staticConnection = All.FindByMemberAccess("ActiveRecord.Base").GetMembersOfTarget().FindByShortName("connection");
	assign = staticConnection.GetAncOfType(typeof(AssignExpr));
	staticConnection = All.FindAllReferences(All.GetByAncs(assign).FindByAssignmentSide(CxList.AssignmentSide.Left)).DataInfluencedBy(staticConnection);
	potentials.Add(staticConnection.GetMembersOfTarget());

	potentials = All * potentials;
	
	result.Add(All.FindByMemberAccess("connection.execute"));
	
	string[] dbCommands = new string[] {
		"find*",
		"first",
		"last",
		"all",
		"execute",
		"update",
		"scoped_by_*"	
		};

	foreach (string s in dbCommands)
	{
		result.Add(potentials.FindByShortName(s));
	}
	
	result.Add(Add_Second_Order_DB(result, dbCommands));

	result -= result.FindAllReferences(All.FindDefinition(result * methods));
}
/*
CxList newOne = All.DataInfluencedBy(All.GetByAncs(All.FindByType(typeof(ObjectCreateExpr))));
CxList newAssing = newOne.GetAncOfType(typeof(AssignExpr));
newOne = All.FindAllReferences(potentials).GetByAncs(newAssing).FindByAssignmentSide(CxList.AssignmentSide.Left);
newAssing = newOne.GetAncOfType(typeof(AssignExpr));
//result.Add(All.GetByAncs(newAssing).FindByAssignmentSide(CxList.AssignmentSide.Right));
*/