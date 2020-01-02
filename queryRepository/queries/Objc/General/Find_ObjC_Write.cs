CxList methods = Find_Methods();
CxList objCFiles = methods.FindByShortNames(new List<string> {"writeToFile:*","writeToURL:*"});

objCFiles -= objCFiles.FindByShortName("writeToURL:options:originalContentsURL:error:");

CxList initObjects = All.GetParameters(methods.FindByShortNames(new List<string>{
		"addRegularFileWithContents:preferredFilename:", "initRegularFileWithContents:",
		"initWithSerializedRepresentation:"}), 0);

initObjects.Add(All.GetParameters(methods.FindByShortName("createFileAtPath:contents:*"), 1));

initObjects -= initObjects.FindByType(typeof(Param));

result = objCFiles.GetTargetOfMembers();
result.Add(initObjects);

// write(toFile:atomically:) write(toUrl:atomically:) 
CxList swiftFiles = methods.FindByShortName("write:atomically*").FindByParameterName("toFile", 0);
swiftFiles.Add(methods.FindByShortName("write:atomically*").FindByParameterName("toUrl", 0));
swiftFiles.Add(methods.FindByShortName("write:atomically*").FindByParameterName("to", 0));

CxList fileManagerInitializers = methods.FindByShortName("FileManager:");
fileManagerInitializers.Add(All.FindByMemberAccess("FileManager.init:"));
swiftFiles.Add(fileManagerInitializers.FindByParameterName("regularFileWithContents", 0));
swiftFiles.Add(fileManagerInitializers.FindByParameterName("serializedRepresentation", 0));

fileManagerInitializers.Add(All.FindByType("FileManager"));
CxList fileManagerDeclarators = fileManagerInitializers.GetAncOfType(typeof(Declarator));
CxList managerCreateFilesList = All.FindAllReferences(fileManagerDeclarators).GetMembersOfTarget().FindByShortName("createFile:*");
swiftFiles.Add(managerCreateFilesList.FindByParameterName("atPath", 0).FindByParameterName("contents", 1).FindByParameterName("attributes", 2));



CxList fileWrapperInitializers = methods.FindByShortName("FileWrapper:");
fileWrapperInitializers.Add(All.FindByMemberAccess("FileWrapper.init:"));
fileWrapperInitializers.Add(All.FindByType("FileWrapper"));

CxList fileWrapperDeclarators = fileWrapperInitializers.GetAncOfType(typeof(Declarator));
CxList wrapperCreateFilesList = All.FindAllReferences(fileWrapperDeclarators).GetMembersOfTarget().FindByShortName("addRegularFile:*");
swiftFiles.Add(wrapperCreateFilesList.FindByParameterName("withContents", 0).FindByParameterName("preferredFilename", 1));



result.Add(swiftFiles);