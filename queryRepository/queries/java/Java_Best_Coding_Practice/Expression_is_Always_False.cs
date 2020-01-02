result = Find_False_Abstract_Value();

result -= result.FindByType(typeof(BooleanLiteral));
result -= result.FindByType(typeof(Declarator));

result = result * (Find_Conditions());