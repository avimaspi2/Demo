CxList AllMethodInvoke = Find_Methods();
CxList baseRefs = Find_BaseRef();
CxList superFinalize = All.FindByFathers(baseRefs.GetFathers()).FindByShortName("finalize");

result = AllMethodInvoke.FindByShortName("finalize") - superFinalize;