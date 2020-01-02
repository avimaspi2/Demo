CxList Inputs = Find_Interactive_Inputs();
CxList sleep = Find_Methods().FindByName("sleep");
result = sleep.DataInfluencedBy(Inputs);