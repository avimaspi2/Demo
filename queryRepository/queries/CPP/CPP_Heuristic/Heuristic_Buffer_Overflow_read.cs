// The correct sanitization shoud be added

CxList read = Find_Methods().FindByShortName("read");
read.Add(Find_Methods().FindByShortName("pread"));
read.Add(Find_Methods().FindByShortName("pread64"));

// Find input influence on size of copy
CxList sizeParam = All.GetParameters(read,2);
sizeParam = All.GetByAncs(sizeParam);	
CxList inputs = Find_Interactive_Inputs();

result = sizeParam.DataInfluencedBy(inputs);