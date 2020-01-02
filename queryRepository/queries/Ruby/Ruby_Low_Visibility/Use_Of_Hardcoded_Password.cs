CxList emptyString = Find_Empty_Strings();
CxList NULL = All.FindByName("null");
CxList psw = Find_Passwords();

// Lists preperation
CxList psw_in_lSide = psw.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList strLiterals = Find_Strings() - emptyString - NULL;
// 	(when the hardcoded string includes a space or dot we believe it is not a password string)
strLiterals -= strLiterals.FindByName("* *");
strLiterals -= strLiterals.FindByName("*.*");
strLiterals -= strLiterals.FindByName("*/*");
strLiterals -= strLiterals.FindByName("*\\*");
CxList lit_in_rSide = strLiterals.FindByAssignmentSide(CxList.AssignmentSide.Right);

// Find password in an initialization operation
CxList eq = All.FindByShortName("==");
eq.Add(All.FindByShortName("==="));
eq.Add(All.FindByShortName("!="));
eq.Add(All.FindByShortName("!=="));
eq.Add(All.FindByShortName("|"));
eq.Add(All.FindByShortName("&"));
eq.Add(All.FindByShortName("^"));
eq = eq.GetAncOfType(typeof(BinaryExpr));

CxList strPass = psw.FindByType(typeof (StringLiteral));
CxList paramPass = psw.FindByType(typeof (Param));

// Find all comparisons of type ==> "hello" == password
eq = strLiterals.GetFathers() * eq;
CxList equalsPassword = (psw - strPass).FindByFathers(eq);

// Find password in as assignment
CxList assignPassword = psw_in_lSide.GetAncOfType(typeof(AssignExpr));
assignPassword = lit_in_rSide.GetByAncs(assignPassword);

CxList assignPasswords = psw - strPass - paramPass.FindByShortName(strPass);
assignPassword.Add(strLiterals.GetFathers() * assignPasswords);	//assignment of type ==> password = "This_string"
///assignment of type ==> 	pass = String.new("This is my string")
assignPassword.Add(strLiterals.GetAncOfType(typeof (ObjectCreateExpr)).GetFathers() * assignPasswords);

// Find password inside define methods
CxList defineMethods = Find_Methods().FindByShortName("define");
CxList pswParameter = All.GetParameters(defineMethods, 0).FindByType(typeof(StringLiteral));
CxList pswLiteralParameter = All.GetParameters(defineMethods, 1).FindByType(typeof(StringLiteral));

pswParameter = pswParameter * psw;
pswLiteralParameter = pswLiteralParameter * strLiterals;
CxList definePasswords = defineMethods.FindByParameters(pswParameter).FindByParameters(pswLiteralParameter);

//Find passwords in "equal?", "eql?", "casecmp"
CxList allMethods = All.FindByType(typeof (MethodInvokeExpr));
CxList equalMethods = allMethods.FindByShortName("eql?") + allMethods.FindByShortName("equal?") 
	+ allMethods.FindByShortName("casecmp");
//find equal methods ==> "hello".eql? pwd  ==> "hello".equal? pwd
CxList strEqualMeth = strLiterals.GetFathers();
strEqualMeth = allMethods.GetByAncs(strEqualMeth);
CxList inEql = psw.GetParameters(strEqualMeth).FindByType(typeof (Param));
//find equal methods ==> pwd.eql? "hello"  ==> pwd.equal? "hello"
strEqualMeth = equalMethods.FindByParameters(strLiterals);
CxList equalOfPsd = allMethods.GetByAncs(psw.GetFathers());
strEqualMeth *= equalOfPsd;
inEql.Add(strLiterals.GetParameters(strEqualMeth));

result = definePasswords + equalsPassword + assignPassword + inEql;