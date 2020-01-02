CxList emptyString = Find_Empty_Strings();
CxList NULL = All.FindByName("null");
CxList psw = Find_Passwords();

// Lists preperation
CxList psw_in_lSide = psw.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList strLiterals = Find_Strings() - emptyString - NULL;
CxList allStrings = strLiterals;
// 	(when the hardcoded string includes a space or dot we believe it is not a password string)
strLiterals -= strLiterals.FindByName("* *");
strLiterals -= strLiterals.FindByName("*.*");
strLiterals -= strLiterals.FindByName("*/*");
strLiterals -= strLiterals.FindByName("*\\*");
CxList lit_in_rSide = strLiterals.FindByAssignmentSide(CxList.AssignmentSide.Right);

CxList fathers = lit_in_rSide.GetFathers() * psw_in_lSide.GetFathers();
lit_in_rSide = lit_in_rSide.FindByFathers(fathers);
//Add hardcoded passwords from assignments
CxList assignPassword = lit_in_rSide;

//remove passwords with equal name and contant ==> currPassword = "currPassword";
CxList notHdPass = All.NewCxList();
char[] trimChars = new char[2] { '\'', '"'};

CxList currPassInRight = null;
foreach(CxList currLit in assignPassword)
{
	currPassInRight = psw_in_lSide.FindByFathers(currLit.GetFathers());
	string strName = currLit.GetName().Trim(trimChars);
	string passName = currPassInRight.GetName();
	if (passName.Equals(strName))
	{
		notHdPass.Add(currLit);
	}
}
assignPassword -= notHdPass;

// Find password in an initialization operation

CxList eq = All.FindByShortNames(new List<string> { "==","===","!=","!==","|","&","^" });

eq = eq.GetAncOfType(typeof(BinaryExpr));
CxList pswNotStringLit = psw - allStrings;
CxList equalsPassword = pswNotStringLit.GetFathers() * eq;
equalsPassword = strLiterals.FindByFathers(equalsPassword);
 
// Find password inside define methods
CxList defineMethods = Find_Methods().FindByShortName("define");
CxList pswParameter = All.GetParameters(defineMethods, 0).FindByType(typeof(StringLiteral));
CxList pswLiteralParameter = All.GetParameters(defineMethods, 1).FindByType(typeof(StringLiteral));

pswParameter = pswParameter * psw;
pswLiteralParameter = pswLiteralParameter * strLiterals;
CxList definePasswords = defineMethods.FindByParameters(pswParameter).FindByParameters(pswLiteralParameter);

//Find password in StrComp method	==> strcmp("hello",pwd,0)
CxList strcmp = Find_Methods().FindByShortName("strcmp");
CxList strcmpParam1 = All.GetParameters(strcmp, 0);
CxList strcmpParam2 = All.GetParameters(strcmp, 1);
CxList hPassInstrcmp = All.FindByParameters(strcmpParam1 * psw).FindByParameters(strcmpParam2 * strLiterals);
hPassInstrcmp.Add(All.FindByParameters(strcmpParam2 * psw).FindByParameters(strcmpParam1 * strLiterals));
hPassInstrcmp = psw.GetParameters(hPassInstrcmp);
hPassInstrcmp = hPassInstrcmp.FindByType(typeof (UnknownReference));

// Find hardcoded password as the first parameter of the encodePassword method (Symfony)
CxList encodePasswordMemberAccess = All.FindByMemberAccess("*.encodePassword", false);
CxList encodePasswordLiteral = Find_Strings().GetParameters(encodePasswordMemberAccess, 0);

result = equalsPassword;
result.Add(assignPassword);
result.Add(hPassInstrcmp);
result.Add(definePasswords);
result.Add(encodePasswordLiteral);

//addPHPArrays
CxList arr = All.FindByShortName("array") * Find_Methods();

CxList inArray = arr.FindByParameters(psw);
CxList paramOfPass = psw.GetFathers().FindByType(typeof(Param));
foreach(CxList pop in paramOfPass)
{
	int index = pop.GetIndexOfParameter();
	if(index % 2 == 0)
	{
		CxList array = inArray.FindByParameters(pop);
		result.Add(strLiterals.GetParameters(array, ++index));
	}
}