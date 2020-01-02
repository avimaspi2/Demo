//Kohana sanitizers:
CxList methods = Find_Methods();

CxList checkMethods = methods.FindByShortName("check");

CxList getInst = All.FindByMemberAccess("Validation.factory");
getInst.Add(All.FindByMemberAccess("Validate.factory"));

CxList createExp = Find_ObjectCreation();
getInst.Add(createExp.FindByShortName("Validate"));
getInst.Add(createExp.FindByShortName("Validation"));

//finds only the invokes of a Kohana validation/validate
CxList getInstInfluences = checkMethods.GetTargetOfMembers().DataInfluencedBy(getInst);
CxList isValidMethods = (getInst + getInstInfluences).GetMembersOfTarget() * checkMethods;

//handling if-statement with check() method
CxList output = Find_Interactive_Outputs();

CxList ifStmt = (All.FindByType(typeof(IfStmt)));
CxList exp = All.FindByType(typeof(Expression));
CxList isValidMethodAsCond = isValidMethods.GetByAncs(exp.FindByFathers(ifStmt));
CxList relevantIfStmt = isValidMethodAsCond.GetAncOfType(typeof(IfStmt));

CxList isNOTValidMethodAsCond = isValidMethodAsCond.GetByAncs(All.FindByShortName("Not").FindByType(typeof(UnaryExpr)));

CxList sanitizedOutput = output.GetByAncs(relevantIfStmt - isNOTValidMethodAsCond.GetAncOfType(typeof(IfStmt)));
result.Add(sanitizedOutput);

//Kohana dedicated sanitizers
result.Add(methods.FindByMemberAccess("Kohana.sanitize"));
result.Add(methods.FindByMemberAccess("Kohana_Core.sanitize"));
result.Add(methods.FindByMemberAccess("Security.encode_php_tags"));
result.Add(methods.FindByMemberAccess("Security.strip_image_tags"));

result.Add(methods.FindByShortName("xss_clean")); //deprecated since Kohana 3.0

CxList unknownRefs = Find_UnknownReferences();
result.Add(methods.FindByMemberAccess("HTML.chars"));

result.Add(methods.FindByMemberAccess("HTML.entities"));