CxList safe = All.FindByShortName("SAFE");
CxList safeNumber = All.GetByAncs(safe.GetAncOfType(typeof(AssignExpr)));
safeNumber = safeNumber.FindByShortName("0").FindByType(typeof(IntegerLiteral));

result = safeNumber.DataInfluencingOn(safe);