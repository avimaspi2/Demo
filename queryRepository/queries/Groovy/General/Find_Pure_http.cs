CxList localhost = All.FindByName(@"*localhost*");
CxList http = All.FindByName(@"*http:*");

CxList pureHttp = http + localhost;
pureHttp -= pureHttp.FindByName(@"*https:*");

CxList pureReferences = All.FindAllReferences(All.FindByType(typeof(Declarator)).DataInfluencedBy(pureHttp));

result = pureHttp + pureReferences;