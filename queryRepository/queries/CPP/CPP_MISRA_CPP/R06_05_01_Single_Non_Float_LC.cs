/*
MISRA CPP RULE 6-5-1
------------------------------
This query searches for 'for' loops that have more than one loop counter or that their loop counter is of type float

The Example below shows code with vulnerability: 
                
    
    for(float w=0.0f; w<100; w++); //non-compliant
*/



CxList allFors = All.FindByType(typeof(IterationStmt));

CxList helper = allFors;
foreach(CxList allf in allFors)
{
	IterationStmt i = allf.data.GetByIndex(0) as IterationStmt;
	if(i != null)
	{
		IterationType it = i.IterationType;
		if(!it.ToString().Equals("For"))
			helper -= allf;
	}
}
allFors = helper;

//CxList totalUnknownR = All.FindByType(typeof(UnknownReference)) + Find_All_Declarators() + All.FindByType(typeof(ParamDecl));
CxList unrf = All.FindByType(typeof(UnknownReference)).GetByAncs(allFors);
CxList totalDecl = Find_All_Declarators() + All.FindByType(typeof(FieldDecl)) + All.FindByType(typeof(ParamDecl));
CxList declarators = totalDecl.GetByAncs(allFors);
CxList leftSd = All.FindByAssignmentSide(CxList.AssignmentSide.Left);

CxList notRef = unrf.GetMembersOfTarget().GetTargetOfMembers();
unrf -= notRef;



//find float typedefs
CxList typedefs = All.FindByName("CX_TYPEDEF").FindByType(typeof(StringLiteral));
typedefs = typedefs.GetAncOfType(typeof(VariableDeclStmt))
	+ typedefs.GetAncOfType(typeof(FieldDecl));
CxList tpr = All.FindByType(typeof(TypeRef));
typedefs.Add(totalDecl.GetByAncs(typedefs) * All.FindByType(typeof(FieldDecl)));


CxList alternativeFloats = All.NewCxList();
foreach(CxList nf in typedefs)
{
	CSharpGraph g = nf.data.GetByIndex(0) as CSharpGraph;
	if(g != null)
	{
		if(g.TypeName == "float")
		{
			alternativeFloats.Add(tpr.FindByShortName(g.ShortName));
		}
	}
                
}

alternativeFloats = totalDecl.GetByAncs(alternativeFloats.GetFathers());






//find the init part of the for statement

foreach(CxList cur in allFors)
{
	CxList init = All.NewCxList();
	IterationStmt iterA = cur.data.GetByIndex(0) as IterationStmt;
	StatementCollection initColl = iterA.Init;
	for(int i = 0; initColl != null && i < initColl.Count; i++)
	{
		if(initColl[i] != null)
		{
			init.Add(All.FindById(initColl[i].NodeId));
		}
	}
	CxList unknownRef = unrf.GetByAncs(init);
	//retrieves the loop counters
	CxList loopCounter = unknownRef.FindByFathers(init.FindByType(typeof(ExprStmt)));

	CxList leftAsn = unknownRef * leftSd;
	loopCounter.Add(leftAsn + declarators.GetByAncs(init));

               
	CxList increment = All.NewCxList();
	StatementCollection incrementColl = iterA.Increment;
                
	for(int i = 0;  incrementColl != null && i < incrementColl.Count; i++)
	{              
		if(incrementColl[i] != null)
		{
			increment.Add(All.FindById(incrementColl[i].NodeId));
		}
	}
                
	CxList test = All.NewCxList();
	Expression testExp = iterA.Test;
	if(testExp != null)
	{
		test.Add(All.FindById(testExp.NodeId));
	}
                
	CxList testUn = unrf.GetByAncs(test);
	CxList incrUn = unrf.GetByAncs(increment);
	CxList additionalLC = incrUn.FindAllReferences(testUn) + testUn.FindAllReferences(incrUn);
	CxList temp = loopCounter.FindAllReferences(additionalLC);
	additionalLC -= additionalLC.FindAllReferences(temp);
                
	CxList potential = additionalLC * testUn;
	loopCounter.Add(potential);
                
                
	CxList nonCompliant = All.NewCxList();
	if(loopCounter.Count > 1)
	{
		nonCompliant.Add(loopCounter);
		result.Add(cur);
	}
	loopCounter -= nonCompliant;
	CxList lcDef = totalDecl.FindDefinition(loopCounter);
	CxList bad = (lcDef * alternativeFloats);
	if(bad.Count > 0)
	{
		result.Add(cur);
	}
	foreach(CxList t in lcDef)
	{
		CSharpGraph g = t.data.GetByIndex(0) as CSharpGraph;
		if(g != null)
		{
			if(g.TypeName.Equals("float"))
			{
				result.Add(cur);
			}
		}
	}
                
}