//Find_WebServices_Input
CxList methods = Find_MethodDeclaration();
CxList customAttributes = Find_CustomAttribute();
CxList classDecl = Find_Class_Decl();
CxList allClassesInterfaces = All.FindByType(typeof(InterfaceDecl));
allClassesInterfaces.Add(classDecl);
CxList allParams = All.FindByType(typeof (ParamDecl));


//JAX-WS
//Find all @WebService and @WebMethod parameters
CxList webServices = All.FindByCustomAttribute("WebService").GetFathers(); //find all @WebService annotations classes
CxList webMethods = All.FindByCustomAttribute("WebMethod").GetFathers(); //find all "published" (@WebMethod) methods
webMethods.Add(methods.GetByAncs(webServices)); //find all "published" (@WebService) methods
CxList webServClassInerface = webMethods.GetAncOfType(typeof(InterfaceDecl)); //get WebServices/WebMethods interfaces
webServClassInerface.Add(webMethods.GetAncOfType(typeof(ClassDecl))); //get WebServices/WebMethods classes

CxList inheritWebServ = allClassesInterfaces.InheritsFrom(webServClassInerface);
CxList methodsInheritClass = methods.GetByAncs(inheritWebServ);
CxList allWebMethods = All.NewCxList();
allWebMethods.Add(webMethods);

foreach(CxList curMethod in webMethods)
{
	CSharpGraph graph = curMethod.data.GetByIndex(0) as CSharpGraph;
	CxList methodWithSameName = methodsInheritClass.FindByShortName(graph.ShortName);
	if(methodWithSameName.Count > 0)
	{
		allWebMethods.Add(methodWithSameName);
	}
	
}

CxList JAXWSParams = allParams.GetByAncs(allWebMethods); //Parameters of @WebService and @WebMethod methods

//JAX-RS
//Find all @PathParam and @QueryParam parameters
CxList pathQueryParam = customAttributes.FindByShortNames(new List<String> { "PathParam", "QueryParam" }).GetFathers();

//Find all @GET, @POST, @PUT, @DELETE and @HEAD parameters
CxList httpMethodsParams = allParams.GetByAncs(Find_WebServices_Methods());

//Find @Provider -> readForm method -> 6-th parameter (last parameter is an inputstream)
CxList providerReadFormParam = customAttributes.FindByShortName("Provider").GetFathers();
providerReadFormParam = methods.GetByAncs(providerReadFormParam).FindByShortName("readFrom");
providerReadFormParam = allParams.GetParameters(providerReadFormParam, 5);

CxList JAXRSParams = pathQueryParam;
JAXRSParams.Add(httpMethodsParams);
JAXRSParams.Add(providerReadFormParam);

//JAX-RPC
//Find all method parameters of classes that inherits from Remote (java.rmi.Remote)
CxList inheritsRemote = All.InheritsFrom("Remote");
CxList classinheritsRemote = inheritsRemote * classDecl;
CxList remoteMethods = methods.GetByAncs(classinheritsRemote);
CxList JAXRPCParams = allParams.GetByAncs(remoteMethods);

result = JAXRSParams;
result.Add(JAXWSParams);
result.Add(JAXRPCParams);