CxList redirect = Find_Cross_Site_History_Manipulation_Redirects();

CxList rand = Find_Cross_Site_History_Manipulation_Random();

redirect -= redirect.DataInfluencedBy(rand);

CxList ifStmt = Find_Ifs();
CxList caseStmt = Find_Cases();

result = redirect.GetByAncs(ifStmt).GetAncOfType(typeof(IfStmt)); 
result.Add(redirect.GetByAncs(caseStmt).GetAncOfType(typeof(Case)));