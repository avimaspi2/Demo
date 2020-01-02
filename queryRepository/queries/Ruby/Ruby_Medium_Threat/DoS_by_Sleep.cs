CxList Inputs = Find_Interactive_Inputs();
CxList methods = Find_Methods();
CxList sleep = methods.FindByName("sleep");

result = sleep.DataInfluencedBy(Inputs);