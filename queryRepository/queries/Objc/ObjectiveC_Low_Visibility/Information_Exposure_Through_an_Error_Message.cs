CxList catchTypes = Find_Catch();
CxList ctch = All.FindAllReferences(catchTypes);
CxList outputs = Find_Outputs();
result = outputs.DataInfluencedBy(ctch);