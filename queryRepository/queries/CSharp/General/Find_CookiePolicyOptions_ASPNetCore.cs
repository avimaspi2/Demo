CxList ASPNetCoreStartupClasses = Find_Classes().FindByShortName("Startup");
CxList ASPNetCoreServiceConfigMethods = Find_MethodDecls()
	.FindByShortName("ConfigureServices").GetByAncs(ASPNetCoreStartupClasses);
CxList ASPNetCoreConfigure = Find_Methods().FindByMemberAccess("IServiceCollection.Configure")
	.GetByAncs(ASPNetCoreServiceConfigMethods);
CxList CookiePolicyOptionsType = Find_TypeRef().FindByShortName("CookiePolicyOptions");
CxList CookiePolicyOptionsConfigure = CookiePolicyOptionsType.GetFathers() * ASPNetCoreConfigure;

result = CookiePolicyOptionsConfigure;
result.Add(ASPNetCoreStartupClasses - (ASPNetCoreStartupClasses.GetClass(CookiePolicyOptionsConfigure)));