CxList smartyMethods = Find_Methods().FindByFileName("*.tpl");

CxList smartyRead = 
	smartyMethods.FindByShortName("fetch");

result.Add(smartyRead);