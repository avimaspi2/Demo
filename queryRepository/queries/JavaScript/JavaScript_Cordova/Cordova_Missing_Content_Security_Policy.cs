/// <summary>
///this query lead with missing content 
///security policy in cordova projects
/// </summary>
string regex = @"meta\s+(http-equiv=""Content-Security-Policy"")\s+(content=""[^""]*"")";
string regexHead = @"<head[^>]*>";
List<string> controlFiles = new List<string>();
string fullPath, subPath;
int lastPosition;

CxList htmlFileNames = Find_Security_Policies_Web_Files();
CxList configCordova = Cordova_Find_Config_Files();

foreach(CxList myFile in configCordova){
	fullPath = myFile.GetFirstGraph().LinePragma.FileName;
	lastPosition = fullPath.LastIndexOf('\\') + 1;
	subPath = fullPath.Substring(0, lastPosition);

	try{
		// Files that are affected by the current config.xml
		CxList htmlFileNamesSubPath = htmlFileNames.FindByFileName(subPath + "*");
		
		foreach(CxList currentFile in htmlFileNamesSubPath){
			try{
				string fileName = currentFile.GetFirstGraph().LinePragma.FileName;
					
				if(!controlFiles.Contains(fileName)){
					controlFiles.Add(fileName);
					string shortName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
					CxList checkHead =  All.FindByRegexExt(regexHead, shortName);
					
					if(checkHead.Count > 0){
						CxList originsInHtml = All.FindByRegexExt(regex, shortName);
						CxList origins = originsInHtml.FindByFileName(fileName);
						
						if(origins.Count == 0 ){
							result.Add(currentFile);
						}
					}
				}
			}catch(Exception exc) {}
		}
	}catch(Exception exc){}
}