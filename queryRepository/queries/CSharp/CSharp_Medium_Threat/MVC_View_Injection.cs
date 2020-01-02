CxList methodInvokes = All.FindByType(typeof(MethodInvokeExpr));
CxList strings = All.FindByTypes(new String[] {"String", "System.String"});

CxList input = Find_Interactive_Inputs() + Find_ASP_MVC_Inputs();
CxList views = methodInvokes.FindByShortName("View");
CxList RedirectToAction_methods = methodInvokes.FindByShortName("RedirectToAction");

CxList views_injectable_parameters = strings.GetParameters(views, 0);
views_injectable_parameters.Add(strings.GetParameters(RedirectToAction_methods));

result = views_injectable_parameters.InfluencedByAndNotSanitized(input, Find_Encode() + Find_Integers());