CxList methods = Find_Methods();
result = methods.FindByShortName("fwrite");

CxList fileOpen = All.FindByMemberAccess("File.open");
result.Add(methods.GetByAncs(fileOpen).FindByShortName("write"));
result.Add(fileOpen.GetMembersOfTarget().FindByShortName("write"));

result.Add(Find_Log_Outputs());