CxList cond = Get_Conditions();
CxList methods = Find_Methods();
CxList ip = 
	methods.FindByShortName("gethostbyaddr") + 
	methods.FindByShortName("getnameinfo") + 
	methods.FindByShortName("getaddrinfo");

result = cond.DataInfluencedBy(ip);