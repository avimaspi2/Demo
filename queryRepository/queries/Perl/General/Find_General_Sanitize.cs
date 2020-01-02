// The sanitization is if there is some if-statements that checks a reference of conditions
//CxList binaryRegex = Find_Regex().GetAncOfType(typeof(BinaryExpr));
CxList conditions = Find_Conditions().FindByType(typeof(BinaryExpr));

CxList iter = All.FindByType(typeof(IterationStmt));
iter = conditions.FindByFathers(iter);
conditions -= iter;
CxList findMethods = Find_Methods();
CxList methods = findMethods * Find_Conditions();
CxList refCond = All.GetByAncs(methods + conditions/*+ binaryRegex*/).FindByType(typeof(UnknownReference));//removed regex in a binary expresion  - too strong sanitizer 
refCond -= refCond.GetMembersOfTarget().GetTargetOfMembers(); // remove A in   if (A.B == 0)   or alike
refCond -= refCond.GetByAncs(Find_DB());

CxList references = All.FindAllReferences(refCond);

CxList IfSanitize = references * references.InfluencingOnAndNotSanitized(refCond, Find_DB());

CxList sockets = methods.FindByShortName("socket");
CxList socketParams = All.GetParameters(sockets) - All.GetParameters(sockets, 0);

result = 
	IfSanitize +
	socketParams + 
	Find_Integers() + 
	findMethods.FindByShortName("encode*", false) +
	findMethods.FindByShortName("escape*", false) +
	findMethods.FindByShortName("*_escape", false) + //for example uri_escape(...)
	findMethods.FindByShortName("quotemeta", false);