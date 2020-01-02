CxList include = All.FindByMemberAccess("response.include"); // prep from jsp:include
include.Add(All.FindByMemberAccess("response.Import")); // prep from c:import
include.Add(All.FindByMemberAccess("RequestDispatcher.include"));
include.Add(All.GetParameters(All.FindByMemberAccess("JspRuntimeLibrary.include"), 2)); // only second parameter is relevant

result = include;