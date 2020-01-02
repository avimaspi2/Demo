// Script poisoning - influencing the timeout of a script

CxList inputs = Find_Interactive_Inputs();
CxList timeout = All.FindByName("*Server.ScriptTimeout", false);

result = inputs.DataInfluencingOn(timeout);