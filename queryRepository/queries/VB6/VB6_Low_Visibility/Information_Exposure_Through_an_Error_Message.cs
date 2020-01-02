CxList outputs = Find_Outputs() + Find_Log_Outputs();
CxList errors = All.FindByName("Err.*", false);
result = outputs.DataInfluencedBy(errors);