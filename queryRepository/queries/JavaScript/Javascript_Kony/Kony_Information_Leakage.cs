/*
	This query finds password widgets with securetextentry set to false
*/
if(cxScan.IsFrameworkActive("KonyInFF"))
{
	CxList konyAll = Kony_All();
	
	string[] widgetTypes = new string[]{
		"kony.ui.TextBox2",
		"kony.ui.RichText",
		};
	
	CxList widgets_decl = Kony_Find_Widgets_Declarations(widgetTypes);
	CxList jsMemberAccesses = (Find_MemberAccesses() * konyAll).FindByFileName("*.json");
	
	CxList widgets_passwords = widgets_decl.FindByShortName("*pass*", false);
	widgets_passwords.Add(widgets_decl.FindByShortName("*pwd*", false));
	
	foreach(CxList widget_password in widgets_passwords){
		
		CxList widget_properties = jsMemberAccesses.FindAllReferences(widget_password).GetMembersOfTarget();
		CxList securetextentry = widget_properties.FindByShortName("securetextentry");
		bool isUnSecure = securetextentry.GetAssigner(konyAll).FindByShortName("false").Count > 0;
		if(isUnSecure){
			result.Add(securetextentry);
		}
	}
}