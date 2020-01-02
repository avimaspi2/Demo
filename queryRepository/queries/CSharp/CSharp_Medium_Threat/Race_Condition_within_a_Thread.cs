CxList thread = All.FindByType("Thread*");
CxList decls = All.FindByType(typeof(Declarator));

if(thread.Count > 0)
{
	//find the thread initiation point
	CxList delegatePassedToThread = All.GetParameters(thread);
	CxList methods = Find_Methods();
	delegatePassedToThread.Add(All.GetParameters(delegatePassedToThread.FindByType(typeof(ObjectCreateExpr))));
	delegatePassedToThread.Add(All.GetParameters(thread.GetMembersOfTarget(), 0));
	//handle direct lambda:
	delegatePassedToThread.Add(methods.GetByAncs(delegatePassedToThread.FindByType(typeof(LambdaExpr))));
	
	
	CxList threadMethod = All.FindDefinition(delegatePassedToThread);		
	//find a static collection
	CxList stat = All.FindByFieldAttributes(Modifiers.Static) - All.FindByFieldAttributes(Modifiers.Readonly);
	
	stat = decls.GetByAncs(stat.FindByType(typeof(FieldDecl)));	
	
	CxList safe = All.NewCxList();
	//remove synchronized collection
	safe.Add(stat.FindByType("SynchronizedCollection"));
	safe.Add(stat.FindByType("BlockingCollection"));
	safe.Add(stat.FindByType("Concurrent*"));
	safe.Add(All.FindAllReferences(stat).GetAssigner().FindByShortName("Synchronized").GetAssignee());	

	stat -= safe;

	
	//for collections- the relevant operation is Contains on collection
	CxList containsCheck = All.FindAllReferences(stat).GetMembersOfTarget().FindByShortName("Contains*");
	CxList collectionModification = methods.FindByShortName("Add") + methods.FindByShortName("Remove");
	collectionModification = collectionModification.GetTargetOfMembers().FindAllReferences(stat);	
	//for public static arrays we need to check if array is under condition, in case yes, we would like to
	//check the modification of this array.
	CxList arrayAccess = All.FindByType(typeof(IndexerRef));
	CxList publicStaticArray = stat * All.FindDefinition(arrayAccess).GetAncOfType(typeof(FieldDecl));
	arrayAccess = arrayAccess.FindAllReferences(publicStaticArray);
	
	CxList conditions = Find_Conditions();
	CxList check = arrayAccess.GetByAncs(conditions);
	CxList stmtColl = All.FindByType(typeof(StatementCollection));
	CxList leftOfAssign = All.FindByAssignmentSide(CxList.AssignmentSide.Left);
	CxList potentialRC = All.NewCxList();
	check.Add(containsCheck.GetTargetOfMembers());
	CxList modifiers = leftOfAssign + collectionModification;
	foreach(CxList indexerUnderCheck in check)
	{
		CxList parent = indexerUnderCheck.GetAncOfType(typeof(IfStmt)) + indexerUnderCheck.GetAncOfType(typeof(TernaryExpr)) +
			indexerUnderCheck.GetAncOfType(typeof(IterationStmt));	
		CxList valid = All.NewCxList();
	
		foreach(CxList pStmt in parent)
		{				
			CxList inStatement = conditions.FindByFathers(pStmt);
		
			if(check.GetByAncs(inStatement).Count > 0)
			{
				valid.Add(pStmt);
			}
		}
		potentialRC.Add(modifiers.FindAllReferences(indexerUnderCheck).GetByAncs(valid));		
		
	}
	// sanitizers:

	//surrounded by lock:
	potentialRC -= potentialRC.GetByAncs(potentialRC.GetAncOfType(typeof(LockStmt)));
	//inside a semaphore and mutex:
	CxList sanitized = Find_Code_Within_Mutex() * potentialRC;
	potentialRC -= sanitized;
	CxList runningGroup = Find_Relevant_Invocations_And_Refs();

	
	
	foreach(CxList marker in potentialRC)
	{
		CxList curMarker = marker.Clone();
		int counter = 0;
		while(counter < 6)
		{			
			CxList encapsulatingMethod = curMarker.GetAncOfType(typeof(MethodDecl));
			if((encapsulatingMethod * threadMethod).Count > 0)
			{
				result.Add(encapsulatingMethod.ConcatenateAllSources(marker));
			}else
			{
						
				curMarker = runningGroup.FindAllReferences(encapsulatingMethod);
				if(curMarker.Count == 0)
				{
					break;
				}
			}
			counter++;
		}
	}

}