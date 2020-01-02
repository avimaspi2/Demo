CxList condition = Find_Conditions(); //NodeJS_Find_Conditions_Parameters();

CxList one = condition.FindByType(typeof(IntegerLiteral)).FindByShortName("1");
CxList oneBinExpr = one.GetFathers().FindByType(typeof (BinaryExpr));
CxList oneInBinExpr = one.GetByAncs(oneBinExpr);

CxList bTrue = condition.FindByType(typeof(BooleanLiteral)).FindByShortName("true", false);
// Remove code created in preprocessing
bTrue = bTrue.FindByRegex("true");
CxList binExpr = bTrue.GetFathers().FindByType(typeof (BinaryExpr));

CxList bTrueInBinExpr = binExpr.GetByAncs(binExpr);

result = (bTrue - bTrueInBinExpr);
result.Add(one - oneInBinExpr);