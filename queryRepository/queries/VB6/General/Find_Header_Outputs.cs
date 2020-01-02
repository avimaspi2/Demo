CxList header = All.FindByMemberAccess("httpresponse.addheader") + 
				All.FindByMemberAccess("httpresponse.appendheader") + 
				All.FindByName("*response.addheader") +  
				All.FindByName("*response.appendheader");
result = header;