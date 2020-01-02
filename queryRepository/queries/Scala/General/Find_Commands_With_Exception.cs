CxList methods = Find_Methods();

CxList otherCommandsWithException = methods.FindByMemberAccess("System.loadLibrary");

result.Add(Find_IO());
result.Add(Find_FileSystem_Read());
result.Add(Find_FileSystem_Write());
result.Add(Find_FileSystem_Sanity_Checks());
result.Add(Find_DB());
result.Add(otherCommandsWithException);


result -= result.GetParameters(result); // leave only commands, not parameters
result -= result.GetByAncs(methods.FindByShortName("getAttribute")); // remove "2nd-level" inputs and DB