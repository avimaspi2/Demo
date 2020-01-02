CxList NULL = All.FindByType(typeof(NullLiteral));
CxList eq = Find_Methods().FindByShortName("Equals");

result = eq.FindByParameters(NULL);