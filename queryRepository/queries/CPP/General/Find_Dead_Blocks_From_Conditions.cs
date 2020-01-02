CxList True = Find_True_Abstract_Value();

CxList False = Find_False_Abstract_Value();

CxList conditions = Find_Conditions();

CxList trueConditions = conditions * True;
CxList falseConditions = conditions * False;

CxList trueConditionFathers = trueConditions.GetFathers();
CxList falseConditionFathers = falseConditions.GetFathers();

// The else block of an if(true)
result = trueConditionFathers.FindByType(typeof(IfStmt)).GetBlocksOfIfStatements(false);
// The if block of an if(false)
result.Add(falseConditionFathers.FindByType(typeof(IfStmt)).GetBlocksOfIfStatements(true));
// The blocks of an iteration with false condition
result.Add(falseConditionFathers.FindByType(typeof(IterationStmt)).GetBlocksOfIterationStatements());
// The true/false statements of a ternary expression with false/true condition
result.Add(falseConditionFathers.FindByType(typeof(TernaryExpr)).GetBranchesOfTernaryExpressions(true));
result.Add(trueConditionFathers.FindByType(typeof(TernaryExpr)).GetBranchesOfTernaryExpressions(false));