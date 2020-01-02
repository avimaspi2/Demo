CxList methods = Find_Methods();

CxList cshtmlContent = methods.FindByFileName("*.cshtml");
cshtmlContent.Add(methods.FindByFileName("*.aspx"));

result = cshtmlContent.FindByMemberAccess("Html.*");