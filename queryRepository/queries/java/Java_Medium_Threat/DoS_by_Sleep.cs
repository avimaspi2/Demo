CxList Inputs = Find_Interactive_Inputs();
CxList sleep = All.FindByName("*Thread.sleep");

CxList tooltipDelay = Find_Jsp_Tags().GetMembersOfTarget().FindByMemberAccess("tooltipDelay.*");//.GetParameters(Find_Jsp_Tags());
tooltipDelay = Find_Methods().GetParameters(tooltipDelay);

CxList delay = All.NewCxList();
delay.Add(sleep);
delay.Add(tooltipDelay);

CxList inputStreamRead = All.FindByMemberAccess("InputStream.read");
CxList readFirstParam = All.GetParameters(inputStreamRead, 0);
CxList readThirdParam = All.GetParameters(inputStreamRead, 2);

//Sleep sanitization
CxList sleepMethods = All.NewCxList();
sleepMethods.Add(delay);
CxList sleepParameters = All.GetParameters(sleepMethods) - Find_Param();

CxList integersAbstractValues = sleepParameters.FindByAbstractValue(abstractValue => abstractValue is IntegerIntervalAbstractValue);
IAbstractValue intervalOneToMil = new IntegerIntervalAbstractValue(1, 1000000);
CxList validIntervals = integersAbstractValues.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(intervalOneToMil));
CxList vulnerableSleepInvokes = sleepMethods.FindByParameters(sleepParameters - validIntervals); 


CxList readThirdVarSleep = All.NewCxList();
readThirdVarSleep.Add(readThirdParam);
readThirdVarSleep.Add(vulnerableSleepInvokes);

CxList readFirstParamInputs = All.NewCxList();
readFirstParamInputs.Add(readFirstParam);
readFirstParamInputs.Add(Inputs);

result = readThirdVarSleep.DataInfluencedBy(readFirstParamInputs);