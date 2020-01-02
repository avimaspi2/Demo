/*
 MISRA CPP RULE 16-0-7
 ------------------------------
 This query searches for undefined macro identifiers in preprocessor directives.

 The Example below shows code with vulnerability: 

	#if x == 0		//Non-compliant
	#endif
	#ifdef y		//Compliant
	#elif k > 9		//Non-compliant
	#endif

*/

System.Collections.Generic.List<string> preDefined = new System.Collections.Generic.List<string>();
preDefined.Add("__LINE__");
preDefined.Add("__FILE__");
preDefined.Add("__DATE__");
preDefined.Add("__TIME__");
preDefined.Add("__STDC__");
preDefined.Add("__cplusplus");

//Find all #define and #if/#elif
CxList regexDefine = All.NewCxList();
CxList definedConsts = All.NewCxList();
CxList regexIfs = All.NewCxList();
All.FindByRegex(@"(?<=^\s*#\s*define\s+)\w+[^(]", regexDefine);
All.FindByRegex(@"^\s*#\s*(if|elif)\s", regexIfs);

CxList headerFiles = All.FindByType(typeof(StringLiteral)).GetParameters(Find_Methods()
	.FindByShortName("CX_INCL"));

ArrayList files = new ArrayList();
foreach(CxList cur in regexIfs){
	CSharpGraph curGraph = cur.data.GetByIndex(0) as CSharpGraph;
	if (!files.Contains(curGraph.LinePragma.FileName)){
		files.Add(curGraph.LinePragma.FileName);
	}
}

foreach (string curFile in files){
	// build list of all files that include current file
	CxList curFileIfs = regexIfs.FindByFileName(curFile);
	string[] splitFile = curFile.Split('\\');
	CxList includesOfCurFile = headerFiles.FindByShortName(splitFile[splitFile.Length - 1]);
	SortedList filesThatIncludeCurFile = new SortedList(new Checkmarx.DataCollections.PragmaComparer());
	
	foreach(CxList cur in includesOfCurFile){
		CSharpGraph curGraph = cur.data.GetByIndex(0) as CSharpGraph;
		LinePragma curPragma = new LinePragma();
		curPragma.FileName = curGraph.LinePragma.FileName;
		curPragma.Line = -1;
		curPragma.Column = -1;
		if (!filesThatIncludeCurFile.Contains(curPragma)){
			filesThatIncludeCurFile.Add(curPragma, null);
		}
	}
	LinePragma pragma = new LinePragma();
	pragma.FileName = curFile;
	pragma.Line = -1;
	pragma.Column = -1;
	filesThatIncludeCurFile.Remove(pragma);
	CxList curFileDefines = regexDefine.FindByPositions(filesThatIncludeCurFile, 0, false);
	//Extract all defined identifiers
	System.Collections.Generic.Dictionary<string,int> defined = 
		new System.Collections.Generic.Dictionary<string,int>();
	foreach(CxList curDefine in curFileDefines) 
	{
		string defineText = ((Comment) curDefine.data.GetByIndex(0)).CommentText;
		LinePragma lp = ((CSharpGraph) curDefine.data.GetByIndex(0)).LinePragma;
		defineText = defineText.Substring(lp.Column - 1);
		string[] split = defineText.Split(' ');
		if(!defined.ContainsKey(split[0])) {
			defined.Add(split[0], 0);
		}
	}
	filesThatIncludeCurFile.Clear();
	filesThatIncludeCurFile.Add(pragma, null);
	curFileDefines = regexDefine.FindByPositions(filesThatIncludeCurFile, 0, false);
	// go over all #if and #elif in curFile
	foreach(CxList curIf in curFileIfs)
	{
		//Split curIf into identifiers.
		string ifText = ((Comment) curIf.data.GetByIndex(0)).CommentText;
		LinePragma ifLp = ((CSharpGraph) curIf.data.GetByIndex(0)).LinePragma;
		foreach(CxList curDefine in curFileDefines) 
		{
			LinePragma defLp = ((CSharpGraph) curDefine.data.GetByIndex(0)).LinePragma;
			if(defLp.Line > ifLp.Line) {
				continue;
			}
			string defineText = ((Comment) curDefine.data.GetByIndex(0)).CommentText;
			defineText = defineText.Substring(defLp.Column - 1);
			string[] split = defineText.Split(' ');
			if(!defined.ContainsKey(split[0])) {
				defined.Add(split[0], 0);
			}
		}

		System.Text.RegularExpressions.Regex identPattern = new 
			System.Text.RegularExpressions.Regex(@"[A-Za-z_]\w*");
		System.Text.RegularExpressions.MatchCollection idents = identPattern.Matches(ifText);
		bool afterDefined = false;
		foreach(System.Text.RegularExpressions.Match ident in idents) 
		{
			string match = ident.Value;
			match = match.Trim();
			if(afterDefined) {
				afterDefined = false;
				continue;
			}
			if(match.Equals("defined") || match.Equals("!defined") ) {
				afterDefined = true;
				continue;	
			}
			if(preDefined.Contains(match) ||
			defined.ContainsKey(match) ||
			match.Equals("if") || match.Equals("elif")) {
				continue;
			}
			result.Add(curIf);
		}
	}
}
result = All.FindByRegexSecondOrder(".*", result);