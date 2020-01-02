CxList header = All.FindByMemberAccess("HttpResponse.AddHeader");
header.Add(All.FindByMemberAccess("HttpResponse.AppendHeader"));
header.Add(All.FindByName("*Response.AddHeader"));
header.Add(All.FindByName("*Response.AppendHeader"));

result = header;