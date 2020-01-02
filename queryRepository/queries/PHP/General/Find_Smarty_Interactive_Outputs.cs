CxList methods = Find_Methods();
CxList smartyMethods = methods.FindByFileName("*.tpl");

//smarty output tag from the sort : {$variable}. These tags are pre-processed into smarty_output($variable)

CxList smartyVariableOutputs = smartyMethods.FindByShortName("smarty_output");

CxList smartyOutputs = smartyMethods.FindByShortNames(new List<string> 
	{"html_checkboxes", "html_image", "html_options", "html_radios","html_table", "mailto", "html_select_date",
		"html_select_time", "smarty_eval", "smarty_include"});

smartyOutputs.Add(methods.FindByMemberAccess("Smarty.display"));
 
//finds the 'fetch' functions which contain only one parameter
CxList fetchMathod = smartyMethods.FindByShortName("fetch");
CxList allSmartyParams = All.GetParameters(smartyMethods);
foreach (CxList singleMethod in fetchMathod)
{
	CxList methodsParams = allSmartyParams.GetParameters(singleMethod).FindByType(typeof(Param));
	if (methodsParams.Count == 1)
	{
		smartyOutputs.Add(singleMethod);
	}
}

result.Add(smartyVariableOutputs + smartyOutputs);