CxList methods = Find_Methods();

// 1. writeToFile:
CxList writeToFileOptions = methods.FindByShortNames(new List<string>{"writeToFile:options:*", "writeToURL:options:*", "write:options:*"});
CxList options = All.GetByAncs(All.GetParameters(writeToFileOptions, 1));

CxList secureOptions = 
	options.FindByShortNames(new List<string>{"NSDataWritingFileProtectionComplete*", "completeFileProtection"});
//Options can be provided as follows '.write(to: path, options: [.dataWritingAtomic])'
secureOptions.Add(secureOptions.GetAncOfType(typeof(ArrayInitializer)));


// 2. createFileAtPath:
CxList createFileAtPath = All.FindByParameters(writeToFileOptions).FindByShortName("createFileAtPath:contents:attributes:*");
CxList attributes = All.GetParameters(createFileAtPath, 2);
CxList secureAttr = All.FindByShortName("NSFileProtectionComplete*");
CxList secureParam = attributes.DataInfluencedBy(secureAttr);
CxList createFileAtPathSecure = 
	createFileAtPath.FindByParameters(secureParam.GetAncOfType(typeof(Param)));

result = writeToFileOptions.FindByParameters(secureOptions).GetTargetOfMembers();
result.Add(writeToFileOptions.GetParameters(createFileAtPathSecure, 1));