CxList activex = All.FindByShortName("createobject");

CxList inputs = Find_Inputs();

result = activex.DataInfluencedBy(inputs);