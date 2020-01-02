CxList condition = Find_Condition();

CxList fls = condition.FindByType(typeof(BooleanLiteral)).FindByShortName("false", false);
CxList zero = condition.FindByType(typeof(IntegerLiteral)).FindByShortName("0");

result = fls.FindByRegex("false");
result.Add(fls.FindByRegex("FALSE"));
result.Add(fls.FindByRegex("NO"));
result.Add(zero.FindByRegex("0"));