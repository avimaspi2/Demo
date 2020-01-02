CxList responses = Find_HTTP_Responses();

CxList writeResponse = responses.GetMembersOfTarget()
	.FindByShortNames(new List<string>(){"Write"});

writeResponse.Add( All.FindByParameters(responses)
	.FindByShortNames(new List<string>(){"Fprintf","ServeFile","Execute","ExecuteTemplate"}));
result = writeResponse;