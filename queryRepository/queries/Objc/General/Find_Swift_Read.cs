CxList methods = Find_Methods();

CxList dictionaries = methods.FindByName("NSDictionary:");
CxList first_parameters = All.GetParameters(dictionaries, 0);
CxList parameters = Find_By_Type_And_Casting(first_parameters, "NSURL");
parameters.Add(Find_By_Type_And_Casting(first_parameters, "NSString"));
parameters.Add(Find_By_Type_And_Casting(first_parameters, "string"));
parameters.Add(Find_By_Type_And_Casting(first_parameters, "URL"));

CxList read = methods.FindByName("NSString:encoding*");
read.Add(methods.FindByShortName("NSString:usedEncoding*"));
read.Add(methods.FindByShortName("NSMutableString:encoding*"));
read.Add(methods.FindByShortName("NSMutableString:usedEncoding*"));
read.Add(methods.FindByShortName("NSString*").FindByParameterName("format", 0));
read.Add(All.FindByParameters(parameters));

List<string> collectionsNames = new List<string> { 		
		"NSDictionary:", "NSMutableDictionary:",
		"NSArray:", "NSMutableArray:"
		};
read.Add(methods.FindByShortNames(collectionsNames).FindByParameterName("contentsOfFile", 0));
read.Add(methods.FindByShortNames(collectionsNames).FindByParameterName("contentsOfURL", 0));
read.Add(methods.FindByShortNames(collectionsNames).FindByParameterName("contentsOf", 0));
	
result = read;