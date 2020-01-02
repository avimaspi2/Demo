CxList localhost = All.FindByName(@"*localhost*");
CxList http = All.FindByName(@"*http:*");

CxList pureHttp = All.NewCxList();
pureHttp.Add(http);
pureHttp.Add(localhost);
pureHttp -= pureHttp.FindByName(@"*https:*");

CxList pureReferences = All.FindAllReferences(Find_Declarators().DataInfluencedBy(pureHttp));

result = pureHttp;
result.Add(pureReferences);