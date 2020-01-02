// Finds any upload which the size was not validated before storage
CxList conditions = Find_Conditions();
CxList methods = Find_Methods();
CxList variables = Find_UnknownReference();
variables.Add(Find_Declarators());

// upload abstraction Objects
CxList relevants = All.FindAllReferences(variables.FindByTypes(new string[]{
	"UploadItem", "CommonsMultipartFile", "MultipartFile", 
	"MockMultipartFile", "FileItem", "DiskFileItem",
	"FileItem", "DefaultFileItem", "CommonsMultipartResolver",
	"MultipartRequest", "Part", 
	"HttpServletRequest", "HttpServletRequestWrapper", 
	"ServletRequest", "ServletRequestWrapper" }));

CxList relevantMethods = relevants.GetMembersOfTarget();

// upload tests
CxList validators = relevantMethods.FindByShortNames(new List<string>{
		"getSize", "getFileSize", "getName", 
		"getContentType", "getSubmittedFileName"});
CxList validatedMethods = relevantMethods.GetByAncs(
	validators.GetByAncs(conditions).GetAncOfType(typeof(IfStmt)));
relevantMethods -= validatedMethods;

// Content access
CxList dataAccess = relevantMethods.FindByShortNames(new List<string>{
		// move
		"transferTo", "write",
		// content
		"getData", "get", "getFile", "getInputStream", "getBytes",
		"getOutputStream", "getFileData", });

result = All.FindDefinition(dataAccess.GetTargetOfMembers())
	.DataInfluencingOn(dataAccess);