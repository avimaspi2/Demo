if(cxScan.IsFrameworkActive("KonyInFF"))
{
	string[] widgetTypes = new string[]{
		"kony.ui.Button",
		"kony.ui.Label",
		"kony.ui.Link",
		"kony.ui.RichText",
		"kony.ui.BarButtonItem",
		"kony.ui.Toast",
		"kony.ui.ActionSheet",
		"kony.ui.ActionItem"
		};

	CxList widgets = Kony_Find_Widgets(widgetTypes);
	result = widgets.GetMembersOfTarget().FindByShortName("text");
}