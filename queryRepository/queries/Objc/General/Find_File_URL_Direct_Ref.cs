CxList vulnerbaleResults = All.NewCxList();

try
{

	CxList methods = Find_Methods();
	
	// NSString
	string[] apiNSString = new String[]{"NSString","String"};
	
	List<string> NSStringWriteMethods = new List<string>() {"writeToFile:*"};	
	List<string> NSStringContentsFileMethods = new List<string>() {
			"*WithContentsOfFile:*",
			"*contentsOfFile:*",
			"*contentsOf:*",
			// Swift
			"NSString:contentsOfFile:encoding:",
			"NSString:contentsOfFile:encoding:error:",
			"NSString:contentsOf:encoding:",
			"NSString:contentsOf:encoding:error:"			
			};
	
	// writeToFile
	CxList writeToFileAux = methods.FindByShortNames(NSStringWriteMethods);	
	// - writeToFile:atomically:encoding:error:
	
	CxList writeMethods = methods.FindByShortNames(new List<string>() {"write:*"});	
	writeToFileAux.Add(writeMethods.FindByParameterName("toFile", 0));	
	writeToFileAux.Add(writeMethods.FindByParameterName("to", 0));
	
	writeToFileAux.Add(All.FindByTypes(apiNSString).GetMembersOfTarget().FindByShortNames(NSStringWriteMethods));
	
	CxList writeToFile = All.FindByParameters(All.GetParameters(writeToFileAux, 0).FindByTypes(apiNSString));	
	writeToFile.Add(All.FindByParameters(All.GetParameters(writeToFileAux, 0)));
	
	// WithContentsOfFile
	CxList withContentsOfFileAux = methods.FindByShortNames(NSStringContentsFileMethods);	
	
	CxList nsStringMethods = methods.FindByShortNames(new List<string>() {"NSString:*"});	
	withContentsOfFileAux.Add(nsStringMethods.FindByParameterName("contentsOfFile", 0));
	withContentsOfFileAux.Add(nsStringMethods.FindByParameterName("contentsOfURL", 0));	
	
	withContentsOfFileAux.Add(All.FindByTypes(apiNSString).GetMembersOfTarget().FindByShortNames(NSStringContentsFileMethods));
	
	CxList withContentsOfFile = All.FindByParameters(All.GetParameters(withContentsOfFileAux, 0).FindByTypes(apiNSString));
	withContentsOfFile.Add(All.FindByParameters(All.GetParameters(withContentsOfFileAux, 0)));
	
	// NSURL
	string[] apiNSURL = new String[]{"NSURL","URL"};
	
	List<string> NSURLWriteMethods = new List<string>() {
			"writeToURL:*"		
			};	
	
	List<string> NSURLContentsFileMethods = new List<string>() {
			"*WithContentsOfURL:*",
			"contentsOfURL:*",
			"contentsOf:*"
			};
	
	// writeToURL
	CxList writeToUrlAux = methods.FindByShortNames(NSURLWriteMethods);
	// - writeToURL:atomically:encoding:error:
	writeToFileAux.Add(writeMethods.FindByParameterName("to", 0));
	writeToFileAux.Add(writeMethods.FindByParameterName("toUrl", 0));
	writeToUrlAux.Add(All.FindByTypes(apiNSString).GetMembersOfTarget().FindByShortNames(NSURLWriteMethods));
	
	CxList writeToUrl = All.FindByParameters(All.GetParameters(writeToUrlAux, 0).FindByTypes(apiNSURL));	
	writeToUrl.Add(All.FindByParameters(All.GetParameters(writeToUrlAux, 0)));
	
	// WithContentsOfURL
	CxList withContentsOfUrlAux = methods.FindByShortNames(NSURLContentsFileMethods);	
	withContentsOfUrlAux.Add(All.FindByTypes(apiNSString).GetMembersOfTarget().FindByShortNames(NSURLContentsFileMethods));
	
	CxList withContentsOfUrl = All.FindByParameters(All.GetParameters(withContentsOfUrlAux, 0).FindByTypes(apiNSURL));
	withContentsOfUrl.Add(All.FindByParameters(All.GetParameters(withContentsOfUrlAux, 0)));	
	
	// NSDictionary
	
	string[] apiNSDictionary = new String[]{ "NSDictionary" };
	
	List<string> NSDictionaryMethods = new List<string>() {
			"dictionaryWithContentsOfFile:*",
			"dictionaryWithContentsOfURL:*",
			"contentsOfFile",
			"contentsOf:*",		
			};	
	
	// dictionaryWithContentsOfFile
	CxList dicContentsOfFileAux = methods.FindByShortNames(NSDictionaryMethods);		
	dicContentsOfFileAux.Add(All.FindByTypes(apiNSDictionary).GetMembersOfTarget().FindByShortNames(NSDictionaryMethods));
	
	CxList dicContentsOfFile = All.FindByParameters(All.GetParameters(dicContentsOfFileAux, 0).FindByTypes(apiNSDictionary));	
	dicContentsOfFile.Add(All.FindByParameters(All.GetParameters(dicContentsOfFileAux, 0)));
	
	// dictionaryWithContentsOfURL
	CxList dicContentsOfUrlAux = methods.FindByShortNames(NSDictionaryMethods);	
	
	CxList nsDictionaryMethods = methods.FindByShortNames(new List<string>() {"NSDictionary:*"});	
	dicContentsOfUrlAux.Add(nsDictionaryMethods.FindByParameterName("contentsOfFile", 0));
	dicContentsOfUrlAux.Add(nsDictionaryMethods.FindByParameterName("contentsOfURL", 0));	
	dicContentsOfUrlAux.Add(nsDictionaryMethods.FindByParameterName("contentsOf", 0));	
	
	dicContentsOfUrlAux.Add(All.FindByTypes(apiNSDictionary).GetMembersOfTarget().FindByShortNames(NSDictionaryMethods));
	
	CxList dicContentsOfUrl = All.FindByParameters(All.GetParameters(dicContentsOfUrlAux, 0).FindByTypes(apiNSDictionary));	
	dicContentsOfUrl.Add(All.FindByParameters(All.GetParameters(dicContentsOfUrlAux, 0)));
	
		
	// NSInputStream
	string[] apiNSInputStream = new String[]{"NSInputStream", "InputStream"};
	
	List<string> inputStreamMethods = new List<string>() {
			"inputStreamWithFileAtPath:*",
			"inputStreamWithURL:*",
			"initWithFileAtPath:*",
			"initWithURL:*",		
			// Swift		
			"NSInputStream:fileAtPath:",
			"NSInputStream:URL:",
			"NSInputStream:url:",		
		
			"InputStream:fileAtPath:",
			"InputStream:URL:",
			"InputStream:url:"		
			};
	
	CxList nsInputStream = methods.FindByShortNames(inputStreamMethods);	
	
	nsInputStream.Add(All.FindByTypes(apiNSInputStream).GetMembersOfTarget().FindByShortNames(inputStreamMethods));
	nsInputStream.Add(nsInputStream.GetMembersOfTarget()); // include all methods calls of NSInputStream	
	CxList nsInputStreamMethods = nsInputStream.FindByShortNames(inputStreamMethods);
	
	CxList nsInputStreamMethodsAll = methods.FindByShortNames(new List<string>() {"NSInputStream:"});	
	
	nsInputStreamMethods.Add(nsInputStreamMethodsAll.FindByParameterName("fileAtPath", 0));
	nsInputStreamMethods.Add(nsInputStreamMethodsAll.FindByParameterName("URL", 0));
	nsInputStreamMethods.Add(nsInputStreamMethodsAll.FindByParameterName("url", 0));
	
	CxList inputStreamMethodsAll = methods.FindByShortNames(new List<string>() {"InputStream:"});	
	
	nsInputStreamMethods.Add(inputStreamMethodsAll.FindByParameterName("fileAtPath", 0));
	nsInputStreamMethods.Add(inputStreamMethodsAll.FindByParameterName("URL", 0));
	nsInputStreamMethods.Add(inputStreamMethodsAll.FindByParameterName("url", 0));
	
	// NSOutputStream
	
	string[] apiNSOutputStream = new String[]{"NSOutputStream", "OutputStream"};
	
	List<string> outputStreamMethods = new List<string>() {
			"outputStreamToFileAtPath:*",
			"outputStreamWithURL:*",
			"initToFileAtPath:*",
			"initWithURL:*",		
			// Swift			
			"NSOutputStream:toFileAtPath:append:",		
			"NSOutputStream:url:append:",
			"NSOutputStream:append:",		
			
			"OutputStream:toFileAtPath:append:",		
			"OutputStream:url:append:",
			"OutputStream:append:"
			
			};
	
	CxList nsOutputStream = methods.FindByShortNames(outputStreamMethods);	
	nsOutputStream.Add(All.FindByTypes(apiNSOutputStream).GetMembersOfTarget().FindByShortNames(outputStreamMethods));
	nsOutputStream.Add(nsOutputStream.GetMembersOfTarget()); // include all methods calls of NSOutputStream	
	CxList nsOutputStreamMethods = nsOutputStream.FindByShortNames(outputStreamMethods);
	
	CxList nsOutputStreamMethodsAll = methods.FindByShortNames(new List<string>() {"NSOutputStream:"});	
	
	nsOutputStreamMethods.Add(nsOutputStreamMethodsAll.FindByParameterName("toFileAtPath", 0));
	nsOutputStreamMethods.Add(nsOutputStreamMethodsAll.FindByParameterName("URL", 0));
	nsOutputStreamMethods.Add(nsOutputStreamMethodsAll.FindByParameterName("url", 0));
	
	CxList outputStreamMethodsAll = methods.FindByShortNames(new List<string>() {"OutputStream:"});	
	
	nsOutputStreamMethods.Add(outputStreamMethodsAll.FindByParameterName("toFileAtPath", 0));
	nsOutputStreamMethods.Add(outputStreamMethodsAll.FindByParameterName("URL", 0));
	nsOutputStreamMethods.Add(outputStreamMethodsAll.FindByParameterName("url", 0));	

	// NSFileManager	
	CxList nsFileManager = methods.FindByMemberAccess("NSFileManager.*");
	nsFileManager.Add(methods.FindByMemberAccess("FileManager.*"));
	nsFileManager.Add(All.FindByType("FileManager"));
	nsFileManager.Add(All.FindAllReferences(nsFileManager.GetAssignee()));	
	nsFileManager.Add(nsFileManager.GetMembersOfTarget()); // include all methods calls of NSFileManager

		
	List<string> fileManagerMethods = new List<string>() {
			"createFileAtPath:*","replaceItemAtURL:*",			
			"copyItemAtURL:*", "copyItemAtPath:*",
			"moveItemAtURL:*", "moveItemAtPath:*",
			"removeItemAtPath:*", "fileExistsAtPath:*",
			"fileExistsAtPath:*", "fileExistsAtPath:*",			
			"isReadableFileAtPath:*", "isWritableFileAtPath:*",
			"isExecutableFileAtPath:*", "isDeletableFileAtPath:*",			
			"componentsToDisplayForPath:*", "attributesOfItemAtPath:*",
			"attributesOfFileSystemForPath:*", "setAttributes:*",			
			"contentsAtPath:*", "contentsEqualAtPath:*",
			"changeFileAttributes:*", "createSymbolicLinkAtPath:*",			
			"fileAttributesAtPath:*", "fileSystemAttributesAtPath:*",
			"pathContentOfSymbolicLinkAtPath:*"
			};	
	
	CxList nsFileManagerMethods = nsFileManager.FindByShortNames(fileManagerMethods);
	
	List<string> meth = new List<string>() {
			"createFile:*","replaceItem:*",			
			"copyItem:*", "copyItem:*",
			"moveItem:*", "moveItem:*",
			"removeItem:*", "fileExists:*",
			"fileExists:*", "fileExists:*",			
			"isReadableFile:*", "isWritableFile:*",
			"isExecutableFile:*", "isDeletableFile:*",		
			"componentsToDisplay:*", "attributesOfItem:*",
			"attributesOfFileSystem:*",		
			"contents:*", "contentsEqual:*",
			"createSymbolicLink:*",			
			"fileAttributes:*", "fileSystemAttributes:*",
			"pathContentOfSymbolicLink:*"			
			};
	
	nsFileManagerMethods.Add(nsFileManager.FindByShortNames(meth).FindByParameterName("atPath", 0));
	nsFileManagerMethods.Add(nsFileManager.FindByShortNames(meth).FindByParameterName("at", 0));
	nsFileManagerMethods.Add(nsFileManager.FindByShortNames(meth).FindByParameterName("forPath", 0));	
	
	// C method 	
	CxList moreVulnerableMethods = methods.FindByShortNames(new List<string>() {"fopen","chmod","chown","stat","mktemp"});

	
	vulnerbaleResults.Add(writeToFile);
	vulnerbaleResults.Add(withContentsOfFile);
	vulnerbaleResults.Add(writeToUrl);
	vulnerbaleResults.Add(withContentsOfUrl);
	vulnerbaleResults.Add(dicContentsOfFile);
	vulnerbaleResults.Add(dicContentsOfUrl);
	vulnerbaleResults.Add(nsInputStreamMethods);
	vulnerbaleResults.Add(nsOutputStreamMethods);
	vulnerbaleResults.Add(nsFileManagerMethods);	
	vulnerbaleResults.Add(moreVulnerableMethods);	

}
catch (Exception error)
{
	cxLog.WriteDebugMessage(error);
}
result = vulnerbaleResults;