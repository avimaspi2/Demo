CxList emptyString = Find_Empty_Strings();
CxList NULL = All.FindByName("null", false);
CxList psw = Find_Passwords();

// Find password in an initialization operation
CxList psw_in_lSide = psw.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList psw_in_lSide_decl = psw_in_lSide.FindByType(typeof(Declarator));

CxList strLiterals = Find_Strings() - emptyString - NULL;

//when the hardcoded string includes a space or dot we believe 
//it is not a password string
CxList strToREmove = strLiterals.FindByName("* *");
strToREmove.Add(strLiterals.FindByName("*.*"));
strToREmove.Add(strLiterals.FindByName("*/*"));
strToREmove.Add(strLiterals.FindByName("*\\*"));
strLiterals -= strToREmove;

//strings as assignment in initialization   ==> Dim pass As String =  "goodbye"	
CxList lit_in_rSide = strLiterals.FindByAssignmentSide(CxList.AssignmentSide.Right);
//add string parametes of StringBuilder to initialization ==> Dim password As New StringBuilder("abcd")
CxList StringBuilders = All.FindByType(typeof (ObjectCreateExpr)).FindByShortName("StringBuilder", false);
CxList allParams = All.FindByType(typeof (Param));
CxList allStringsParams = strLiterals.GetFathers() * allParams.GetParameters(StringBuilders);
lit_in_rSide.Add(allStringsParams);

CxList initializePassword = psw_in_lSide_decl.FindByInitialization(lit_in_rSide);

// Find password in an "equals" operation
CxList eq = All.FindByMemberAccess("String.Equals", false);
CxList eqWithTwoParams = eq;
eq *= psw.GetMembersOfTarget();
CxList equalsPassword = strLiterals.GetByAncs(eq);

eq = eqWithTwoParams;
eq *= strLiterals.GetMembersOfTarget();
equalsPassword.Add(psw.GetByAncs(eq));

//equals of type "string".equals(password);
CxList strEQ = strLiterals.GetMembersOfTarget().FindByShortName("Equals", false);
strEQ = psw.GetByAncs(strEQ);
equalsPassword.Add(strEQ);

//equals of type [String].Equals("hello", password)
CxList equalsParam1 = All.GetParameters(eqWithTwoParams, 0);
CxList equalsParam2 = All.GetParameters(eqWithTwoParams, 1);
CxList hPassInEq2Params = All.NewCxList();
if (equalsParam2.Count > 0)
{
	hPassInEq2Params = All.FindByParameters(equalsParam1 * psw).FindByParameters(equalsParam2 * strLiterals);
	hPassInEq2Params.Add(All.FindByParameters(equalsParam2 * psw).FindByParameters(equalsParam1 * strLiterals));
	hPassInEq2Params = psw.GetParameters(hPassInEq2Params);
}

// Find a password in a simple assignment
CxList assignPassword = psw_in_lSide.GetAncOfType(typeof(AssignExpr));
assignPassword = lit_in_rSide.GetByAncs(assignPassword);

result = initializePassword;
result.Add(equalsPassword); 
result.Add(assignPassword);
result.Add(hPassInEq2Params);