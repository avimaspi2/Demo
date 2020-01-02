//<Core:html> is not vulnerable if "sanitizeContent" property is set to true
CxList coreHtmlInsideIfCondition = Find_Conditions().FindByShortName("core_html",false); 
CxList boolTrue = Find_Strings().FindByShortName("true"); 
CxList propertiesSetToTrue = boolTrue.GetAssignee();
CxList sanitizeContentSetToTrue = propertiesSetToTrue.FindByShortName("sanitizecontent",false);
CxList outputs = Find_SAPUI_Outputs_XSS();
outputs.Add(Find_Framework_Outputs());

foreach(CxList elem  in coreHtmlInsideIfCondition)
{
	CxList ifStmt = elem.GetFathers();
	CxList property = sanitizeContentSetToTrue.GetByAncs(ifStmt);
	if(property.Count > 0)
	{
		result.Add(All.GetParameters(outputs.GetByAncs(ifStmt)));	
	}
}

result.Add(Find_SAPUI5_Sanitize().FindByShortNames(new List<string> {"encodeHTML", "escapeHTML", "encodeXML"}));