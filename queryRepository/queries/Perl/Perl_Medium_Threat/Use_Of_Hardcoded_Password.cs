CxList emptyString = Find_Empty_Strings();
CxList NULL = All.FindByName("null");
CxList psw = Find_Passwords();

// dbi password
CxList dbi_conn = Find_Methods().FindByMemberAccess("DBI", "connect") + Find_Methods().FindByMemberAccess("DBI", "connect_cached");
psw.Add(All.GetParameters(dbi_conn, 3));

// oracle password
CxList oracle_conn = Find_Methods().FindByShortName("ora_login");
psw.Add(All.GetParameters(oracle_conn, 3));

// mysql password
CxList mysql_conn = Find_Methods().FindByMemberAccess("Mysql", "connect");
psw.Add(All.GetParameters(mysql_conn, 2));
	
// Lists preperation
CxList psw_in_lSide = psw.FindByAssignmentSide(CxList.AssignmentSide.Left);

CxList strLiterals = Find_Strings() - emptyString - NULL;
// 	(when the hardcoded string includes a space or dot we believe it is not a password string)
strLiterals -= strLiterals.FindByName("* *");
strLiterals -= strLiterals.FindByName("*.*");
strLiterals -= strLiterals.FindByName("*/*");
strLiterals -= strLiterals.FindByName("*\\*");
CxList lit_in_rSide = strLiterals.FindByAssignmentSide(CxList.AssignmentSide.Right);
CxList passNoString = psw - strLiterals;

// Find password in an initialization operation
CxList eq = All.FindByShortName("==");
eq.Add(All.FindByShortName("eq"));
eq.Add(All.FindByShortName("ne"));
eq.Add(All.FindByShortName("!="));

CxList equalsPassword = passNoString.GetByAncs(eq);
eq = equalsPassword.GetFathers() * eq;
equalsPassword = strLiterals.GetByAncs(eq);

// Find password in as assignment
CxList assignPassword = psw_in_lSide.GetAncOfType(typeof(AssignExpr));
assignPassword = lit_in_rSide.GetByAncs(assignPassword);

//// Add hardcoded password in post login
//CxList methods = Find_Methods();
//CxList strings = Find_Strings();
//CxList loginStrings = strings.FindByShortName("*/login*");
//CxList httpStrings = strings.FindByShortName("*http:*");
//CxList post = methods.FindByShortName("post");
//CxList postParam = All.GetParameters(post, 0);
//CxList postLoginParam = postParam.DataInfluencedBy(loginStrings) + postParam * loginStrings;
//post = post.FindByParameters(postLoginParam);
//CxList postPassword = post.FindByRegex(@"password\s*=>\s*'\w");

assignPassword.Add(All.FindByRegex(@"password\s*=>\s*'\w"));

result = assignPassword + equalsPassword;