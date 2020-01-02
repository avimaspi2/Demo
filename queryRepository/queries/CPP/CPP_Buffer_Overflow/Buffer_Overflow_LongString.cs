// The purpose of the query is to find long string buffer overflow
// For example:
//              char xxx[5];
//				*xxx = "FFFFFFFFFFFFFFFFFFFF";
// 

//stack overflow:
CxList stringLiteral = Find_Strings();
CxList integers=Find_Integer_Literals();
CxList ur = Find_UnknownReference();
CxList declarators = Find_Declarators();
CxList arraryInitializer = Find_ArrayInitializer();
CxList charDecls = declarators.FindByType("char");
CxList charPointerDecl = charDecls.FindByPointerType("char");
CxList rankSpecifier = All.FindByType(typeof(RankSpecifier));
CxList variableDeclarations = declarators.GetAncOfType(typeof(VariableDeclStmt));
CxList type = rankSpecifier.FindByFathers(Find_TypeRef().FindByType("char").GetByAncs(variableDeclarations));
CxList charArrays = declarators.GetByAncs(type.GetAncOfType(typeof(VariableDeclStmt)));  
CxList variablesOfTypeCharPointer = ur.FindAllReferences(charArrays + charPointerDecl);

CxList sanitizers = Find_Sanitize();

CxList relevant = variablesOfTypeCharPointer.FindByFathers(All.FindByAssignmentSide(CxList.AssignmentSide.Left));
relevant.Add(variablesOfTypeCharPointer.FindByAssignmentSide(CxList.AssignmentSide.Left));
relevant.Add(variablesOfTypeCharPointer.GetByAncs(Find_VariableDeclStmt()));
relevant -= relevant.FindByFathers(relevant.GetFathers().FindByType(typeof(IndexerRef)));

CxList flow = relevant.InfluencedByAndNotSanitized(stringLiteral, sanitizers);

CxList arrayCreateExpr = Find_ArrayCreateExpr();

CxList valueAccess = variablesOfTypeCharPointer.FindByFathers(variablesOfTypeCharPointer.GetFathers().FindByShortName("Pointer"));

foreach(CxList elementInFlow in flow.GetCxListByPath())
{

	CxList sink = elementInFlow.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	CxList declOfSink = charDecls.FindDefinition(sink);
	CxList stringInFault = elementInFlow.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
	int stringSize = stringInFault.GetName().Length;

	CxList arrayCreation = arrayCreateExpr.GetByAncs(declOfSink);
	foreach(CxList arr in arrayCreation)
	{
		try{
			ArrayCreateExpr r = arr.TryGetCSharpGraph<ArrayCreateExpr>();
			//scenario #1 declarator of type char x[size]
			if (r.Sizes == null)
			{
				continue;
			}
			if(r.Sizes.Count > 0)
			{
				ExpressionCollection sizes = r.Sizes;
				CxList firstSize = All.NewCxList();
				foreach(Expression size in sizes)
				{
					firstSize.Add(integers.GetByAncs(All.FindById(size.NodeId)));
				}
				string arraySizeAsString = firstSize.GetName();
				int arraySize = -1;
				if(Int32.TryParse(arraySizeAsString, out arraySize) &&
					arraySize != -1 && stringSize > arraySize - 1)
				{
					result.Add(elementInFlow);
				}
			}
			else
			{	//scenario #2 declarator of type char x[]={'d','s',...}	
				ArrayInitializer aiGraph = arraryInitializer.GetByAncs(arr).TryGetCSharpGraph<ArrayInitializer>();			
				if(aiGraph != null && aiGraph.InitialValues != null)
				{
					bool valid = true;
					ExpressionCollection iv = aiGraph.InitialValues;
					foreach(Expression e in iv)
					{
						if(All.FindById(e.NodeId).FindByType(typeof(StringLiteral)).Count > 0)
						{
							valid = false;
						}										
					}
					if(!valid)
					{
						continue;
					}
					int initialValuesLength = aiGraph.InitialValues.Count;
					if(stringSize > initialValuesLength)
					{
						result.Add(elementInFlow);
					}
				}
			}	
		}
		catch(Exception e)
		{
			cxLog.WriteDebugMessage(e);
		}
	}
	// scenario 3: char label[]="ffff"; and 4: char * z="fff"; char *x="dddd"; *z=*x;
	CxList sinkIsValueAccess = valueAccess * sink;
	if(sinkIsValueAccess.Count > 0)
	{
		CxList pointerDefinition = declarators.FindDefinition(sinkIsValueAccess);
		CxList stringCreation = stringLiteral.FindByFathers(pointerDefinition);
		if(stringCreation.GetName().Length > 0)
		{
			if(stringSize > stringCreation.GetName().Length)		
			{
				result.Add(elementInFlow);
			}
		}
	}
}