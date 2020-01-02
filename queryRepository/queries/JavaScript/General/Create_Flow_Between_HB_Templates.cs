CxList hbPresent = All.FindByMemberAccess("Handlebars.compile");
if(hbPresent.Count != 0 )
{
	CxList template = All.FindByShortName("*template*", false);	
	CxList invokes = Find_Methods();
	CxList templateInvoke = template * invokes;
	CxList hbInvokesOnly = All.NewCxList();
	CxList compile = invokes.FindByMemberAccess("Handlebars.compile");
	foreach(KeyValuePair<int,IGraph> tInvokes in templateInvoke.data)
	{
		CSharpGraph g = tInvokes.Value as CSharpGraph;
		if(g != null && g.LinePragma != null && g.NodeId != null)
		{
			CxList allInFile = All.FindByFileId(g.LinePragma.GetFileId());
			if((compile * allInFile).Count > 0)			
			{
				hbInvokesOnly.Add(All.FindById(g.NodeId));
			}
		}
	}
	
	CxList referencesDecls = Find_UnknownReference();
	referencesDecls.Add(Find_Declarators());
	
	CxList stringL = Find_String_Literal();
	CxList ByIdOrClassName = stringL.FindByShortNames(new List<string>{"#*",".*"});	
	
	CxList flowToTemplate = (template * referencesDecls).DataInfluencedBy(ByIdOrClassName);

	foreach(CxList flow in flowToTemplate.GetCxListByPath())
	{
		CxList relevantTemplate = flow.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);	
		foreach(CxList rt in relevantTemplate)
		{
		
			CSharpGraph g = rt.GetFirstGraph();
			if(g == null || g.LinePragma == null)
			{
				continue;
			}
			int fileId = g.LinePragma.GetFileId();
			CxList found = hbInvokesOnly.FindByShortName(rt).FindByFileId(fileId);
			CxList hasKnownDefinition = found.FindAllReferences(rt);
			
			CxList toMap = All.NewCxList();
			
			if(hasKnownDefinition.Count > 0)
			{
				toMap.Add(hasKnownDefinition);
			}else
			{
				toMap.Add(found);
			}
			
			foreach(KeyValuePair<int,IGraph> templ in rt.data)
			{
				CSharpGraph t = templ.Value as CSharpGraph;	
				foreach(KeyValuePair<int,IGraph> fnd in toMap.data)		
				{			
					CSharpGraph f = fnd.Value as CSharpGraph;			
					if(t.LinePragma.Line < f.LinePragma.Line)
					{
						CustomFlows.AddFlow(All.FindById(t.NodeId), All.FindById(f.NodeId));
					}
				}
			
			}
		}
	}
}