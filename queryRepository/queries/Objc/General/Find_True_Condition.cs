CxList condition = Find_Condition();
CxList integerConditions = condition.FindByType(typeof(IntegerLiteral));
CxList integerFalse = integerConditions.FindByShortName("0");

CxList bTrueAll = condition.FindByType(typeof(BooleanLiteral)).FindByShortName("true", false);
// Remove code created in preprocessing
CxList bTrue = All.NewCxList();
bTrue.Add(bTrueAll.FindByRegex("true"));
bTrue.Add(bTrueAll.FindByRegex("TRUE"));
bTrue.Add(bTrueAll.FindByRegex("YES"));

CxList trueLiteral = All.NewCxList();
trueLiteral.Add(bTrue);
trueLiteral.Add(integerConditions);
trueLiteral -= integerFalse;

CxList conditionFather = trueLiteral.GetFathers();

// Leave only if statements becuase in loops we might want to run forever
result = trueLiteral.FindByFathers(conditionFather.FindByType(typeof(IfStmt)));