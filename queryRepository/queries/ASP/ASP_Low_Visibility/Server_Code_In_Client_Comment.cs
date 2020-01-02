// Server-code in client-comment
// -----------------------------
// In this case one might expect the comment to be ignored, but actually
// it is part of the fully-functional server side
//////////////////////////////////////////////////////////////////////////////

//find server side code inside VBScript comment 
CxList candidatesVBScript = All.FindByRegex(@"\'\s*<%", All.NewCxList());
CxList vbs = AllMembers.All.FindByLanguage("VbScript");
CxList insideVBScript = All.NewCxList();

foreach(CxList elem in candidatesVBScript){
	CSharpGraph csg = elem.GetFirstGraph();
	if(csg != null && csg.LinePragma != null && !String.IsNullOrEmpty(csg.LinePragma.FileName)){
		bool found = false;
		for(int i = csg.LinePragma.Line - 5; !found && i < (csg.LinePragma.Line + 5);  i++){
			// there must be VbScript elements at the same line to be a valid result
			if(vbs.FindByPosition(csg.LinePragma.FileName, i).Count > 0){
				insideVBScript.Add(elem);
				found = true;
			}
		}
	}
}

//find server side code inside Javascript comment 
CxList insideJavaScript = All.FindByRegex(@"[^p:]//\s*<%", true, false, false, All.NewCxList());

//return only server side code (no Jacascript or VBScript elements)
result = insideJavaScript;
result.Add(insideVBScript);