CxList conStr = Find_Strings().FindByName("*Provider*", false);

CxList openConnection = All.FindByType(typeof(ObjectCreateExpr)).FindByShortName("*Connection");
CxList openConParam = All.GetParameters(openConnection);

result = conStr.DataInfluencingOn(openConParam);