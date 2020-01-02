CxList allExpr=Lightning_Find_All_Expressions_In_Project();
CxList bry=allExpr.FindByType(typeof(BinaryExpr));
CxList desc=cxXPath.GetAllExpressionDescendents(bry, 8);
CxList sanitizer=desc.FindByShortName("/");
CxList be=sanitizer.GetFathers();
result=cxXPath.GetAllExpressionDescendents(be, 8);