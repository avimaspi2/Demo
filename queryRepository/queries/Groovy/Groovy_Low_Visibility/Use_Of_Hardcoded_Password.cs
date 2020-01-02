CxList emptyString = Find_Empty_Strings();
CxList NULL = All.FindByName("null");
CxList psw = Find_Passwords();

// Find password in an initialization operation
CxList psw_in_lSide = psw.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList psw_in_lSide_decl = psw_in_lSide.FindByType(typeof(Declarator));

CxList strLiterals = Find_Strings() - emptyString - NULL;

//when the hardcoded string includes a space or dot we believe 
//it is not a password string
strLiterals -= strLiterals.FindByName("* *");
strLiterals -= strLiterals.FindByName("*.*");
strLiterals -= strLiterals.FindByName("*/*");
strLiterals -= strLiterals.FindByName("*\\*");

CxList lit_in_rSide = strLiterals.FindByAssignmentSide(CxList.AssignmentSide.Right);
CxList initializedPassword = psw_in_lSide_decl.FindByInitialization(lit_in_rSide);

//remove passwords with equal name and contant ==> currPassword = "currPassword";
CxList notHdPass = All.NewCxList();
char[] trimChars = new char[2] { '\'', '"'};
foreach(CxList currPass in initializedPassword)
{
	CxList currStrInLeft = lit_in_rSide.FindInitialization(currPass);
	string strName = currStrInLeft.GetName().Trim(trimChars);
	string passName = currPass.GetName();
	if (passName.Equals(strName))
	{
		notHdPass.Add(currPass);
	}
}
initializedPassword -= notHdPass;

// Find password in an "equals" operation
CxList eq = All.FindByMemberAccess("String.equals");
CxList equalsPassword = strLiterals.GetByAncs(eq * psw.GetMembersOfTarget());

eq *= strLiterals.GetMembersOfTarget();
equalsPassword.Add(psw.GetByAncs(eq));

//equals of type "string".equals(password);
CxList strEQ = strLiterals.GetMembersOfTarget().FindByShortName("equals");
strEQ = psw.GetByAncs(strEQ);
equalsPassword.Add(strEQ);

// Find password in assignments
CxList assignPassword = psw_in_lSide.GetAncOfType(typeof(AssignExpr));
assignPassword = lit_in_rSide.GetByAncs(assignPassword);

CxList methods = Find_Methods();
CxList connection = methods.FindByShortName("getConnection");
CxList sqlNewInstance = methods.FindByName("*Sql.newInstance");
CxList connetionParam2 = All.GetParameters(connection, 2) + All.GetParameters(sqlNewInstance, 2);

CxList connetionParam0 = All.GetParameters(connection, 0);
CxList pwdInFirstConnectionParam = connetionParam0.FindByType(typeof(StringLiteral)).FindByName("*PWD=*") + 
	connetionParam0.FindByType(typeof(StringLiteral)).FindByName("*PWD =*");



// Add also KerberosKey's second parameter as a potentially vulnerable hardcoded parameter
CxList KerberosKey = All.FindByType("KerberosKey");
KerberosKey = 
	KerberosKey.FindByType(typeof(ObjectCreateExpr)) + 
	KerberosKey.FindByType(typeof(Declarator));

// Get second parameter
CxList KerberosKeyParam1 = All.GetParameters(KerberosKey, 1);

// Add also KerberosPrincipal's second parameter as a potentially vulnerable hardcoded parameter
CxList PasswordAuthentication = All.FindByType("PasswordAuthentication");
PasswordAuthentication = 
	PasswordAuthentication.FindByType(typeof(ObjectCreateExpr)) + 
	PasswordAuthentication.FindByType(typeof(Declarator));

// Get second parameter
CxList PasswordAuthenticationParam1 = All.GetParameters(PasswordAuthentication, 1);

CxList relevantParams = KerberosKeyParam1 + connetionParam2 + PasswordAuthenticationParam1;

// Sanitize by binaries such as "+" and by concatenate - could be concatenated with a non hard-coded key, 
// which is OK
CxList bin = All.FindByType(typeof(BinaryExpr));
bin = bin.FindByShortName("");
CxList concat = All.FindByShortName("concatenate", false);

CxList sanitize = bin + concat;
CxList undefinedMethods = methods - methods.FindAllReferences(All.FindDefinition(methods));
sanitize.Add(undefinedMethods);

// Add the parameter itself, or whatever is influencing it
CxList paramsAffectedByString = relevantParams * strLiterals + 
	relevantParams.InfluencedByAndNotSanitized(strLiterals, sanitize);

CxList setPasswordMethod = Find_Methods().FindByShortName("setPassword", false);
CxList passwordParams = All.GetParameters(setPasswordMethod).FindByType(typeof(StringLiteral));
CxList hardcodedPasswordInMethod = setPasswordMethod.DataInfluencedBy(passwordParams);

// All
result = initializedPassword + 
	equalsPassword + 
	assignPassword + 
	paramsAffectedByString + 
	hardcodedPasswordInMethod +
	pwdInFirstConnectionParam;