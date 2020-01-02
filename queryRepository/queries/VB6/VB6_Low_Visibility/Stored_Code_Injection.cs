CxList code = Find_Methods().FindByShortName("CallByName", false);
CxList inputs = Find_DB_Out();
inputs.Add(Find_Read());

result = inputs.DataInfluencingOn(code);