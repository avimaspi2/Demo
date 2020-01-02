CxList conStr = Find_Strings().FindByName("*jdbc:*", false);
CxList methods = Find_Methods();
CxList openConnection = methods.FindByShortName("*getConnection") +
						methods.FindByName("*Sql.newInstance");
CxList openConParam = All.GetParameters(openConnection);

result = 
	conStr.DataInfluencingOn(openConParam) + 
	conStr * openConParam;