CxList inputs = Find_Interactive_Inputs();

CxList methods = Find_Methods();
	
CxList files = Find_File_Paths();
CxList filesAccessExludeList = methods.FindByShortNames(new List<String>()
	{ "fputs", "filemtime", "is_dir", "is_executable", "is_file", "is_link", "realpath", "basename", "filesize" });

CxList firstParam = All.GetParameters(methods.FindByShortName("fputs"), 0);;

filesAccessExludeList = All.GetParameters(filesAccessExludeList);
files = firstParam + files - filesAccessExludeList;

CxList numberSanitizer = methods.FindByShortNames(new List<String>(){ "round", "doubleval", "strlen" }, false);
CxList generalSanitizer = methods.FindByShortNames(new List<String>(){ "stripslashes*", "post_permalink" }, false);

CxList sanitized = Find_General_Sanitize();
sanitized.Add(Find_Integers());
sanitized.Add(numberSanitizer);
sanitized.Add(generalSanitizer);
sanitized.Add(methods.FindByShortName("*sanitiz*"));//user_defined function for sanitation
sanitized.Add(Find_Replace());
sanitized.Add(Find_Server_Param_Sanitizer());
result = files.InfluencedByAndNotSanitized(inputs, sanitized);
result.Add(files * inputs);