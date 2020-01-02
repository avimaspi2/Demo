CxList inputs = Find_Interactive_Inputs();
CxList obj = All.FindByType(typeof(UnknownReference)) + All.FindByType(typeof(Declarator));
CxList files = 	obj.FindByType("*filestream") + 
				obj.FindByType("*fileinfo") +	
				All.FindByName("*.file.*");

CxList sanitize = All.FindByName("*server.mappath") + All.FindByName("*request.mappath"); 
result = files.InfluencedByAndNotSanitized(inputs, sanitize);