CxList data = All.FindByTypes(new String[]{"NSData","NSData:"});
CxList dataMembers = data.GetMembersOfTarget();
dataMembers.Add(dataMembers.FindByShortName("alloc").GetMembersOfTarget());
result = dataMembers.FindByShortNames(new List<string> {
		"initWithContentsOfFile:*",
		"initWithContentsOfURL:*" ,
		"dataWithContentsOfFile:*",
		"dataWithContentsOfURL:*",
		"dataWithContentsOfMappedFile:",
		"initWithContentsOfMappedFile:",
		//Swift
		"NSData:init:contentsOfFile:",
		"NSData:contentsOfFile:",
		"NSData:init:contentsOfFile:options:",
		"NSData:contentsOfFile:options:",
		"NSData:init:contentsOfMappedFile:options:",
		"NSData:contentsOfMappedFile:options:"
		});

result.Add(All.FindByShortName("NSData*").FindByParameterName("contentsOfFile", 0));
result.Add(All.FindByShortName("NSData*").FindByParameterName("contentsOfURL", 0));
result.Add(All.FindByShortName("NSData*").FindByParameterName("contentsOf", 0));
result.Add(All.FindByShortName("NSData*").FindByParameterName("contentsOfMappedFile", 0));

CxList dictionary = All.FindByTypes(new String[]{"NSDictionary","NSDictionary:"});
CxList dictionaryMembers = dictionary.GetMembersOfTarget();
dictionaryMembers.Add(dictionaryMembers.FindByShortName("alloc").GetMembersOfTarget());
result.Add(dictionaryMembers.FindByShortNames(new List<string> {
		"initWithContentsOfFile:",
		"initWithContentsOfURL:",
		"dictionaryWithContentsOfFile",
		"dictionaryWithContentsOfURL",
		//Swift
		"NSDictionary:init:contentsOfFile:",
		"NSDictionary:contentsOfFile:",
		"NSDictionary:init:contentsOf:",
		"NSDictionary:contentsOf:"	
		}));

result.Add(All.FindByShortName("NSDictionary*").FindByParameterName("contentsOfFile", 0));
result.Add(All.FindByShortName("NSDictionary*").FindByParameterName("contentsOfURL", 0));
result.Add(All.FindByShortName("NSDictionary*").FindByParameterName("contentsOf", 0));