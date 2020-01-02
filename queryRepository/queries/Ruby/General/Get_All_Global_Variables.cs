CxList allGlobals = All.GetByAncs(All.FindByType(typeof(AssignExpr))).FindByShortName("$*").FindByAssignmentSide(CxList.AssignmentSide.Left);
allGlobals -= allGlobals.FindByShortName("$");

// remove double occurence of globals
Hashtable allNames = new Hashtable();
foreach (CxList gl in allGlobals)
{
	CSharpGraph g = gl.data.GetByIndex(0) as CSharpGraph;
	string name = g.ShortName;
	if (!allNames.ContainsKey(name))
	{
		allNames.Add(name, name);
		result.Add(gl);
	}
}