CxList ctch = All.FindByType(typeof(Catch));
CxList exc = All.FindAllReferences(ctch) - ctch;
CxList outputs = Find_Interactive_Outputs() + Find_Console_Outputs();

result = outputs.DataInfluencedBy(exc);