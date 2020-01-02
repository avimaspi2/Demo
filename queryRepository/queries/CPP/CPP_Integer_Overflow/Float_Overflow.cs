CxList smallTypeList = Find_Builtin_Types().FindByType("float");
CxList bigTypeList = Find_Builtin_Types().FindByType("double");

CxList sanitized = Sanitize_Integer_Overflow();

smallTypeList = smallTypeList.FindByAssignmentSide(CxList.AssignmentSide.Left) * smallTypeList;

CxList assignments = smallTypeList.GetAncOfType(typeof(AssignExpr));
CxList declarators = smallTypeList.GetAncOfType(typeof(Declarator));

// Remove expressions that are casted to float
CxList smallCasts =Find_TypeRef().FindByShortName("float").GetAncOfType(typeof(CastExpr));
bigTypeList -= bigTypeList.GetByAncs(smallCasts);

CxList rSide = Get_Right_Assignment()*bigTypeList;
CxList sanitizedRSide = rSide - sanitized; 

CxList directResults = sanitizedRSide.GetAncOfType(typeof(AssignExpr))*assignments +
	sanitizedRSide.GetAncOfType(typeof(Declarator))*declarators;

CxList indirectResults = sanitizedRSide.FindByAssignmentSide(CxList.AssignmentSide.Left).GetAncOfType(typeof(AssignExpr)).GetFathers().GetAncOfType(typeof(AssignExpr));
indirectResults *= assignments;

result.Add(directResults + indirectResults);