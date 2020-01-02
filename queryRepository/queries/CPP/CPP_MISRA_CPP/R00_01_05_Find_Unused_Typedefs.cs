/*
 MISRA CPP RULE 0-1-5
 ------------------------------
 This query searches for unused typedefs throughout the project.
 
 The Example below shows code with vulnerability: 

		int unusedtype()
		{
			typedef int local_Type; //Non-compliant
			return 0;
		}

*/

//Find typedefs
CxList typedefs = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF");
typedefs = typedefs.GetAncOfType(typeof(VariableDeclStmt))
	+ typedefs.GetAncOfType(typeof(FieldDecl));
typedefs = Find_All_Declarators().GetByAncs(typedefs);

CxList unused = All.FindDefinition(All.FindAllReferences(typedefs) - typedefs);
unused = typedefs - unused;

//Start finding typedefs used with includes.
CxList headerFiles = All.FindByType(typeof(StringLiteral)).GetParameters(Find_Methods()
	.FindByShortName("CX_INCL"));

CxList typeRefs = All.FindByType(typeof(TypeRef));
ArrayList files = new ArrayList();
foreach(CxList cur in unused){
	CSharpGraph curGraph = cur.data.GetByIndex(0) as CSharpGraph;
	if (!files.Contains(curGraph.LinePragma.FileName)){
		files.Add(curGraph.LinePragma.FileName);
	}
}

foreach (string curFile in files){
	// build list of all files that include current file
	CxList curFileTypedefs = typedefs.FindByFileName(curFile);
	string[] splitFile = curFile.Split('\\');
	CxList includesOfCurFile = headerFiles.FindByShortName(splitFile[splitFile.Length-1], false);
	SortedList filesThatIncludeCurFile = new SortedList(new Checkmarx.DataCollections.PragmaComparer());
	foreach(CxList cur in includesOfCurFile){//Add filename of each include
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
	filesThatIncludeCurFile.Add(pragma, null);
	
	CxList curFileTypeRefs = typeRefs.FindByPositions(filesThatIncludeCurFile, 0, false);
	
	// go over all typedefs
	foreach(CxList curTypedef in curFileTypedefs)
	{
		// compare to all same name invokations
		CxList sameName = curFileTypeRefs.FindByShortName(curTypedef);
		if (sameName.Count > 0){
			unused -= curTypedef;
		}
	}
}
result = unused;