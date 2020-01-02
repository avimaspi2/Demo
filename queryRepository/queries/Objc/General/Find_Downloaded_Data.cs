// Find_Downloaded_Data
// ----------------------------------------------
// This query finds all url strings that contain http scheme (http or https according to parameter), as well as all
// NSData objects that were retrieved from these http urls via connection delegates.
if (param.Length > 0) 
{
	CxList http = param[0] as CxList;
	CxList downloadData = All.NewCxList();
	CxList methods = Find_Methods();
	CxList connections = methods.FindByMemberAccess("NSURLConnection.connectionWithRequest:delegate:");
		
	List<string> connectionsMethodsNames = new List<string>() {	
			"initWithRequest:delegate:","initWithRequest:delegate:startImmediately:",
			"*WithContentsOfURL:*","*initWithContentsOfURL:*",
			//Swift	
			"init:request:delegate:","init:delegate:","init:request:delegate:startImmediately:",
			"init:delegate:startImmediately:"
			};
	
	connections.Add(methods.FindByShortNames(connectionsMethodsNames));

	connections.Add(methods.FindByShortName("NSURLConnection:delegate:").FindByParameterName("request", 0));
	connections.Add(methods.FindByShortName("NSURLConnection:delegate:startImmediately:").FindByParameterName("request", 0));	
	connections.Add(methods.FindByShortName("NSData:").FindByParameterName("contentsOf", 0));

	CxList classDecl = Find_Classes();
	CxList methodDecl = Find_MethodDecls();
	CxList declarator = Find_Declarators();
	CxList typeRef = Find_TypeRef();
	CxList connectionParamers0 = All.GetParameters(connections, 0);
	CxList connectionParamers1 = All.GetParameters(connections, 1);
	CxList paramDef = All.FindDefinition(connectionParamers1);
	
		
	List<string> allDidReceiveDataMethodsNames = new List<string>() {
			"connection:didReceiveData:","URLSession:dataTask:didReceiveData:",
			"urlSession:dataTask:didReceive:","urlSession:didReceive:"						
			};
	CxList allDidReceiveData = methodDecl.FindByShortNames(allDidReceiveDataMethodsNames);

		
	CxList didReceiveDataParams = All.GetParameters(allDidReceiveData, 1);
	didReceiveDataParams -= didReceiveDataParams.FindByType(typeof(Param));

	foreach (CxList connection in connections)
	{
		CxList pathPart1 = connectionParamers0.GetParameters(connection, 0).DataInfluencedBy(http);
		if (pathPart1.Count > 0)
		{
			CxList connectionDelegate = connectionParamers1.GetParameters(connection, 1);
			connectionDelegate -= connectionDelegate.FindByType(typeof(Param));
			bool isThis = (connectionDelegate.FindByType(typeof(ThisRef)).Count > 0);
			pathPart1 = pathPart1.ConcatenatePath(connectionDelegate);
			if (!isThis)
			{
				pathPart1 = pathPart1.ConcatenatePath(paramDef.FindDefinition(connectionDelegate));
			}

			// Find all the class declarators that are a reference of the delgate (self)
			CxList delegateClass = classDecl.FindAllReferences(connectionDelegate);

			// Find the case where the delegate is not for self, but of another class
			CxList delegateDelarator = declarator.FindAllReferences(connectionDelegate);
			CxList declarators = delegateDelarator.GetAncOfType(typeof(VariableDeclStmt));
			if (declarators.Count > 0)
			{
				CxList definition = All.FindByType(typeRef.GetByAncs(declarators));
				CxList classDef = classDecl.FindDefinition(definition);
				delegateClass.Add(classDef);
			}
			foreach (CxList cls in delegateClass)
			{
				if (!isThis)
				{
					pathPart1 = pathPart1.ConcatenatePath(cls);
				}
				CxList didReceiveData = allDidReceiveData.GetByAncs(cls);
				downloadData.Add(pathPart1.ConcatenatePath(didReceiveDataParams.GetParameters(didReceiveData, 1)));
			}		
		}
	}

	List<string> urlMethodsNames = new List<string>() {
			"*WithContentsOfURL:*","*initWithContentsOfURL:*"						
			};
	CxList urlMethods = methods.FindByShortNames(urlMethodsNames);	

	urlMethods.Add(methods.FindByShortName("NSString:encoding:*").FindByParameterName("contentsOf", 0));
	urlMethods.Add(methods.FindByShortName("NSString:encoding:*").FindByParameterName("contentsOfURL", 0));
	urlMethods.Add(methods.FindByShortName("NSString:encoding:*").FindByParameterName("format:", 0));
	urlMethods.Add(methods.FindByShortName("String:encoding:*").FindByParameterName("contentsOf:", 0));
	downloadData.Add(urlMethods);

	//for swift

	CxList swift_potential_connection_methods = All.NewCxList();
		
	List<string> swiftPotentialConnectionMethodsNames = new List<string>() {
			"NSString:encoding:*","NSString:usedEncoding:*","NSMutableString:encoding:*",
			"NSMutableString:usedEncoding:*","NSData:","NSData:options:*","NSMutableData:","NSMutableData:options:*",
			"NSArray:","NSMutableArray:","NSDictionary:","NSMutableDictionary:","NSMutableDictionary:",
			"dataTaskWithURL:","dataTaskWithURL:completionHandler:"
			};	
		
	CxList allConnectionsMethods = methods.FindByShortNames(swiftPotentialConnectionMethodsNames);
	swift_potential_connection_methods.Add(allConnectionsMethods);		
	swift_potential_connection_methods.Add(allConnectionsMethods.FindByParameterName("contentsOf", 0));
	swift_potential_connection_methods.Add(allConnectionsMethods.FindByParameterName("contentsOfURL", 0));
	swift_potential_connection_methods.Add(allConnectionsMethods.FindByParameterName("format:", 0));

	CxList relevant_http = Find_By_Type_And_Casting(http, "NSURL");
	relevant_http.Add(Find_By_Type_And_Casting(http, "URL"));
	CxList first_params = relevant_http.GetParameters(methods, 0);
	CxList swift_connection_methods = swift_potential_connection_methods * methods.FindByParameters(first_params);

	downloadData.Add(swift_connection_methods);

	result = downloadData;
}