if (2 == param.Length && null != param[0] as string && null != param[1] as string)
{
	string FileNameA = param[0] as string;
	string FileNameB = param[1] as string;
	
	CxList A = All.FindByFileName(FileNameA);
	CxList B = All.FindByFileName(FileNameB);
	
	// Heuristic!
	result = A.FindByRegex("<%@\\ include\\ file\\ =", true, false, false);
	
}
else if (2 == param.Length && null != param[0] as CxList && null != param[1] as CxList)
{
	CxList A = param[0] as CxList;
	CxList B = param[1] as CxList;
	
	string FileNameA = A.GetFirstGraph().LinePragma.FileName;
	string FileNameB = B.GetFirstGraph().LinePragma.FileName;
	
	result = JSP_Does_Include(FileNameA, FileNameB);
}