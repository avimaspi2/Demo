if ((All.FindByFileName(@"*.cshtml").Count > 0 || All.FindByFileName(@"*.aspx").Count > 0) &&
	(All.FindByFileName(@"*controllers*").Count > 0 || All.FindByFileName(@"*views*").Count > 0))
{
	CxList controllerMethods = Find_ASP_MVC_Controllers();
	CxList paramCollections = All.FindByType(typeof(ParamDeclCollection)).FindByFathers(controllerMethods);
	result.Add(All.FindByType(typeof(ParamDecl)).GetByAncs(paramCollections));
	result.Add(Find_MVC_Input_Annotations());
}