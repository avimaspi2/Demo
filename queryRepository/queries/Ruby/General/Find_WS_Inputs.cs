CxList MethodDecls = All.FindByType(typeof(MethodDecl));

// Find all custom attributes
CxList custAttr = All.FindByType(typeof(CustomAttribute));

CxList WebAttr = custAttr.FindByName("*WebMethod"); 		// Web Services
CxList WcfAttr = custAttr.FindByName("*OperationContract");	// WCF

CxList inMethod1 = (WcfAttr + WebAttr).GetFathers().GetFathers().FindByType(typeof(MethodDecl));

// Methods of interface class 
CxList interfaceMethod = WcfAttr.GetFathers().GetFathers().FindByType(typeof(MethodDecl));
CxList interfaceClass = All.GetClass(interfaceMethod);
CxList InheritsFromInterface = All.InheritsFrom(interfaceClass);

// All Methods of implementation class Inherits From Interface
CxList MethodsOfImpl = All.GetByAncs(InheritsFromInterface) * MethodDecls;
CxList inMethod2 = MethodsOfImpl;	 

// Find all methods with name from CxList - CxList FindMembersByName(CxList cml)
CxList mList = All.NewCxList();
foreach (CxList cml in interfaceMethod)
{
	MethodDecl m = cml.data.GetByIndex(0) as MethodDecl;
	string nodeName = m.Name;
	bool CaseSensitive = true;
	mList.Add(MethodDecls.FindByShortName(nodeName, CaseSensitive));
}

inMethod2 = inMethod2 * mList;	// Only methods that implementat interface method


// Methods Of Interface class only
CxList inClass = All.FindByType(typeof(InterfaceDecl));
CxList MethodsOfInterface = All.GetByAncs(inClass) * MethodDecls;

CxList inMethod = inMethod1 + inMethod2;
inMethod -= MethodsOfInterface;	// remove Methods Of Interface - for sake of performance

result = All.FindByFathers(All.FindByFathers(inMethod)).FindByType(typeof(ParamDecl));