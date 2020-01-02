// This query tries to find hardcoded keys in the code, that can be used to encrypt data
// It does that searching for the following items:
//    * Fields named in a suspect way
//    * Usage of hardcoded (or influenced by hardcoded) strings as parameters of *SecretKeySpec objects

CxList cryptKeys = Find_Hard_Coded_Cryptographic_Encription_Keys();
cryptKeys -= Find_Properties_Files();
CxList keysInLeftSide = cryptKeys.FindByAssignmentSide(CxList.AssignmentSide.Left);

CxList methodInvokeExpr = Find_Methods();

// Java Properties File  
CxList methodInvokes = methodInvokeExpr.FindByMemberAccess("Properties", "getProperty");
CxList getPropertyMethods = (All.FindByParameters(cryptKeys).GetAncOfType(typeof(MethodInvokeExpr)) * methodInvokes);

keysInLeftSide.Add(getPropertyMethods.GetAssignee());

CxList nullLiteral = All.FindByName("null");
CxList booleans = Find_BooleanLiteral();
booleans.Add(All.FindByType("bool"));

CxList nullsAndBooleans = All.NewCxList();
nullsAndBooleans.Add(nullLiteral);
nullsAndBooleans.Add(booleans);

CxList strLiterals = Find_PrimitiveExpr();
strLiterals -= nullsAndBooleans;

CxList literalInRightSide = strLiterals.FindByAssignmentSide(CxList.AssignmentSide.Right);

CxList keysLeftmostTarget = keysInLeftSide.GetLeftmostTarget();

CxList keysInLeftSideFathers = keysInLeftSide.GetFathers();
keysInLeftSideFathers.Add(keysLeftmostTarget.GetFathers());

CxList fathers = keysInLeftSideFathers * literalInRightSide.GetFathers();

// Adds sources that are part of an assignment with the string literals
result = keysInLeftSide.FindByFathers(fathers);

// Adds leftmost target of source if it is part of an assignment with the string literals
result.Add(keysLeftmostTarget.FindByFathers(fathers));

// Add declarators named as the specified keys
result.Add(literalInRightSide.GetFathers().FindByType(typeof(Declarator)) * keysInLeftSide);

CxList fieldsAndConstants = All.NewCxList();
fieldsAndConstants.Add(Find_FieldDecls());
fieldsAndConstants.Add(Find_Constants());
	
CxList keyInField = All.GetByAncs(fieldsAndConstants) * cryptKeys;

CxList encryptRef = All.FindAllReferences(keyInField);
CxList encryptRefDecl = encryptRef.FindByType(typeof(Declarator));
CxList badEncryptRefDecl = encryptRefDecl - encryptRefDecl.GetByAncs(Find_MethodDecls());
result.Add((encryptRef - badEncryptRefDecl).DataInfluencedBy(strLiterals));

// Add also SecretKeySpec's first parameter as a potentially vulnerable hardcoded parameter
CxList SecretKeySpecTypes = All.FindByTypes(new string [] {
	"SecretKeySpec",
	"KeySpec",
	"DESKeySpec",
	"DESedeKeySpec",
	"EncodedKeySpec",
	"PBEKeySpec",
	"PKCS8EncodedKeySpec",
	"X509EncodedKeySpec"});

CxList SecretKeySpec = All.NewCxList();
SecretKeySpec.Add(SecretKeySpecTypes.FindByType(typeof(ObjectCreateExpr)));
SecretKeySpec.Add(SecretKeySpecTypes.FindByType(typeof(Declarator)));

// Get first parameter of *SecretKeySpec
CxList SecretKeySpecParam0 = All.GetParameters(SecretKeySpec, 0);
// Sanitize by binaries such as "+" and by concatenate - could be concatenated with a non hard-coded key, 
// which is OK
CxList concatBinExpr = Find_BinaryExpr().GetByBinaryOperator(BinaryOperator.Add);

CxList concat = All.FindByShortName("concatenate", false);
CxList nonKey = methodInvokeExpr.FindByMemberAccess("SecretKeyFactory.getInstance");
nonKey.Add(All.GetParameters(methodInvokeExpr.FindByMemberAccess("String.split")));

CxList unknownReferences = Find_UnknownReference();
CxList getInstance = methodInvokeExpr.FindByMemberAccess("KeyGenerator.getInstance");
getInstance.Add(unknownReferences.InfluencedBy(getInstance));
CxList generateKey = getInstance.GetMembersOfTarget().FindByShortName("generateKey");

CxList sanitize = All.NewCxList();
sanitize.Add(concatBinExpr);
sanitize.Add(concat);
sanitize.Add(nonKey);
sanitize.Add(generateKey);

// Add the parameter itself, or whatever is influencing it
result.Add(SecretKeySpecParam0 * strLiterals);
result.Add(SecretKeySpecParam0.InfluencedByAndNotSanitized(strLiterals, sanitize));