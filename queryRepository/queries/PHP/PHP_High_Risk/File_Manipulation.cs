CxList include = Find_Write();

CxList inputs = Find_Interactive_Inputs();

CxList methods = Find_Methods();

CxList filesAccessExcludeList = methods.FindByShortNames(new List<string>()
	{ "filemtime", "is_dir", "is_executable", "is_file", "is_link", "realpath", "basename", "filesize" });

CxList firstParam = All.GetParameters(methods.FindByShortName("fputs"), 0);

filesAccessExcludeList = All.GetParameters(filesAccessExcludeList);
include = firstParam + include - filesAccessExcludeList;

// find the file input sanitizers (number and string functions)
CxList sanitized = Find_File_Sanitizers();

result = include.InfluencedByAndNotSanitized(inputs, sanitized);