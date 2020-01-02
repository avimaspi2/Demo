CxList session = All.FindByType("HtppSessionState") + All.FindByType("HttpSession") - All.FindByType(typeof(TypeRef)) -
	All.FindByType(typeof(Declarator)) - All.FindByType(typeof(ParamDecl));
session.Add(All.FindByType(typeof(IndexerRef)).FindByShortName("Session_*"));
CxList sessionAssigned = session.FindByAssignmentSide(CxList.AssignmentSide.Left);
sessionAssigned.Add(All.FindByType(typeof(MemberAccess)).FindByShortName("Session"));
session.Add(All.FindByType(typeof(UnknownReference)).FindByShortName("Session"));
session = session.FindByType(typeof(UnknownReference));
sessionAssigned.Add(session.GetMembersOfTarget().FindByShortName("Add"));

result = sessionAssigned;