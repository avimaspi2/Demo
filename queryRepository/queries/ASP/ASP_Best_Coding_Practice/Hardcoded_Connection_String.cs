CxList conStr = Find_Strings().FindByName("*provider*", false);

CxList openConnection = All.FindByType(typeof(ObjectCreateExpr)).FindByShortName("*connection");
CxList openConParam = All.GetParameters(openConnection);

result.Add(conStr.DataInfluencingOn(openConParam));