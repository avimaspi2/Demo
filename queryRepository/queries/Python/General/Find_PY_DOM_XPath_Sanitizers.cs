CxList py_dom_xpath_list = All.NewCxList();

try
{
	py_dom_xpath_list = (param.Length == 1) ? param[0] as CxList : Find_PY_DOM_XPath();
} 
catch (Exception ex)
{
	cxLog.WriteDebugMessage(ex);
}