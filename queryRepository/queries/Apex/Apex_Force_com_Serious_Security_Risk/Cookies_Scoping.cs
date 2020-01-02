CxList methods = Find_Methods();
CxList put = methods.FindByShortName("put");
CxList firstParam = All.GetParameters(put, 0);
firstParam = firstParam.FindByShortName("'cookie'");

put = All.FindByParameters(firstParam);
CxList secondParam = All.GetParameters(put, 1);
CxList inputs = Find_Interactive_Inputs();
CxList influencingInput = inputs.DataInfluencingOn(secondParam);

result = put.InfluencedByAndNotSanitized(influencingInput, Find_Test_Code());

result -= Find_Test_Code();