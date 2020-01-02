CxList header = All.FindByMemberAccess("HttpResponse.AddHeader") + 
				All.FindByMemberAccess("HttpResponse.AppendHeader") + 
				All.FindByName("*Response.AddHeader") +  
				All.FindByName("*Response.AppendHeader");
result = header;