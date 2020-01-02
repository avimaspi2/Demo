result = All.FindByAbstractValue(abstractValue => abstractValue is FalseAbstractValue);
IAbstractValue zero = new IntegerIntervalAbstractValue(0);
result.Add(All.FindByAbstractValue(abstractValue => abstractValue is IntegerIntervalAbstractValue && zero.IncludedIn(abstractValue)));