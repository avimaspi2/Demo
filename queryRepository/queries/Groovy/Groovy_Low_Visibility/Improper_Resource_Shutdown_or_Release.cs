CxList methods = Find_Methods();
CxList returns = Find_ReturnStmt();
CxList cattr = All.FindByType(typeof(CustomAttribute));
CxList cathrows = cattr.FindByShortName("CxThrows");
CxList myMethods = cathrows.GetAncOfType(typeof(MethodDecl));
CxList objects = Find_Object_Create();
CxList allRefsInProj = All.FindByType(typeof(TypeRef));
CxList allConstructorDeclInProj = All.FindByType(typeof(ConstructorDecl));
CxList allStatementCollection = All.FindByType(typeof(StatementCollection));
CxList Trys = All.FindByType(typeof(TryCatchFinallyStmt));


List<string> relevantTypes = new List<string> {
		"AbstractInterruptibleChannel",
		"AbstractSelectableChannel",
		"AbstractSelector",
		"AsynchronousFileChannel",
		"AsynchronousServerSocketChannel",
		"AsynchronousSocketChannel",
		"AudioInputStream",
		"AudioInputStream",
		"AudioInputStream",
		"AudioInputStream",
		"BufferedInputStream",
		"BufferedInputStream",
		"BufferedInputStream",
		"BufferedOutputStream",
		"BufferedOutputStream",
		"BufferedOutputStream",
		"BufferedReader",
		"BufferedReader",
		"BufferedReader",
		"BufferedWriter",
		"BufferedWriter",
		"BufferedWriter",
		"CharArrayReader",
		"CharArrayReader",
		"CharArrayReader",
		"CharArrayReader",
		"CharArrayWriter",
		"CheckedInputStream",
		"CheckedOutputStream",
		"CipherInputStream",
		"CipherInputStream",
		"CipherInputStream",
		"CipherInputStream",
		"CipherOutputStream",
		"CipherOutputStream",
		"CipherOutputStream",
		"CipherOutputStream",
		"Connection",
		"Connection",
		"DataInputStream",
		"DataInputStream",
		"DataInputStream",
		"DataInputStream",
		"DataOutputStream",
		"DataOutputStream",
		"DataOutputStream",
		"DataOutputStream",
		"DatagramChannel",
		"DatagramSocket",
		"DatagramSocket",
		"DatagramSocket ",
		"DatagramSocket ",
		"DeflaterInputStream",
		"DeflaterInputStream",
		"DeflaterInputStream",
		"DeflaterInputStream",
		"DeflaterOutputStream",
		"DeflaterOutputStream",
		"DeflaterOutputStream",
		"DeflaterOutputStream",
		"DigestInputStream",
		"DigestOutputStream",
		"FileCacheImageInputStream",
		"FileCacheImageOutputStream",
		"FileChannel",
		"FileImageInputStream",
		"FileImageOutputStream",
		"FileInputStream",
		"FileInputStream",
		"FileInputStream",
		"FileInputStream",
		"FileLock",
		"FileOutputStream",
		"FileOutputStream",
		"FileOutputStream",
		"FileOutputStream",
		"FileReader",
		"FileReader",
		"FileReader",
		"FileReader",
		"FileSystem",
		"FileWriter",
		"FileWriter",
		"FileWriter",
		"FileWriter",
		"FilterInputStream",
		"FilterInputStream",
		"FilterInputStream",
		"FilterInputStream",
		"FilterOutputStream",
		"FilterOutputStream",
		"FilterOutputStream",
		"FilterOutputStream",
		"FilterReader",
		"FilterWriter",
		"Formatter",
		"ForwardingJavaFileManager",
		"GZIPInputStream",
		"GZIPOutputStream",
		"ImageInputStreamImpl",
		"ImageOutputStreamImpl",
		"InflaterInputStream",
		"InflaterInputStream",
		"InflaterInputStream",
		"InflaterInputStream",
		"InflaterOutputStream",
		"InflaterOutputStream",
		"InflaterOutputStream",
		"InflaterOutputStream",
		"InputStream",
		"InputStreamLogger",
		"InputStreamReader",
		"InputStreamReader",
		"InputStreamReader",
		"InputStreamReader",
		"JarFile",
		"JarInputStream",
		"JarOutputStream",
		"LineNumberInputStream",
		"LineNumberReader",
		"LineNumberReader",
		"LineNumberReader",
		"LineNumberReader",
		"LogStream",
		"MLet",
		"MemoryCacheImageInputStream",
		"MemoryCacheImageOutputStream",
		"MulticastSocket",
		"ObjectInputStream",
		"ObjectInputStream",
		"ObjectInputStream",
		"ObjectInputStream",
		"ObjectOutputStream",
		"ObjectOutputStream",
		"ObjectOutputStream",
		"ObjectOutputStream",
		"OutputStream",
		"OutputStream",
		"OutputStreamWriter",
		"OutputStreamWriter",
		"OutputStreamWriter",
		"OutputStreamWriter",
		"PipedInputStream",
		"PipedInputStream",
		"PipedInputStream",
		"PipedInputStream",
		"PipedOutputStream",
		"PipedOutputStream",
		"PipedOutputStream",
		"PipedOutputStream",
		"PipedReader",
		"PipedReader",
		"PipedReader",
		"PipedReader",
		"PipedWriter",
		"PipedWriter",
		"PipedWriter",
		"PipedWriter",
		"PrintStream",
		"PrintStream",
		"PrintStream",
		"PrintStream",
		"PrintWriter",
		"PrintWriter",
		"PrintWriter",
		"PrintWriter",
		"PrivateMLet",
		"ProgressMonitorInputStream",
		"ProgressMonitorInputStream",
		"ProgressMonitorInputStream",
		"ProgressMonitorInputStream",
		"PushbackInputStream",
		"PushbackInputStream",
		"PushbackInputStream",
		"PushbackInputStream",
		"PushbackReader",
		"PushbackReader",
		"PushbackReader",
		"PushbackReader",
		"RMIConnectionImpl",
		"RMIConnectionImpl_Stub",
		"RMIConnector",
		"RMIIIOPServerImpl",
		"RMIJRMPServerImpl",
		"RMIServerImpl",
		"RandomAccessFile",
		"Reader",
		"SSLServerSocket",
		"SSLSocket",
		"SSLSocket",
		"SSLSocket",
		"SSLSocket",
		"Scanner",
		"SelectableChannel",
		"Selector",
		"SequenceInputStream",
		"SequenceInputStream",
		"SequenceInputStream",
		"SequenceInputStream",
		"ServerSocket",
		"ServerSocket",
		"ServerSocket",
		"ServerSocket",
		"ServerSocketChannel",
		"SinkChannel",
		"Socket",
		"Socket",
		"Socket",
		"Socket",
		"SocketChannel",
		"SourceChannel",
		"StringBufferInputStream",
		"StringReader",
		"StringReader",
		"StringReader",
		"StringReader",
		"URLClassLoader",
		"Writer",
		"XMLDecoder",
		"XMLEncoder",
		"ZipFile",
		"ZipInputStream",
		"ZipOutputStream"};

// Get all classes inherits
CxList ClassInheritsFrom = All.NewCxList();
CxList ClassDeclList = All.FindByType(typeof(ClassDecl)); 

List<string> inheriting = new List<string>();
foreach( string c in relevantTypes){
	string className = ClassDeclList.InheritsFrom(c).GetName();
	if(!String.IsNullOrEmpty(className) && !relevantTypes.Contains(className)) 
		inheriting.Add(className);
}
relevantTypes.AddRange(inheriting);


CxList allResourcesInProj = (objects.FindByShortNames(relevantTypes));

CxList wrappingObjects = All.FindAllReferences(All.GetClass(allResourcesInProj.GetAncOfType(typeof(ConstructorDecl))))
	.GetFathers().FindByType(typeof(ObjectCreateExpr));

//methods where is opened resources
CxList auxOpenedMethods = All.FindAllReferences(myMethods.GetMethod(allResourcesInProj.FindByFathers(returns))).FindByType(typeof(MethodInvokeExpr));
allResourcesInProj.Add(auxOpenedMethods);
allResourcesInProj.Add(allResourcesInProj.GetByAncs(myMethods.GetMethod(allResourcesInProj.FindByFathers(returns))));

CxList allOpen = (methods.FindByShortNames(new List<string> {
		"getConnection", 
		"open",
		"getOutputStream",
		"getInputStream",
		"getWriter",
		"getReader",
		"getResourceAsStream",
		"openOutputStream",
		"openFileOutput",
		"makeConnection",
		"getErrorStream",
		"streamContent",
		"createInputStream"
		}));

CxList unAccountable = allOpen.FindByMemberAccess("HttpServletRequest.*");
unAccountable.Add(allOpen.FindByMemberAccess("HttpServletResponse.*"));
allOpen -= unAccountable;

allResourcesInProj.Add(allOpen);

CxList mimeUtility = methods.FindByMemberAccess("MimeUtility.encode");
mimeUtility.Add(methods.FindByMemberAccess("MimeUtility.decode"));
allResourcesInProj.Add(mimeUtility);

CxList sesionConnection = methods.FindByMemberAccess("Session.connection");
allResourcesInProj.Add(sesionConnection);

CxList urlStream = methods.FindByMemberAccess("URL.openStream");
allResourcesInProj.Add(urlStream);
CxList transtaction = methods.FindByMemberAccess("Transaction.begin");
allResourcesInProj.Add(transtaction);

CxList returningConnection = All.FindAllReferences(allOpen.GetAssignee()).FindByFathers(returns);
returningConnection.Add(allOpen.FindByFathers(returns));

allResourcesInProj.Add(All.FindAllReferences(myMethods.GetMethod(returningConnection)).FindByType(typeof(MethodInvokeExpr)));
// avoid encapsulating objects (considering the resource is closed by the wrapping object)
allResourcesInProj -= allResourcesInProj.GetParameters(allResourcesInProj.FindByParameters(allResourcesInProj));


CxList ThrowingReferences = All.FindAllReferences(allResourcesInProj.GetAssignee());
ThrowingReferences.Add(All.FindAllReferences(All.FindByType(typeof(TypeRef)).FindByFathers(All.FindByType(typeof(CastExpr)))
	.FindByTypes(relevantTypes.ToArray()).GetFathers().GetAssignee()));

ThrowingReferences.Add(ThrowingReferences.GetMembersOfTarget());

allResourcesInProj = allResourcesInProj.DataInfluencingOn(ThrowingReferences);

CxList TryEnds = allResourcesInProj.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

CxList TryBlocks = All.NewCxList();
foreach(CxList tryCatch in Trys){
	try{
		TryCatchFinallyStmt tryGraph = tryCatch.TryGetCSharpGraph<TryCatchFinallyStmt>();
		if(tryGraph.Try != null){
			
			TryBlocks.Add(tryGraph.Try.NodeId, tryGraph.Try);
		}		
	}catch(Exception ex){
		
	}
}
CxList nodeObjectDeclarator = allResourcesInProj.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
nodeObjectDeclarator.Add(wrappingObjects);

CxList TryBlock = nodeObjectDeclarator.GetByAncs(TryBlocks);

CxList allClose = methods.FindByShortName("close");

allClose -= Find_Dead_Code();
//search for all invocations of methods which close the resource
CxList auxClosedMethods = All.FindAllReferences(myMethods.GetMethod(allClose)).FindByType(typeof(MethodInvokeExpr));

CxList closes = TryEnds * allClose;
//methods close transaction from Apache Transaction 
closes.Add(methods.FindByMemberAccess("Transaction.safeRollback"));
closes.Add(methods.FindByMemberAccess("Transaction.rollback"));
closes.Add(methods.FindByMemberAccess("Transaction.commit"));

CxList closedResources = All.NewCxList();
foreach(CxList myTry in TryBlock){
	CxList curFinally = allStatementCollection.GetFinallyClause(myTry.GetAncOfType(typeof(TryCatchFinallyStmt)));
	try{
		//we test if we have a try statement 
		if(curFinally.TryGetCSharpGraph<StatementCollection>().Count > 0){
			// remove resource that close on the finally block
			CxList myList = All.FindAllReferences(closes.GetByAncs(curFinally).GetTargetOfMembers()).GetAssigner();
			myList.Add(All.FindByFathers(myList.FindByType(typeof(CastExpr))));
			closedResources.Add(myList.GetByAncs(myTry));
		
			// remove resources that are wrapped and are closed along recurring to auxiliar method
			CxList openedResources = All.FindByFathers(All.FindAllReferences(auxClosedMethods.GetByAncs(curFinally).GetTargetOfMembers()).GetByAncs(TryBlocks).GetAncOfType(typeof(AssignExpr))).GetAssignee();
			CxList resourcesWrapperClass = All.FindDefinition(allRefsInProj.FindByFathers(openedResources.GetAssigner().FindByType(typeof(ObjectCreateExpr))));
			CxList ResourcesAllocatedOnWrapperConstructor = allResourcesInProj.GetByAncs(allConstructorDeclInProj.GetByAncs(resourcesWrapperClass));
			closedResources.Add(ResourcesAllocatedOnWrapperConstructor);
			CxList resource_closer = All.GetParameters(auxClosedMethods).GetByAncs(curFinally);
			closedResources.Add(All.FindAllReferences(resource_closer).FindByAssignmentSide(CxList.AssignmentSide.Left).GetAssigner());
		
			// remove resources that are wrapped, opened through a method and are close along recurring to auxiliar method 
			CxList methodsReturningResources = All.FindDefinition(openedResources.GetAssigner().FindByType(typeof(MethodInvokeExpr)));
			closedResources.Add(allResourcesInProj.GetByAncs(allResourcesInProj.GetByAncs(methodsReturningResources)));
		
		}
	}catch(Exception ex){
		
	}
}
/*
remove all resources that are closed at some "finally" even if not located under try.
*/
CxList allFinallies = allStatementCollection.GetFinallyClause(All.FindByType(typeof(TryCatchFinallyStmt)));
CxList closable = methods.FindByShortName("close").GetByAncs(allFinallies);
closedResources.Add(closable);
CxList resourceReference = closable.GetTargetOfMembers();
closedResources.Add(All.FindAllReferences(resourceReference));

CxList TryBlockMethods = TryBlock.FindByType(typeof(MethodInvokeExpr)) * auxOpenedMethods;

CxList mDefinitions = All.FindDefinition(TryBlockMethods);
//list with the resource open in auxiliar methods
//this methods are invoke inside a try block (mDefinitions)
CxList auxResourcesOpen = All.GetByAncs(mDefinitions).FindByType(typeof(ObjectCreateExpr));
auxResourcesOpen.Add(closedResources);

CxList tempResult = allResourcesInProj - auxResourcesOpen;

// remove methods wrapping unAccountable
CxList paramsOftempResult = All.GetParameters(tempResult);
CxList paramOfClosedStr = paramsOftempResult * unAccountable;
paramOfClosedStr.Add(unAccountable.DataInfluencingOn(paramsOftempResult));
CxList wrapperOFunAccountable = tempResult.FindByParameters(paramOfClosedStr);
tempResult -= wrapperOFunAccountable; 	
	
List<string> autocloseable = new List<string> {"BufferedReader","BufferedWriter", "BufferedInputStream", "BufferedOutputStream", "ResponseEntity"};
// Get all classes inherits from autocloseable
CxList classNames = ClassDeclList.InheritsFrom("AutoCloseable");
foreach(CxList c in classNames)
{
	string clsName = c.GetName();
	autocloseable.Add(clsName);
}

CxList paramOfBuffered = All.GetParameters(All.FindByShortNames(autocloseable));
CxList closedByBuffered = tempResult.InfluencingOn(paramOfBuffered) + paramOfBuffered; 
tempResult -= closedByBuffered;

CxList addResult = All.NewCxList();

foreach(CxList res in tempResult.GetCxListByPath())
{
	CxList allNodes = res.GetStartAndEndNodes(CxList.GetStartEndNodesType.AllNodes);
	if((allNodes * closedResources).Count == 0)
	{
		addResult.Add(res);
	}
}


Dictionary<int,Tuple<int, CxList>> validFlows = new Dictionary<int, Tuple<int, CxList>>();
foreach (CxList res in addResult.GetCxListByPath()){
	try{
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
	}catch(Exception ex){
		
	}
}

foreach(KeyValuePair<int,Tuple<int,CxList>> elem in validFlows){
	result.Add(elem.Value.Item2);
}