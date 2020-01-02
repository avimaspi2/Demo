result = All.FindByAbstractValue(abstractValue => abstractValue is TrueAbstractValue);
IAbstractValue notZero = new IntegerIntervalAbstractValue(null, null,
						new List<KeyValuePair<long, long>>(){new KeyValuePair<long, long>(0, 0)});
result.Add(All.FindByAbstractValue(abstractValue => abstractValue is IntegerIntervalAbstractValue && abstractValue.IncludedIn(notZero)));