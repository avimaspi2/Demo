CxList smartyMethods = Find_Methods().FindByFileName("*.tpl");

CxList smartyOutputs = smartyMethods.FindByShortNames(new List<string>
	{"escape", "html_options", "date_format"});

CxList smartyButtonsMethods = smartyMethods.FindByShortNames(new List<string>
	{"html_checkboxes", "html_radios"});

//all smarty parameters
CxList allSmartyParams = All.GetParameters(smartyMethods);
allSmartyParams -=  allSmartyParams.FindByType(typeof(Param));

//the 2nd and 5th parameters are escaped by default
CxList sanitizedParams = allSmartyParams.GetParameters(smartyButtonsMethods, 1);
sanitizedParams.Add(allSmartyParams.GetParameters(smartyButtonsMethods, 4));

//the 3rd parameter - the output array is escaped by default, not escaped if the last parameter is false
CxList outputParam = allSmartyParams.GetParameters(smartyButtonsMethods, 2);

foreach (CxList singleParam in outputParam)
{
	CxList escapeFlag = allSmartyParams.GetParameters(smartyButtonsMethods.FindByParameters(singleParam), 9).FindByType(typeof(BooleanLiteral));
	if (escapeFlag.FindByName("false").Count == 0)
	{
		sanitizedParams.Add(singleParam);
	}
}


CxList setFilterMethods = smartyMethods.FindByShortName("setfilter");

CxList ifStmt = All.FindByType(typeof(IfStmt));
CxList exp = All.FindByType(typeof(Expression));
CxList setFilterMethodAsCond = setFilterMethods.GetByAncs(exp.FindByFathers(ifStmt));
CxList relevantIfStmt = setFilterMethodAsCond.GetAncOfType(typeof(IfStmt));
CxList sanitizeIfStatement = All.GetByAncs(relevantIfStmt);

result.Add(smartyOutputs);
result.Add(sanitizedParams);
result.Add(sanitizeIfStatement);