CxList code = Find_Methods().FindByShortName("CallByName", false);
CxList inputs = Find_Interactive_Inputs();

result = inputs.DataInfluencingOn(code);