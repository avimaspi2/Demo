CxList session = All.FindByShortName("*Session_User*", false);
session.Add(All.FindByShortName("*Session_cust*", false));
session.Add(All.FindByShortName("*Session_id*", false));

if(session.Count != 0)
{	
	CxList emptyString = Find_Empty_Strings();
	CxList zero = All.FindByName("0");

	CxList clear = All.FindByName("*Session.Clear", false);
	clear.Add(All.FindByName("*Session.Abandon", false));
	clear.Add(All.FindByMemberAccess("*FormsAuthentication.SignOut", false));

	CxList a = All.FindByAssignmentSide(CxList.AssignmentSide.Left) * session;
	CxList b = All.FindByAssignmentSide(CxList.AssignmentSide.Right) * (emptyString + zero);

	CxList c = a.GetFathers() * b.GetFathers();

	if((c + clear).Count == 0)
	{
		result = session;
	}
}