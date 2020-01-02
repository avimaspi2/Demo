////////////////////////////////////////////////////////////////////////////////////////
// Missing_X_Frame_Options query
// The query makes sure a web application sets the X-Frame-Options of the response header 
// to prevent XSS in an iFrame.
// If X-Frame-Options is not found, the result is retured for the first line of web.xml
// or first arbitrary line if web.xml is not found.
////////////////////////////////////////////////////////////////////////////////////////

if(All.isWebApplication){
	CxList strings = Find_Strings();
	CxList xFrameOptions = strings.FindByName("\"X-FRAME-OPTIONS\"", false);
	
	if(xFrameOptions.Count == 0){
		CxList webxml = Find_Web_Xml();

		// Return the first line from web.xml
		result = webxml.FindByName("CxXmlConfigClass*", false).FindByType(typeof(ClassDecl));
		
		// If there were no results in web.xml - just return the first result found
		if(result.Count == 0){
			List < string > resultExtensions = new List<string>(){"java","jsp","jspf","jsf"};
			foreach(CxList l in All){
				CSharpGraph cs = l.TryGetCSharpGraph<CSharpGraph>(); 
				string fileName = cs.LinePragma.FileName;
				
				//to avoid getting results located in the plugins
				if (!fileName.Contains("Plugins\\Java")){
					string[] tokens = fileName.Split('.');
					string extension = tokens[tokens.Length - 1];
					if(resultExtensions.Contains(extension)){
						result = l;
						break;
					}
				}				
			}
		}		
	}
}