/// <summary>
///this query lead with permissive content 
///security policy in cordova projects
/// </summary>
string regex = @"meta\s+(http-equiv=""Content-Security-Policy"")\s+(content=""[^""]*"")";
List<string> controlFile = new List<string>();

CxList htmlFileNames = Find_Security_Policies_Web_Files();
CxList configCordova = Cordova_Find_Config_Files();
foreach(CxList myFile in configCordova){
	string fullPath = myFile.GetFirstGraph().LinePragma.FileName;
	int lastPosition = fullPath.LastIndexOf('\\') + 1;
	string subPath = fullPath.Substring(0, lastPosition);
	try{
		// Files that are affected by the current config.xml	
		CxList htmlFileNamesSubPath = htmlFileNames.FindByFileName(subPath+"*");
		
		foreach(CxList currentFile in htmlFileNamesSubPath){				
			try{
				string fileName = currentFile.GetFirstGraph().LinePragma.FileName;
					
				if(!controlFile.Contains(fileName)){
					controlFile.Add(fileName);
					
					string shortName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
					CxList originsInHtml = All.FindByRegexExt(regex, shortName);					
					CxList origins = originsInHtml.FindByFileName(fileName);
					
					result.Add(Cordova_Get_Problematic_Content_Security_Policies(origins));
				}
			}catch(Exception exc) {}
		}
	}catch(Exception exc){}	
}