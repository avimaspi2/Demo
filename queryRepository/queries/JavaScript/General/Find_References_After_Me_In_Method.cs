//this query looks for all references of a given CxList that are located below given element (in it's method scope)

if(param.Length > 0)
{
	CxList current = param[0] as CxList;
	CxList allRefs = All.FindAllReferences(current);
	CxList definition = All.FindDefinition(current);
	CxList definitionField = definition.GetAncOfType(typeof(FieldDecl));
	CxList hasDefinitionAsField = allRefs.GetByAncs(definitionField);

	foreach(CxList c in current)
	{		
		CxList refsOfCurrent = allRefs.FindAllReferences(c);
		CxList field = hasDefinitionAsField * refsOfCurrent;
		if(field.Count > 0)
		{
			refsOfCurrent = refsOfCurrent.GetByAncs(c.GetAncOfType(typeof(MethodDecl)));
		}
		CSharpGraph cGraph = c.GetFirstGraph();
		if(cGraph != null && cGraph.LinePragma != null)
		{
			try
			{
				int currentLine = cGraph.LinePragma.Line;
				int currentCol = cGraph.LinePragma.Column;
				int fileId = cGraph.LinePragma.GetFileId();
				refsOfCurrent = refsOfCurrent.FindByFileId(fileId);
				foreach(KeyValuePair<int,IGraph> r in refsOfCurrent.data)
				{
					CSharpGraph rGraph = r.Value as CSharpGraph;
					if(rGraph != null && rGraph.LinePragma != null)
					{
						try
						{
							int refLine = rGraph.LinePragma.Line;
							int refCol = rGraph.LinePragma.Column;
							if(refLine > currentLine)
							{
								result.Add(All.FindById(rGraph.NodeId));
							}
							if(refLine == currentLine && refCol > currentCol)
							{
								result.Add(All.FindById(rGraph.NodeId));
							}
							if(refLine == currentLine && refCol == currentCol)
							{
								if(rGraph.NodeId > cGraph.NodeId)
								{
									result.Add(All.FindById(rGraph.NodeId));
								}
							}
						}catch(Exception e)
						{
							cxLog.WriteDebugMessage(e);
						}
					}
					
				}
			}
			catch(Exception e)
			{
				cxLog.WriteDebugMessage(e);
			}
		}
		
	}
}