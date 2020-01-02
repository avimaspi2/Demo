/////////////////////////////////////////////////////////////////
// Query: Input_Path_Not_Canonicalized
// Purpose: Make sure that input paths are canonicalized before 
//          being used in filesystem operations.
/////////////////////////////////////////////////////////////////

CxList inputs = Find_Inputs();
CxList methods = Find_Methods();

// Find file open
CxList fileOpen = All.FindByTypes(new string[] { "FileOutputStream","FileInputStream"});
fileOpen = fileOpen * Find_Object_Create();
fileOpen.Add(Find_FileSystem_Read());
fileOpen.Add(Find_FileSystem_Write());

// Methods that return the canocial path will serve as sanitizers when used on a comparison
CxList sanitize = methods.FindByMemberAccess("File.getCanonicalPath");
sanitize.Add(All.FindByMemberAccess("File.canonicalPath"));
sanitize.Add(methods.FindByMemberAccess("Path.normalize"));
sanitize.Add(methods.FindByMemberAccess("FilenameUtils.normalize"));
sanitize.Add(methods.FindByMemberAccess("FilenameUtils.normalizeNoEndSeparator"));
sanitize.Add(methods.FindByMemberAccess("URI.normalize"));
sanitize.Add(sanitize.GetTargetOfMembers());

// Find comparisons
CxList compare = Find_String_Compare();
compare.Add(methods.FindByShortName("replace*", false));
compare -= Find_Excludes_From_Path_Compare(compare);

sanitize.Add(Find_Integers() - compare);

// Find places where inputs paths are compared without canonicalization
inputs = inputs.InfluencingOnAndNotSanitized(compare, sanitize);
result = inputs.InfluencingOnAndNotSanitized(fileOpen, sanitize);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);