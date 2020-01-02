CxList session = All.FindByShortNames(new List<string> {"*Session_User*",
		"*Session_Cust*",
		"*Session_Id*"}, false);

//The code below takes just one line from 3 possible for each Session Open
session = session.FindByType(typeof(IndexerRef));
if(session.Count > 0)
{
	CxList emptyString = Find_Empty_Strings();
	CxList zero = All.FindByName("0");
	CxList clear = All.FindByName("*Session.Clear") + 
		All.FindByName("*Session.Abandon") + 	
		All.FindByMemberAccess("*FormsAuthentication.SignOut");
	CxList a = All.FindByAssignmentSide(CxList.AssignmentSide.Left) * session;
	CxList b = All.FindByAssignmentSide(CxList.AssignmentSide.Right) * (emptyString + zero);

	CxList c = a.GetFathers() * b.GetFathers();
	if((c + clear).Count == 0 && session.data.Count > 0)
	{
		// From all places where session is opened, take just first one to present
		// Build first as DOM element
		CSharpGraph graph = session.data.GetByIndex(0) as CSharpGraph;
		result = All.FindById(graph.NodeId);
	}
}