CxList resps = Find_HTTP_Responses();
result = resps.GetRightmostMember().FindByShortNames(new List<string>(){
		"end",
		"send",
		"render",
		"sendfile",
		"sendFile",
		"json",
		"jsonp",
		"sendStatus"});