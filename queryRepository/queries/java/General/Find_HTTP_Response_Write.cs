CxList responses = Find_HTTP_Responses();
CxList responseMembers = responses.GetMembersOfTarget();
CxList write = Find_Write();
CxList outputStreams = responseMembers.FindByShortNames(new List<string>()
	{"getWriter","getOutputStream"}).GetAssignee();
result.Add(responses);
result.Add(All.FindAllReferences(outputStreams));

result = result.GetRightmostMember()
	.FindByShortNames(new List<string>(){"print","printf","println","write"});
result.Add(responseMembers * write);