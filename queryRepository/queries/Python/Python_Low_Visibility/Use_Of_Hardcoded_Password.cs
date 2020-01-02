CxList emptyString = Find_Empty_Strings();
CxList nullLiteral = All.FindByShortName("none", false);
CxList psw = Find_Passwords();

// Find password in an initialization operation
CxList pswInlSide = psw.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList pswInlSideDecl = pswInlSide.FindByType(typeof(Declarator));

CxList emptyStringNull = All.NewCxList();
emptyStringNull.Add(emptyString);
emptyStringNull.Add(nullLiteral);

CxList strLiterals = Find_Strings() -  emptyStringNull;


//when the hardcoded string includes a space or dot we believe 
//it is not a password string
List<string> strNames = new List<string>{
		"* *","*.*","*/*","*\\*" };

strLiterals -= strLiterals.FindByShortNames(strNames);

CxList litInrSide = strLiterals.FindByAssignmentSide(CxList.AssignmentSide.Right);

CxList initializedPassword = pswInlSideDecl.FindByInitialization(litInrSide);

// Find password in an "equals" operation
CxList bin = Find_BinaryExpr();

CxList eq = bin.FindByShortName("==");
eq.Add(bin.FindByShortName("!="));

CxList equalsPassword = psw.GetFathers() * eq;
equalsPassword = strLiterals.FindByFathers(equalsPassword);

// Find password in assignments
CxList assignPassword = pswInlSide.GetAncOfType(typeof(AssignExpr));
assignPassword = litInrSide.GetByAncs(assignPassword);

//Passwords as method parameter
CxList methods = Find_Methods();
CxList connection = methods.FindByShortName("*connect*", false);
connection.Add(Find_DB_Conn_Strings());

CxList connetionParam2 = All.GetParameters(connection, 2);
CxList connetionParam1 = All.GetParameters(connection, 1);

CxList connetParams = All.NewCxList();
connetParams.Add(connetionParam1);
connetParams.Add(connetionParam2);

CxList pwdInConnectioParam = strLiterals.GetByAncs(connetParams * psw);
CxList ancsPsw = All.GetByAncs(All.GetParameters(connection).FindByType(typeof(ArrayInitializer)))
	.FindByType(typeof(UnknownReference)) * psw;

pwdInConnectioParam.Add(ancsPsw);

// Sanitize by binaries such as "+" and by concatenate - could be concatenated with a non hard-coded key, 
// which is OK
CxList sanitize = bin.FindByShortName("");

CxList undefinedMethods = methods - methods.FindAllReferences(All.FindDefinition(methods));
sanitize.Add(undefinedMethods);

// Add the parameter itself, or whatever is influencing it
CxList paramsAffectedByString = (connetionParam2 * strLiterals);
paramsAffectedByString.Add(connetionParam2.InfluencedByAndNotSanitized(strLiterals, sanitize));
paramsAffectedByString.Add((connetionParam1 * strLiterals));
paramsAffectedByString.Add(connetionParam1.InfluencedByAndNotSanitized(strLiterals, sanitize));

paramsAffectedByString *= psw;

CxList setPasswordMethod = methods.FindByShortName("setPassword", false);
CxList passwordParams = All.GetParameters(setPasswordMethod).FindByType(typeof(StringLiteral));
CxList hardcodedPasswordInMethod = setPasswordMethod.DataInfluencedBy(passwordParams);

// All
result.Add(initializedPassword);
result.Add(equalsPassword);
result.Add(assignPassword);
result.Add(paramsAffectedByString);
result.Add(hardcodedPasswordInMethod);
result.Add(pwdInConnectioParam);