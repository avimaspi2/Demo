CxList methods = Find_Methods();
//Find method loadComponent
CxList components = methods.FindByShortName("loadComponent", false);
//Get Parameters of the components
CxList parameters = All.GetParameters(components, 0);
//Check if components have the Csrf parameter
CxList csrf = parameters.FindByShortNames(new List<string>(){"Csrf", "Security"}, false);
//If it has the sanitizer get the ClassController in cause
CxList controller = csrf.GetAncOfType(typeof(ClassDecl)).FindByShortName("*Controller");
//everything is sanitized
result.Add(All.GetByAncs(controller));

//Find method array
CxList arrayMethods = methods.FindByShortName("array", false);
//Get Parameters of the arrays
CxList arrayParameters = All.GetParameters(arrayMethods, 0);
//Check if arrays have the Security parameter
CxList security = arrayParameters.FindByShortName("Security", false);
//If it has the sanitizer get the ClassController in cause
CxList controller2 = security.GetAncOfType(typeof(ClassDecl)).FindByShortName("*Controller");
//everything is sanitized
result.Add(All.GetByAncs(controller2));

//Find method off
CxList offMethods = methods.FindByShortName("off", false);
//Get Parameters of the off methods
CxList offParameters = All.GetParameters(offMethods, 0);
//Check if off methods have the Csrf parameter
CxList offCsrf = offParameters.FindByShortName("Csrf", false);
//If it's turning the csrf token off get the ClassController in cause
CxList controller3 = offCsrf.GetAncOfType(typeof(ClassDecl)).FindByShortName("*Controller");

//support cases like  $this->Security->csrfCheck = false
CxList csrfChecks = All.FindByMemberAccess("Security.csrfCheck");
CxList assigner = csrfChecks.GetAssigner();
CxList csrf_disablers = assigner.FindByShortName("false");
CxList controller4 = csrf_disablers.GetAncOfType(typeof(ClassDecl)).FindByShortName("*Controller");

//everything below these controllers is vulnerable
CxList controllers_to_remove = controller3;
controllers_to_remove.Add(controller4);

result -= All.GetByAncs(controllers_to_remove);