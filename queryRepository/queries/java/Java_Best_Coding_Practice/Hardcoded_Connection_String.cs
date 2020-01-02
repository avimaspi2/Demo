CxList conStr = Find_Strings().FindByName("*jdbc:*", false);

CxList openConnection = Find_Methods().FindByShortName("*getConnection");
CxList openConParam = All.GetParameters(openConnection);

result = 
	conStr.DataInfluencingOn(openConParam) + 
	conStr * openConParam;