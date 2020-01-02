CxList PersistSecurityInfo = All.FindByRegex(@"Persist Security Info(\s)*=(\s)*(True|Yes)");

CxList openConnection = All.FindByType(typeof(ObjectCreateExpr)).FindByShortName("*Connection");
CxList openConParam = All.GetParameters(openConnection);

result = openConParam.DataInfluencedBy(PersistSecurityInfo.FindByType(typeof(StringLiteral)));