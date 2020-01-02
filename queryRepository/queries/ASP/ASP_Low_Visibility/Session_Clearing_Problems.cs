CxList session = All.FindByShortName("*Session_user*", false);
session.Add(All.FindByShortName("*session_cust*", false));
session.Add(All.FindByShortName("*Session_id*", false));
session.Add(All.FindByShortName("*SessionID", false));

session -= Find_Strings();

if(session.Count > 0)
{
	CxList emptyString = Find_Empty_Strings();
	CxList zero = All.FindByName("0");

	CxList clear = All.FindByName("*Session.clear");
	clear.Add(All.FindByName("*Session.abandon"));
	clear.Add(All.FindByMemberAccess("*formsauthentication.signout"));

	CxList a = All.FindByAssignmentSide(CxList.AssignmentSide.Left) * session;
	CxList b = All.FindByAssignmentSide(CxList.AssignmentSide.Right) * (emptyString + zero);

	CxList c = a.GetFathers() * b.GetFathers();

	if((c + clear).Count == 0)
	{
		//Add only the left side of assignments
		result = a;
	}
}