//Search for Repeaters only in .aspx files and check if in the same file there is SqlDataSource 
//This is a heuristic solution to remove Repeaters that are not binded to DB
CxList aspx = All.FindByFileName("*.aspx");
CxList domsWithSqlDataSource = aspx.FindByShortName("SqlDataSource", false).FindByType(typeof(TypeRef));
CxList repeaters = aspx.FindByType(typeof(Declarator)).FindByType("Repeater");
HashSet<string> fileMap = new HashSet<string>();

//Save in the hash map all the files that contain SqlDataSource
foreach(CxList dom in domsWithSqlDataSource)
{
	CSharpGraph g = dom.data.GetByIndex(0) as CSharpGraph;
	if (g != null && g.LinePragma != null && g.LinePragma.FileName != null) 
	{
		fileMap.Add(g.LinePragma.FileName);
	}
}

//Only add Repeaters that are in files that contain SqlDataSource
foreach(CxList repeater in repeaters)
{
	CSharpGraph g = repeater.data.GetByIndex(0) as CSharpGraph;
	if (g != null && g.LinePragma != null && g.LinePragma.FileName != null) 
	{
		if(fileMap.Contains(g.LinePragma.FileName))
		{
			result.Add(repeater);
		}
	}
}