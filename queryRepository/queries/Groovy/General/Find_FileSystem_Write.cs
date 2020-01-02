/*
	Find_FileSystem_Write
	This query looks for method invocations that write meta-information
	to the file system.
*/


CxList methods = Find_Methods();

string[] memberAccesses = new string[] {
	"File.renameTo",
	
	"Files.copy",
	"Files.move"
	};

foreach (string s in memberAccesses)
	result.Add(methods.FindByMemberAccess(s));



result.Add(Find_FileSystem_Create());
result.Add(Find_FileSystem_Write_Permissions());