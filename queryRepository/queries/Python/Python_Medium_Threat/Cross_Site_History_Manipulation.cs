CxList redirects = Find_Redirects();
CxList rand = Find_Methods_By_Import("random", 
	new string[]{"random", "uniform", "randint", "randrange", "triangular"});
//	.betavariate(alpha, beta), .expovariate(lambd), .gammavariate(alpha, beta), .gauss(mu, sigma), .lognormvariate(mu, sigma)
//	.normalvariate(mu, sigma), .vonmisesvariate(mu, kappa), .paretovariate(alpha), .weibullvariate(alpha, beta)
// 	generate random numbers according to a defined distribution and so are even weaker than other generators.

redirects -= redirects.DataInfluencedBy(rand);

CxList ifStmt = Find_Ifs();
CxList caseStmt = Find_Cases();

result = redirects.GetByAncs(ifStmt).GetAncOfType(typeof(IfStmt));
result.Add(redirects.GetByAncs(caseStmt).GetAncOfType(typeof(Case)));