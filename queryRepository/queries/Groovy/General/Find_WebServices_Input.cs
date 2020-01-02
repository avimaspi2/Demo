//Find_WebServices_Input
CxList methods = All.FindByType(typeof (MethodDecl));
CxList allParams = All.FindByType(typeof (ParamDecl));
CxList allClassesInterfaces = Find_Class_Decl() + All.FindByType(typeof(InterfaceDecl)); 

//JAX-WS
//Find all @WebService and @WebMethod parameters
CxList webServices = All.FindByCustomAttribute("WebService").GetFathers();	//find all @WebService annotations classes
CxList webMethods = All.FindByCustomAttribute("WebMethod").GetFathers() + methods.GetByAncs(webServices);	//find all "published" (@WebService or @WebMethod) methods
CxList webServClassInerface = webMethods.GetAncOfType(typeof(InterfaceDecl)) +
	webMethods.GetAncOfType(typeof(ClassDecl));			//get WebServices/WebMethods class/interface

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
		allWebMethods.Add(methodsInheritClass.FindByShortName(graph.ShortName));
	}
}
CxList JAXWSParams = allParams.GetByAncs(allWebMethods);	//Parameters of @WebService and @WebMethod methods

//JAX-RS
//Find all @PathParam and @QueryParam parameters
CxList pathQueryParam = All.FindByCustomAttribute("PathParam").GetFathers() 
	+ All.FindByCustomAttribute("QueryParam").GetFathers();

//Find all @GET, @POST, @PUT, @DELETE and @HEAD parameters
CxList httpMethods = All.FindByCustomAttribute("GET").GetFathers() + All.FindByCustomAttribute("POST").GetFathers() +
	All.FindByCustomAttribute("PUT").GetFathers() + All.FindByCustomAttribute("DELETE").GetFathers() +
	All.FindByCustomAttribute("HEAD").GetFathers();
CxList httpMethodsParams = allParams.GetByAncs(httpMethods);

//Find @Provider -> readForm method -> 6-th parameter (last parameter is an inputstream)
CxList providerReadFormParam = All.FindByCustomAttribute("Provider").GetFathers();
providerReadFormParam = methods.GetByAncs(providerReadFormParam).FindByShortName("readFrom");
providerReadFormParam = allParams.GetParameters(providerReadFormParam, 5);

CxList JAXRSParams = pathQueryParam + httpMethodsParams + providerReadFormParam;

//JAX-RPC
//Find all method parameters of classes that inherits from Remote (java.rmi.Remote)
CxList inheritsRemote = All.InheritsFrom("Remote");
CxList classinheritsRemote = inheritsRemote.FindByType(typeof (ClassDecl));
CxList remoteMethods = methods.GetByAncs(classinheritsRemote);
CxList JAXRPCParams = allParams.GetByAncs(remoteMethods);

result = JAXRSParams + JAXWSParams + JAXRPCParams;