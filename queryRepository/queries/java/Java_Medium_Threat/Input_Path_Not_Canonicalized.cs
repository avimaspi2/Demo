/////////////////////////////////////////////////////////////////
// Query: Input_Path_Not_Canonicalized
// Purpose: Make sure that input paths are canonicalized before 
//          being used in filesystem operations.
/////////////////////////////////////////////////////////////////

CxList inputs = Find_Inputs();
CxList methods = Find_Methods();

// Find file open
CxList fileOpen = Find_Files_Open();

// Methods that return the canocial path will serve as sanitizers when used on a comparison
CxList sanitize = methods.FindByMemberAccess("File.getCanonicalPath");
sanitize.Add(methods.FindByMemberAccess("Path.normalize"));
sanitize.Add(methods.FindByMemberAccess("FilenameUtils.normalize"));
sanitize.Add(methods.FindByMemberAccess("FilenameUtils.normalizeNoEndSeparator"));
sanitize.Add(methods.FindByMemberAccess("URI.normalize"));
sanitize.Add(sanitize.GetTargetOfMembers());

//Find all fileOpens that have some connection with sanitizers
CxList fileOpenSanitized = fileOpen.InfluencingOn(sanitize) + fileOpen.InfluencedBy(sanitize);

//Delete inputs that are sanitized
inputs -= inputs.InfluencingOn(fileOpenSanitized+sanitize);

inputs = inputs.InfluencingOn(fileOpen);
result = inputs.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);