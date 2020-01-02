CxList param3 = All.GetParameters(Find_Encryption_Methods(), 3);
CxList strings = Find_Strings();
CxList bin = Find_BinaryExpr();

result = param3.FindByType(typeof(StringLiteral));
result.Add(param3.InfluencedByAndNotSanitized(strings, bin));