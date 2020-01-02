CxList Inputs = Find_Interactive_Inputs();
CxList sleep = All.FindByName("*Thread.Sleep", false);
result = sleep.DataInfluencedBy(Inputs);