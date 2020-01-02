//Find_WebServices_Input
CxList methods = Find_MethodDeclaration();
CxList allParams = base.Find_ParamDecl();
CxList allClassesInterfaces = Find_Class_Decl(); 
allClassesInterfaces.Add(Find_Interfaces());

//JAX-WS
//Find all @WebService and @WebMethod parameters
CxList webServices = All.FindByCustomAttribute("WebService").GetFathers();	//find all @WebService annotations classes
CxList webMethods = All.FindByCustomAttribute("WebMethod").GetFathers();	//find all "published" (@WebService or @WebMethod) methods
webMethods.Add(methods.GetByAncs(webServices));

CxList webServClassInerface = webMethods.GetAncOfType(typeof(InterfaceDecl));
webServClassInerface.Add(webMethods.GetAncOfType(typeof(ClassDecl)));			//get WebServices/WebMethods class/interface

CxList inheritWebServ = allClassesInterfaces.InheritsFrom(webServClassInerface);
CxList methodsInheritClass = methods.GetByAncs(inheritWebServ);
CxList allWebMethods = All.NewCxList();
allWebMethods.Add(webMethods);

foreach(CxList curMethod in webMethods)
{
	CSharpGraph graph = curMethod.TryGetCSharpGraph<CSharpGraph>();
	CxList methodWithSameName = methodsInheritClass.FindByShortName(graph.ShortName);
	if(methodWithSameName.Count > 0)
	{
//		allWebMethods.Add(methodsInheritClass.FindByShortName(graph.ShortName));
		allWebMethods.Add(methodWithSameName);
	}
	
}

CxList JAXWSParams = allParams.GetByAncs(allWebMethods);	//Parameters of @WebService and @WebMethod methods

//JAX-RS
//Find all @PathParam and @QueryParam parameters
CxList pathQueryParam = All.FindByCustomAttribute("PathParam").GetFathers(); 
pathQueryParam.Add(All.FindByCustomAttribute("QueryParam").GetFathers());

//Find all @GET, @POST, @PUT, @DELETE and @HEAD parameters
CxList httpMethods = All.NewCxList();
httpMethods.Add(All.FindByCustomAttribute("GET").GetFathers());
httpMethods.Add(All.FindByCustomAttribute("POST").GetFathers());
httpMethods.Add(All.FindByCustomAttribute("PUT").GetFathers());
httpMethods.Add(All.FindByCustomAttribute("DELETE").GetFathers());
httpMethods.Add(All.FindByCustomAttribute("HEAD").GetFathers());

CxList httpMethodsParams = allParams.GetByAncs(httpMethods);

//Find @Provider -> readForm method -> 6-th parameter (last parameter is an inputstream)
CxList providerReadFormParam = All.FindByCustomAttribute("Provider").GetFathers();
providerReadFormParam = methods.GetByAncs(providerReadFormParam).FindByShortName("readFrom");
providerReadFormParam = allParams.GetParameters(providerReadFormParam, 5);

CxList JAXRSParams = All.NewCxList();
JAXRSParams.Add(pathQueryParam);
JAXRSParams.Add(httpMethodsParams);
JAXRSParams.Add(providerReadFormParam);

//JAX-RPC
//Find all method parameters of classes that inherits from Remote (java.rmi.Remote)
CxList inheritsRemote = All.InheritsFrom("Remote");
CxList classinheritsRemote =  inheritsRemote.FindByType(typeof (ClassDecl));
CxList remoteMethods = methods.GetByAncs(classinheritsRemote);
CxList JAXRPCParams = allParams.GetByAncs(remoteMethods);

result =  JAXRSParams;
result.Add(JAXWSParams);
result.Add(JAXRPCParams);