////////////////////////////////////////////////////////////////////////////////////////
// Missing_X_Frame_Options query
// The query makes sure a web application sets the X-Frame-Options of the response header 
// to prevent XSS in an iFrame.
// If X-Frame-Options is not found, the result is retured for the first line of web.config
// or first arbitrary line if web.config is not found.
////////////////////////////////////////////////////////////////////////////////////////
if(AllMembers.All.FindByLanguage("CSharp").Count == 0)
{
	if(All.isWebApplication)
	{	
		CxList strings = Find_Strings();
		CxList xFrameOptions = strings.FindByName("X-Frame-Options", false);

		if(xFrameOptions.Count == 0)
		{
			CxList webConfig = Find_Web_Config();
		
			// Find the XML config class of the web.config (first line)
			result = webConfig.FindByName("CxXmlConfigClass*", false).FindByType(typeof(ClassDecl));
		
			// If web.config is absent, just find first position in first file.
			if(result.Count == 0)
			{
				foreach(CxList l in All)
				{
					result = l;
					break;
				}
			}
		}
	}
}