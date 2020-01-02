if(cxScan.IsFrameworkActive("Jelly"))
{
	CxList methods = Find_Methods();
	CxList htmlEscape = methods.FindByShortNames(new List<string> {"CxEscapeHtml", "CxEscapeAll"});
	
	result = All.GetByAncs(htmlEscape);
}