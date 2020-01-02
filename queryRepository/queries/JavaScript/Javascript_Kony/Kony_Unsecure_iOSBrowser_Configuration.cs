/*
	This query finds Kony Browser widgets with no property baseurl
*/
if(cxScan.IsFrameworkActive("KonyInFF"))
{
	CxList konyAll = Kony_All();
	
	string[] widgetTypes = new string[]{
		"kony.ui.Browser",
		};
	
	CxList widgets_decl = Kony_Find_Widgets_Declarations(widgetTypes);
	CxList jsMemberAccesses = (Find_MemberAccesses() * konyAll).FindByFileName("*.json");
	jsMemberAccesses = jsMemberAccesses.FindAllReferences(widgets_decl);
	
	// baseURL
	foreach(CxList widget_decl in widgets_decl){
		CxList widget_properties = jsMemberAccesses.FindAllReferences(widget_decl).GetMembersOfTarget();
		CxList baseurl = widget_properties.FindByShortName("baseurl");
		
		bool baseurl_isUnsecure = baseurl.GetAssigner(konyAll).Count == 0;
		
		if(baseurl_isUnsecure){
			result.Add(widget_decl);
		}
	}
}