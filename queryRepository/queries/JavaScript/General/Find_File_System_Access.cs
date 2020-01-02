CxList invoke = Find_Methods();
result.Add(invoke.FindByShortNames(new List<string>{"getFile","getDirectory"}));

/* Find second and third parmeters of: sap.ui.core.util.File.save */
CxList sapLibrary = Find_SAP_Library();
CxList core = sapLibrary.GetMembersOfTarget().FindByShortName("core");
CxList util = core.GetMembersOfTarget().FindByShortName("util");
CxList file = util.GetMembersOfTarget().FindByShortName("File");

CxList saveMethod = invoke * file.GetMembersOfTarget().FindByShortName("save");

CxList fileNameParameter = All.GetParameters(saveMethod, 1);
CxList extensionParameter = All.GetParameters(saveMethod, 2);
result.Add(fileNameParameter);
result.Add(extensionParameter);

/* Find saveAs method */
CxList saveAs = invoke.FindByMemberAccess("window.saveAs");
result.Add(All.GetParameters(saveAs, 1));

result -= Find_Param();