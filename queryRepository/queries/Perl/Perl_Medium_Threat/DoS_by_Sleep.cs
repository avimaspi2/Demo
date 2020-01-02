CxList Inputs = Find_Interactive_Inputs();
CxList methods = Find_Methods();
CxList sleepMethods = methods.FindByName("sleep");
CxList sleepParameters = All.GetParameters(sleepMethods) - All.FindByType(typeof(Param));
CxList integersAbstractValues = sleepParameters.FindByAbstractValue(abstractValue => abstractValue is IntegerIntervalAbstractValue);
IAbstractValue intervalOneToMil = new IntegerIntervalAbstractValue(1, 1000000);
CxList validIntervals = integersAbstractValues.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(intervalOneToMil));
CxList vulnerableSleepInvokes = sleepMethods.FindByParameters(sleepParameters - validIntervals);
result = vulnerableSleepInvokes.DataInfluencedBy(Inputs);