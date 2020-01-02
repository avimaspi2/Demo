//List of functions that sanitize File Inclusion vulnerable inputs
CxList methods = Find_Methods();
result = methods.FindByShortNames(new List<string>(){ "wp_unslash", "plugin_basename", "stripslashes_deep" }, false);

//get all instances of check_admin_referer (car)
CxList cars = methods.FindByShortName("check_admin_referer");
foreach (CxList car in cars)
{
	CxList scope = (car.GetFathers()).GetFathers();
	CxList scopeContent = All.GetByAncs(scope); //get all components of the car parameters
	CxList parameters = (scopeContent.GetParameters(car));
	parameters = scopeContent.GetByAncs(parameters);
	CxList refs = scopeContent.FindAllReferences(parameters);
	result.Add(refs);
}