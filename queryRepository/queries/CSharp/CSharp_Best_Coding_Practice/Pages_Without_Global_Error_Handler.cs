CxList webconfig = Find_Web_Config();

CxList customErrors = webconfig.FindByShortName("customErrors", false);
CxList mode = customErrors.GetMembersOfTarget().FindByShortName("mode", false);

CxList modeOff = mode.GetAssigner();

if(modeOff.FindByShortName("off", false).Count > 0 || customErrors.Count == 0)
{

	CxList errorHandledPages = All.GetClass(All.FindByName("*Page.ErrorPage"));
	CxList AllPages = All.GetClass(All.FindByName("*Page_Load*").FindByType(typeof(MethodDecl)));
	result = AllPages - errorHandledPages;  
}