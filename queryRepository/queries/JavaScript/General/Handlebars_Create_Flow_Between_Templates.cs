// Query to create a flow between non mapped Handlebars template invocations and non mapped templates

if(cxScan.IsFrameworkActive("Handlebars"))
{
	CxList templateInvocations = Find_Non_Mapped_Template_Invocations();
	CxList templates = Find_Non_Mapped_Templates();

	foreach (CxList templateInvocation in templateInvocations)
	{
		foreach (CxList template in templates)
		{
			CustomFlows.AddFlow(All.GetParameters(templateInvocation, 0), All.GetParameters(template, 0));
		}
	}
}