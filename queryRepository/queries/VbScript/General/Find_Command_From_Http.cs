if (param.Length > 0)
{
	CxList httpRequest = param[0] as CxList;

	CxList requests = httpRequest.GetFathers();
	CxList assignRequests = requests.FindByType(typeof(AssignExpr));
	requests -= assignRequests;
	requests.Add(All.GetByAncs(assignRequests).FindByAssignmentSide(CxList.AssignmentSide.Left));
	result = All.FindAllReferences(requests).GetMembersOfTarget().FindByShortName("open");
}