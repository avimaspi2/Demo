CxList methods = All.FindByType(typeof(MethodInvokeExpr));
CxList methodsAllowOrigin = methods.FindByShortName("AccessControlAllowOrigin");
CxList classes = All.FindByTypes(new String[]{"config","CorsSettings","CORSConfig","CorsDirectivesSpec","HttpResponse","HttpMessage"});
methodsAllowOrigin.Add(classes.GetMembersOfTarget().FindByShortNames(new List<String>{"headers", "origins","allowedOrigins","AccessControlAllowOrigin"}));

CxList methodsVuln = All.NewCxList();

if (methodsAllowOrigin.Count > 0){
	CxList strings = All.FindByType(typeof(StringLiteral));
	CxList wildcard = strings.FindByRegex("\"\\*\"");
	methodsVuln = methodsAllowOrigin.InfluencedBy(wildcard);
	CxList influencers = Find_Inputs();
	influencers.Add(Find_DB_Out());
	influencers.Add(Find_Read());
	influencers.Add(Find_Remote_Requests());
	methodsVuln.Add(methodsAllowOrigin.InfluencedBy(influencers));
}

result = methodsVuln;