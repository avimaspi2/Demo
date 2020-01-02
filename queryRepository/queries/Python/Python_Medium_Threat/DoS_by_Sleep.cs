CxList inputs = Find_Interactive_Inputs();
CxList methods = Find_Methods();

CxList sleep = methods.FindByShortName("sleep");

sleep.Add(Find_Members("time.sleep", methods));
sleep.Add(Find_Methods_By_Import("time", new string[]{"sleep"}));

result = sleep.DataInfluencedBy(inputs);