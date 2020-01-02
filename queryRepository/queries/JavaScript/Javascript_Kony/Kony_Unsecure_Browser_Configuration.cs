/*
	This query finds Kony Browser widgets with property enablenativecommunication set to true
*/
if(cxScan.IsFrameworkActive("KonyInFF"))
{
	string[] widgetTypes = new string[]{
		"kony.ui.Browser",
		};
	
	CxList widgets = Kony_Find_Widgets_Properties(widgetTypes);
	
	// enableNativeCommunication
	foreach(CxList widget in widgets){
		CxList enablenativecommunication = widget.FindByShortName("enablenativecommunication");
		
		bool enablenativecommunication_isUnsecure = enablenativecommunication.GetAssigner(Kony_All().FindByShortName("true")).Count > 0;
		
		if(enablenativecommunication_isUnsecure){
			result.Add(enablenativecommunication);
		}
	}
}