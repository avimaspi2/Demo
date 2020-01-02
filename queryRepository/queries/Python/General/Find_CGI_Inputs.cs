/*** This query finds inputs from cgi (getvalue method and .value member access)

Examples:

form = cgi.FieldStorage() 
first_name = form.getvalue('first_name')
last_name  = form['last_name'].value

***/
CxList cgi = Find_Methods_By_Import("cgi", new string[]{"FieldStorage"});
CxList cgiVars = cgi.GetAncOfType(typeof(AssignExpr));
cgiVars.Add(cgi.GetAncOfType(typeof(Declarator)));

CxList cgiFormGetvalues = All.NewCxList();

foreach(CxList v in cgiVars)
{
	try
	{
		string name = "";
		int id = 0;	
		CSharpGraph graphType = v.GetFirstGraph();
		if (graphType is AssignExpr)
		{
			CxList arr = All.FindByAssignmentSide(CxList.AssignmentSide.Left).GetByAncs(v);
			CSharpGraph graph = v.GetFirstGraph();
			CxList def = All.FindDefinition(arr);
			id = graph.NodeId;
			Declarator d = def.TryGetCSharpGraph<Declarator>();
			name = d.Name;
		}
		else if (graphType is Declarator)
		{
			Declarator d = v.TryGetCSharpGraph<Declarator>();
			CSharpGraph graph = v.GetFirstGraph();		
			id = graph.NodeId;
			name = d.Name;
		}
		CxList dataInfluenced = All.DataInfluencedBy(All.FindById(id));
		cgiFormGetvalues.Add(Find_Members(name + ".getvalue", dataInfluenced));
		cgiFormGetvalues.Add(Find_Members(name + ".value", dataInfluenced.FindByType(typeof(MemberAccess))));
	}
	catch (Exception e)
	{
		cxLog.WriteDebugMessage(e.Message);
	}
}

result = cgiFormGetvalues;