CxList methods = Find_Methods();
CxList redirect = methods.FindByShortName("header", false);
CxList stringLiterals = Find_Strings();
CxList redirectLocationStrings = stringLiterals.FindByShortName("*location*", false) -
	stringLiterals.FindByShortName("*-location*", false);

redirect = redirect.InfluencedBy(redirectLocationStrings);

CxList rand = methods.FindByShortNames(new List<String>(){ "*_rand", "rand" }, false) +
	Get_ESAPI().FindByShortName("getRandom*"); // ESAPI

redirect -= redirect.DataInfluencedBy(rand);

//CxList php = All.FindByFileName("*.php");
CxList ifStmt = All.FindByType(typeof(IfStmt));
CxList caseStmt = All.FindByType(typeof(Case));

result = redirect.GetByAncs(ifStmt).GetAncOfType(typeof(IfStmt)) + 
	redirect.GetByAncs(caseStmt).GetAncOfType(typeof(Case));