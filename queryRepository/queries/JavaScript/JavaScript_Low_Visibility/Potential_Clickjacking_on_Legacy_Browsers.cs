//checks wheter there is insufficient clickJacking protection implemented in Code.
result = Find_Insufficient_ClickJacking_Protection_OnClient();

// Add to it all the files in which there's no protection at all
CxList conditions = Find_Conditions();

CxList htmlDocs = All.FindByRegexExt("<", "*.html");
htmlDocs.Add(All.FindByShortName("CxJSNS*").GetAncOfType(typeof(NamespaceDecl)));

// In this query, we only return the first result found. So that this doesn't result in a consistency problem,
// we'll first order the nodes by the order of their filename.
SortedList<string, CxList> sortedRelevantHtmlDocs = new SortedList<string, CxList>();
foreach (CxList doc in htmlDocs)
{
	try
	{
		// Get the fileName
		CSharpGraph g = doc.GetFirstGraph();
		string filename = g.LinePragma.FileName;
		if (result.FindByFileName(filename).Count != 0)
			continue;
		
		string extension = "";
		int index = filename.LastIndexOf('.');
		if (index != -1)
			extension = filename.Substring(index).ToLower();
		
		// Ignore JS extension
		List<string> ignoreFiles = new List<string> {".js",".xsjs",".xsjslib",".xsaccess",".xsapp",".xml", ".json", ".ts", ".tsx"};
		if(ignoreFiles.Contains(extension))
			continue;
		
		if (!sortedRelevantHtmlDocs.ContainsKey(filename)) {
			sortedRelevantHtmlDocs.Add(filename, doc.Clone());
		}
	}
	catch(Exception ex){
		cxLog.WriteDebugMessage(ex);	
	}
}

// Now itrate through the relevent CxLists and return the first which isn't protected
foreach(var doc in sortedRelevantHtmlDocs)
{	
	// Find all elements in the file
	CxList allInThisFile = All.FindByFileName(doc.Key);
		
	// Add all files that don't protect themselves
	bool foundProtection = false;
	CxList top_frames = allInThisFile.FindByShortName("top");
	CxList self = allInThisFile.FindByShortName("self");
	CxList location = allInThisFile.FindByShortName("location");
	CxList length = allInThisFile.FindByShortName("length");
		
	CxList conditionsInThis = conditions * allInThisFile;
	foreach(CxList condition in conditionsInThis)
	{
		if (top_frames.GetByAncs(condition).Count > 0 && 
			(self.GetByAncs(condition).Count > 0 || 
			location.GetByAncs(condition).Count > 0 ||
			length.GetByAncs(condition).Count > 0))
		{
			foundProtection = true;
			break;
		}
	}
		
	if (!foundProtection)
	{
		result.Add(doc.Value);
		break;
		// We only want to return this result once, since this vulniribility can be protected againts in the 
		// configuration (something we can't know about), and we don't want to flood the user with results
	}
}