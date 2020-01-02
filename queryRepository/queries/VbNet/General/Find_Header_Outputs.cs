CxList header = All.FindByMemberAccess("HttpResponse.AddHeader", false);
header.Add(All.FindByMemberAccess("HttpResponse.AppendHeader", false));
header.Add(All.FindByName("*Response.AddHeader", false)); 
header.Add(All.FindByName("*Response.AppendHeader", false));
result = header;