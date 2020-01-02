CxList PersistSecurityInfo = All.FindByRegex(@"persist security info(\s)*=(\s)*(true|yes)");

CxList openConnection = All.FindByType(typeof(ObjectCreateExpr)).FindByShortName("*connection");
CxList openConParam = All.GetParameters(openConnection);

result = openConParam.DataInfluencedBy(PersistSecurityInfo.FindByType(typeof(StringLiteral)));