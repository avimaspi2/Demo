// $ASP

CxList header = FindByMemberAccess_ASP("Response.AddHeader") + 
	FindByMemberAccess_ASP("Response.Cookies"); 
		 
result = header;