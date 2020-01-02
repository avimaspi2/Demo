CxList appController = All.FindByType(typeof(ClassDecl)).InheritsFrom("ApplicationController") 
	+ All.FindByType(typeof(ClassDecl)).FindByShortName("ApplicationController");

CxList appControllerContent = All.GetByAncs(appController);
CxList protect_from_forgery = appControllerContent.FindByShortName("protect_from_forgery");
protect_from_forgery = 
	protect_from_forgery.FindByType(typeof(UnknownReference)) +
	protect_from_forgery.FindByType(typeof(MethodInvokeExpr));
protect_from_forgery.Add((appControllerContent * Find_Methods()).FindByShortName("verify"));

if (protect_from_forgery.Count == 0)
{
	result = appController;
}