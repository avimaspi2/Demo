//Direct ResesponseWriter Write
result.Add(All.FindByMemberAccess("http.ResponseWriter", "Write", false));
result.Add(All.FindByMemberAccess("http.ResponseWriter", "WriteHeader", false));
result.Add(Find_Set_Headers());

CxList ioWriterMembers = All.FindByMemberAccess("\"io\".WriteString");
ioWriterMembers.Add(All.FindByMemberAccess("\"fmt\".Fprintf"));

CxList responseWriters = All.FindAllReferences(All.FindByType("http.ResponseWriter"));
result.Add((All.GetParameters(ioWriterMembers, 0) * responseWriters).GetAncOfType(typeof(MethodInvokeExpr)));