if (param.Length > 1)
{	
	CxList httpRequest = param[0] as CxList;
	string command = param[1] as string;
	
	if (command != null && httpRequest != null)
	{
		CxList requests = httpRequest.GetAssignee();
		result = All.FindAllReferences(requests).GetMembersOfTarget().FindByShortName(command);
	}
}