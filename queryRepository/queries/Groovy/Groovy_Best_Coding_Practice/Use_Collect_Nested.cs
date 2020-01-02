// Instead of nested collect calls use collectNested.
CxList collectCalls = Find_Methods().FindByName("*.collect");
CxList parametersCollect = Find_Closures().GetParameters(collectCalls);
result = collectCalls.GetByAncs(parametersCollect);