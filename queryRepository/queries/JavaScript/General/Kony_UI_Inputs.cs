if(cxScan.IsFrameworkActive("KonyInFF"))
{
	string[] widgetTypes = new string[]{
		"kony.ui.TextBox2",
		"kony.ui.RichText",
		"kony.ui.TextArea2"
		};

	CxList widgets = Kony_Find_Widgets(widgetTypes);

	result = widgets.GetMembersOfTarget();
}