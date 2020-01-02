CxList methodDecls = Find_MethodDecls();
CxList methodCalls = Find_Methods();
CxList classes = Find_Classes();
CxList startupClasses = Find_ClassDecl().FindByShortName("Startup");
CxList configureMethods = methodDecls.FindByShortName("Configure").GetByAncs(startupClasses);
CxList hstsUsage = Find_Methods().FindByMemberAccess("IApplicationBuilder.UseHsts");

CxList configureServices = methodDecls.FindByShortName("ConfigureServices");
CxList servicesConfigurationMethodCall = methodCalls.FindByMemberAccess("IServiceCollection.Configure").GetByAncs(configureServices);
CxList hstsOptionsTypes = Find_TypeRef().FindByShortName("HstsOptions");
CxList hstsConfigureMethodCall = hstsOptionsTypes.GetFathers() * servicesConfigurationMethodCall.GetByAncs(configureServices);

CxList addHstsMethods = methodCalls.FindByShortName("AddHsts").GetByAncs(startupClasses);

result = hstsUsage.GetByAncs(configureMethods);
result.Add(addHstsMethods);
// Add classes without any hsts configuration.
result.Add(startupClasses - (classes.GetClass(hstsConfigureMethodCall) + classes.GetClass(addHstsMethods) + classes.GetClass(hstsUsage)));
result.Add(hstsConfigureMethodCall);