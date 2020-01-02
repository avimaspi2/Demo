/////////////////////////////////////////////////////////////////////////
// Query Access_Specifier_Manipulation
// Purpose : Find use of AccessibleObject.setAccessible with parameter
//           true (it could be either the first or the second parameter)

CxList methods = Find_Methods();
CxList setAcc = methods.FindByMemberAccess("Method.setAccessible");
setAcc.Add(methods.FindByMemberAccess("Field.setAccessible"));
setAcc.Add(methods.FindByMemberAccess("Constructor.setAccessible"));

CxList booleans = Find_BooleanLiteral().FindByShortName("true");

result = booleans.InfluencingOn(setAcc);