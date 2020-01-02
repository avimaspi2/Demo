CxList binaryExpressions = All.FindByType(typeof(BinaryExpr));
CxList unaryExpressions = All.FindByType(typeof(UnaryExpr));
var booleanOperators = new List<Checkmarx.Dom.BinaryOperator>
	{
		Checkmarx.Dom.BinaryOperator.BooleanOr,
		Checkmarx.Dom.BinaryOperator.BooleanAnd,
		Checkmarx.Dom.BinaryOperator.IdentityInequality,
		Checkmarx.Dom.BinaryOperator.IdentityEquality,
		Checkmarx.Dom.BinaryOperator.GreaterThanOrEqual,
		Checkmarx.Dom.BinaryOperator.LessThanOrEqual,
		Checkmarx.Dom.BinaryOperator.GreaterThan,
		Checkmarx.Dom.BinaryOperator.LessThan,
		Checkmarx.Dom.BinaryOperator.Is,
		Checkmarx.Dom.BinaryOperator.RegexMatch
		};
        
foreach (Checkmarx.Dom.BinaryOperator binaryOperator in booleanOperators)
{
	result.Add(binaryExpressions.GetByBinaryOperator(binaryOperator));
}
        
foreach (CxList unaryExpression in unaryExpressions)
{
	UnaryExpr castedUnaryExpr = unaryExpression.TryGetCSharpGraph<UnaryExpr>();
            
	if (castedUnaryExpr != null && castedUnaryExpr.Operator == Checkmarx.Dom.UnaryOperator.Not)
	{
		result.Add(unaryExpression);
	}
}