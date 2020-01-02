// $ASP*

CxList Inputs = Find_Inputs();
CxList sleep = All.FindByName("WScript.Sleep", false);

result = sleep.DataInfluencedBy(Inputs);