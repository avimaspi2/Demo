// See : https://www.securecoding.cert.org/confluence/pages/viewpage.action?pageId=76775519

// Find plain use of a 2 parameter open methods
CxList methods = Find_Methods();
CxList open = methods.FindByShortName("open");
CxList open2 = open.FindByParameters(All.GetParameters(open, 1));
CxList open3 = open.FindByParameters(All.GetParameters(open, 2));

result = (open2 - open3).DataInfluencedBy(Find_Inputs());

// Also <ARGV> and <> are using a 2 parameters open
result.Add(methods.FindByShortName("<?>").FindByParameters(All.FindByShortName("ARGV")));