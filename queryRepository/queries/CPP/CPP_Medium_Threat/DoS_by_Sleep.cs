CxList Inputs = Find_Interactive_Inputs();
CxList sleep = Find_Methods().FindByShortName("sleep");

result = sleep.DataInfluencedBy(Inputs);