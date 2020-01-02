CxList methods = Find_Methods();
CxList redirect = methods.FindByShortName("header", false);
CxList stringLiterals = All.FindByType(typeof(StringLiteral));
CxList redirectLocationStrings = stringLiterals.FindByShortName("*location*", false) -
	stringLiterals.FindByShortName("*-location*", false);

redirect = redirect.InfluencedBy(redirectLocationStrings);

CxList rand = methods.FindByShortName("*_rand", false);
rand.Add(methods.FindByShortName("rand", false));
rand.Add(methods.FindByShortName("randnum", false));
rand.Add(methods.FindByShortName("randbyte", false));
rand.Add(methods.FindByShortName("random_number", false));
rand.Add(methods.FindByShortName("random_bytes", false));
redirect -= redirect.DataInfluencedBy(rand);

CxList ifStmt = All.FindByType(typeof(IfStmt));
CxList caseStmt = All.FindByType(typeof(Case));

result = redirect.GetByAncs(ifStmt).GetAncOfType(typeof(IfStmt)) + 
	redirect.GetByAncs(caseStmt).GetAncOfType(typeof(Case));