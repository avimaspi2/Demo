CxList Inputs = Find_Interactive_Inputs();

CxList obj = All.FindByType(typeof(UnknownReference));
obj.Add(All.FindByType(typeof(Declarator)));
obj.Add(Find_Methods());

CxList files = obj.FindByType("*StreamReader", false);
files.Add(obj.FindByType("*FileStream", false));
files.Add(obj.FindByType("*FileInfo", false));
files.Add(obj.FindByType("*DirectoryInfo", false));
files.Add(obj.FindByMemberAccess("*File.*", false));
files -= files.FindByMemberAccess("Profile.*", false);
files.Add(obj.FindByMemberAccess("*Directory.*", false));

CxList sanitized = Find_Sanitize();

result = files.InfluencedByAndNotSanitized(Inputs, sanitized);