CxList ctch = Find_Catch();

CxList exc = All.FindAllReferences(ctch) - ctch;

exc.Add(All.GetParameters(Find_Methods().FindByMemberAccess("CFile.Open"), 2));
CxList outputs = Find_Outputs();

result = outputs.DataInfluencedBy(exc);