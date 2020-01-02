// NOTICE THAT THE RESULT OF THIS QUERY IS ONLY A PARTIAL VIEW OF THE NEEDED RESULT.
// WE SHOULD FIND AN EXECUTE OF A FILE THAT THE USER WAS WRITING TO, BUT WE SHOULD ALSO SHOW
// THAT THE USER WAS WRITING, SO WE ONLY SHOW ONE OF THEM, AS CONFUSING AS IT MIGHT BE.

// $ASP
CxList openFile = Find_Member_With_Target("Scripting.FileSystemObject", "OpenTextFile");
CxList opens = All.NewCxList();
CxList textFileName = Find_Strings().GetByAncs(All.GetParameters(openFile, 0));
CxList executeFile = Find_Strings().GetParameters(Find_Execute());
foreach (CxList exec in executeFile)
{
	CSharpGraph g1 = exec.data.GetByIndex(0) as CSharpGraph;
	string name1 = g1.FullName;
	foreach (CxList text in textFileName)
	{
		CSharpGraph g2 = text.data.GetByIndex(0) as CSharpGraph;
		string name2 = g2.FullName;
		if (name1.Equals(name2))
		{
			opens.Add(text);
		}
	}
}

CxList l1 = Find_Write().DataInfluencedBy(Find_Interactive_Inputs()).GetTargetOfMembers();
CxList l2 = All.FindAllReferences(
	All.GetByAncs(opens.GetAncOfType(typeof(AssignExpr))).
	FindByAssignmentSide(CxList.AssignmentSide.Left)
	); // Get left side of text file

result = (l1 * l2).GetMembersOfTarget();