// Check for C-arrays whose size specified as a literal instead of a macro or a constant.
CxList arrays = Find_ArrayCreateExpr();
CxList num = Find_IntegerLiterals();
CxList arrayChildren = num.GetByAncs(arrays);
CxList arraySizeLiterals = arrayChildren.FindByRegex(@"\[\s*\d+\s*\]");
result = arraySizeLiterals;