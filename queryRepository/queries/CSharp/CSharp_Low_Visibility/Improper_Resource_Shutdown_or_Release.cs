// Database Resources
List<string> dbResourceTypes = new List<string>()
	{
		"DbTransaction","DbConnection", "DbCommand", "DbCommand","DbDataAdapter","DbDataReader",
		"SqlTransaction", "SqlConnection", "SqlCommand","SqlDataAdapter", "SqlDataReader",
		"OleDbTransaction", "OleDbConnection", "OleDbCommand", "OleDbDataAdapter", "OleDbDataReader",
		"OracleTransaction","OracleConnection", "OracleCommand", "OracleDataAdapter", "OracleDataReader",
		};

// System.IO Resources
List<string> ioResourceTypes = new List<string>()
	{
		"TextReader","TextWriter",
		"StringReader","StringWriter",
		"StreamReader","StreamWriter",
		"BinaryReader","BinaryWriter",
		"XmlReader", "XmlWriter",
		"BufferedStream","FileStream",
		"XmlTextReader", "XmlTextWriter",
		"Graphics"
		};

// Static methods
List<string> ObjectStaticMethods = new List<string>()
	{
		"Create","OpenText","CreateText",
		"CreateCommand","ExecuteReader",
		"FromImage", "CreateGraphics",
		"FromHwnd"
		};


List<string> ResourceCloseMethods = new List<string>() {"Close","Dispose"};

List<string> ResourceTypeNames = new List<string>();
ResourceTypeNames.AddRange(dbResourceTypes);
ResourceTypeNames.AddRange(ioResourceTypes);
ResourceTypeNames.Add("IDisposable");

// General functions
CxList MethodInvoke = Find_Methods();
CxList MethodDecl = Find_MethodDecls();
CxList UsingStmts = All.FindByType(typeof(UsingStmt));
CxList ClassDeclList = Find_ClassDecl();
CxList CreateExpr = Find_ObjectCreations();
CxList TypeRef = All.FindByType(typeof(TypeRef));
CxList Trys = All.FindByType(typeof(TryCatchFinallyStmt));

// Get all classes inherits
CxList ClassInheritsFrom = All.NewCxList();
foreach( string c in ResourceTypeNames){
	ClassInheritsFrom.Add(ClassDeclList.InheritsFrom(c));
}

/* Collect all the instances of allocated resources */
CxList objectQueryInherits = All.NewCxList();
objectQueryInherits.Add(ClassInheritsFrom);

CxList typeOfObjectQuery = All.FindByTypes(ResourceTypeNames.ToArray());

typeOfObjectQuery.Add(TypeRef.FindByShortName(ClassInheritsFrom).GetFathers());
typeOfObjectQuery -= MethodDecl;
CxList variableDecl = typeOfObjectQuery.FindByType(typeof(VariableDeclStmt));
typeOfObjectQuery.Add(Find_Declarators().GetByAncs(variableDecl));
CxList RefTypeOfObjectQuery = All.FindAllReferences(typeOfObjectQuery);

CxList ResourceAllocationInstances = CreateExpr.FindByShortNames(ResourceTypeNames);
ResourceAllocationInstances.Add(CreateExpr * typeOfObjectQuery);
ResourceAllocationInstances.Add(RefTypeOfObjectQuery.FindByType(typeof(MethodInvokeExpr)));
CxList wrappingObjects = All.FindAllReferences(All.GetClass(ResourceAllocationInstances.GetAncOfType(typeof(ConstructorDecl))))
	.GetFathers().FindByType(typeof(ObjectCreateExpr));


/*
Case 1 : Example

	StreamReader reader = new StreamReader("lastupdate.txt");
	using (reader)
	{  
		reader.Use();
	}
*/
CxList NoDeclaresResourceInUsing = All.NewCxList();
foreach (CxList UsingStmt in UsingStmts)
{
	UsingStmt UsingGraph = UsingStmt.TryGetCSharpGraph<UsingStmt>();
	if (UsingGraph.DeclaresResource)
	{
		foreach (Declarator d in UsingGraph.Declaration.Declarators)
		{
			NoDeclaresResourceInUsing.Add(All.FindById(d.NodeId));	
		}
	}
	else
	{
		if(UsingGraph != null && UsingGraph.Target != null)
			NoDeclaresResourceInUsing.Add(All.FindDefinition(All.FindById(UsingGraph.Target.NodeId)));		
	}
}


/* Case 2 : Example: 

	Message m = ClassX.Create();
	using (var outStream = File.Create(filePath))
*/
CxList ResourcesAll = All.FindByTypes(ResourceTypeNames.ToArray());
CxList StaticMethods = MethodInvoke.FindByShortNames(ObjectStaticMethods);
CxList StaticMethodDefineInList = ResourcesAll * StaticMethods.GetAssignee();

CxList ObjAssigneeReferences = All.NewCxList();

if(StaticMethodDefineInList.Count > 0){
	ObjAssigneeReferences = All.FindAllReferences(ResourceAllocationInstances.GetAssignee());
	ObjAssigneeReferences.Add(All.FindAllReferences(StaticMethodDefineInList));	
	ObjAssigneeReferences -= All.FindAllReferences(StaticMethodDefineInList.GetByAncs(UsingStmts));
	ResourceAllocationInstances.Add(All.FindDefinition(StaticMethodDefineInList.FindByType(typeof(UnknownReference))));
	ResourceAllocationInstances.Add(StaticMethodDefineInList);	
}

ObjAssigneeReferences.Add(All.FindAllReferences(ResourceAllocationInstances.GetAssignee()));

/* Remove case 1 from case 2 */
ObjAssigneeReferences -= All.FindAllReferences(NoDeclaresResourceInUsing);


/* Include resources used in a chain call */
ObjAssigneeReferences.Add(ObjAssigneeReferences.GetMembersOfTarget());


/* Evaluate all the flows starting from the resource and ending on the same instance */
ResourceAllocationInstances = ResourceAllocationInstances.DataInfluencingOn(ObjAssigneeReferences);

/* Collect the Try statements Block */
CxList TryEnds = ResourceAllocationInstances.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
CxList TryBlocks = All.NewCxList();
foreach(CxList tryCatch in Trys){
	TryCatchFinallyStmt tryGraph = tryCatch.TryGetCSharpGraph<TryCatchFinallyStmt>();
	if(tryGraph != null && tryGraph.Try != null)
		TryBlocks.Add(tryGraph.Try.NodeId, tryGraph.Try);
}

CxList TryBlock = (ResourceAllocationInstances.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly) + wrappingObjects).GetByAncs(TryBlocks);

/* Collect Methods that close resources passed through parameter */
CxList allCloseMethod = MethodInvoke.FindByShortNames(ResourceCloseMethods);
CxList RefMethodClosesInvoke = All.FindAllReferences(MethodDecl.GetMethod(allCloseMethod)).FindByType(typeof(MethodInvokeExpr));
CxList closes = TryEnds * allCloseMethod;

/*
  Exclude resources opened in a try block and properly closed on the finally block
 */
CxList closedResources = All.NewCxList();
foreach(CxList myTry in TryBlock){
	CxList curFinally = All.GetFinallyClause(myTry.GetAncOfType(typeof(TryCatchFinallyStmt)));
	
	//we test id we the statement try have a finally
	if(curFinally.TryGetCSharpGraph<StatementCollection>().Count > 0){
		// remove resource that close on the finally block
		closedResources.Add(All.FindAllReferences(closes.GetByAncs(curFinally).GetTargetOfMembers()).GetAssigner().GetByAncs(myTry));
		
		// remove resources that are wrapped and are close along recurring to auxiliar method
		CxList openedResources = All.FindAllReferences(RefMethodClosesInvoke.GetByAncs(curFinally).GetTargetOfMembers()).GetByAncs(TryBlocks).FindByAssignmentSide(CxList.AssignmentSide.Left);
		CxList resourcesWrapperClass = All.FindDefinition(All.FindByType(typeof(TypeRef)).FindByFathers(openedResources.GetAssigner().FindByType(typeof(ObjectCreateExpr))));
		CxList ResourcesAllocatedOnWrapperConstructor = ResourceAllocationInstances.GetByAncs(All.FindByType(typeof(ConstructorDecl)).GetByAncs(resourcesWrapperClass));
		closedResources.Add(ResourcesAllocatedOnWrapperConstructor);
		
		// remove resources that are wrapped, opened through a method and are close along recurring to auxiliar method 
		CxList methodsReturningResources = All.FindDefinition(openedResources.GetAssigner().FindByType(typeof(MethodInvokeExpr)));
		closedResources.Add(ResourceAllocationInstances.GetByAncs(ResourceAllocationInstances.GetByAncs(methodsReturningResources)));
	}
}

/* Exclude resources opened through the Using Statement */
closedResources.Add(Find_Using_Declarators().GetAssigner());
CxList tempResult = ResourceAllocationInstances - closedResources;


/*
  Keep only the longest flow starting from the resource
  It is considered the first node of the flow, the place where the resource is allocated
 */
Dictionary<int,Tuple<int, CxList>> validFlows = new Dictionary<int, Tuple<int, CxList>>();
foreach (CxList res in tempResult.GetCxListByPath()){
	CSharpGraph init = res.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly).GetFirstGraph();
	CxList allNodes = res.GetStartAndEndNodes(CxList.GetStartEndNodesType.AllNodes);
	int numNodes = allNodes.Count;
	if(init != null){
		if(validFlows.ContainsKey(init.NodeId)){
			Tuple<int,CxList> flowElem = validFlows[init.NodeId];
			if(numNodes > flowElem.Item1){
				validFlows[init.NodeId] = new Tuple<int,CxList>(numNodes, res.Clone());
			}
		}
		else{
			validFlows[init.NodeId] = new Tuple<int, CxList>(numNodes, res.Clone());
		}
	}
}

foreach(KeyValuePair<int,Tuple<int,CxList>> elem in validFlows){
	result.Add(elem.Value.Item2);
}