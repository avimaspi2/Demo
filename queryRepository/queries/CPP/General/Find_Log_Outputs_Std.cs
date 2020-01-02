CxList methods = Find_Methods();

result = All.GetParameters(methods.FindByShortName("syslog"), 2);

CxList clog = All.FindByShortName("clog");
CxList exp = clog.GetAncOfType(typeof (BinaryExpr));
result.Add(All.FindByFathers(exp) - clog);