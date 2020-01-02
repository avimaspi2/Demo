CxList condition = Find_Conditions(); //NodeJS_Find_Conditions_Parameters();

CxList fls = condition.FindByType(typeof(BooleanLiteral)).FindByShortName("false", false);
CxList zero = condition.FindByType(typeof(IntegerLiteral)).FindByShortName("0");

CxList _fathers = fls.GetFathers();
_fathers.Add(zero.GetFathers());
CxList binExpr = _fathers.FindByType(typeof (BinaryExpr));

CxList flsInBinExpr = fls.GetByAncs(binExpr);
CxList zeroInBinExpr = zero.GetByAncs(binExpr);

CxList _res = fls - flsInBinExpr;
_res.Add(zero - zeroInBinExpr);
result = _res;