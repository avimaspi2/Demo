CxList responseWriter = Find_HTTP_Responses();
CxList writerHeader = responseWriter.GetMembersOfTarget().FindByShortName("Header");
CxList changeHeader = writerHeader.GetMembersOfTarget().FindByShortNames(new List<string>(){"Add", "Set"});

result = changeHeader;