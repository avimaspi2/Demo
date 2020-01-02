CxList match = All.FindByRegex(@"//\s*CxSanitizer_ParameterTampering", true, false, false);
CxList sanitizer = All.NewCxList();
foreach(CxList res in match)
{
	CSharpGraph r = res.data.GetByIndex(0) as CSharpGraph;
	if(r != null)
	{	
		int line = r.LinePragma.Line;		
		string fileName = r.LinePragma.FileName;		
		sanitizer.Add(All.FindByPosition(fileName, line));
	}
}
result.Add(sanitizer);