/*
    Sinks:
	Usage of an implemented X509TrustManager (or X509ExtendedTrustManager) without implementing its methods, 
    or returning a null object. Which means no validation is done and all certificates will be accepted. 
    Another possible sink is the hostname verification returning true for all hostnames.
*/

CxList classDeclarations = Find_ClassDecl();
CxList typeReferences = Find_TypeRef();
CxList methods = Find_Methods();
CxList returnStmts = Find_ReturnStmt();
CxList methodDecls = Find_MethodDecls();
CxList nullLiterals = Find_NullLiteral();
CxList trueAbsValue = Find_True_Abstract_Value();

// Get implementations of the TrustManager abstract class
CxList trustManagerImpl = classDeclarations.InheritsFrom("X509ExtendedTrustManager");
trustManagerImpl.Add(classDeclarations.InheritsFrom("X509TrustManager"));

// Get occurences of empty return statements. Ex: "fun a():Unit {return}"
CxList emptyReturnStmts = returnStmts - All.FindByFathers(returnStmts).GetFathers();

// The TrustManager class forces the implementations of several 
// overloads of checkClientTrusted & checkServerTrusted methods
CxList trustManagerCheckMethods = methodDecls
	.FindByShortNames(new List<string> {"checkClientTrusted", "checkServerTrusted"});
CxList vulnerableMethods = emptyReturnStmts.GetAncOfType(typeof(MethodDecl)) * trustManagerCheckMethods;
//Ensure checkClient & checkServer methods return and nothing else. 
foreach(CxList vulnerableMethod in vulnerableMethods){
	MethodDecl md = vulnerableMethod.TryGetCSharpGraph<MethodDecl>();
	// Every method calls the class meta constructor, 
	// hence the " ...Count > 2". It refers to the return statement plus the meta call.
	if(md.Statements.Count > 2)
		vulnerableMethods -= vulnerableMethod;
}
CxList vulnerableTrsutManagerImpl = trustManagerImpl.GetClass(vulnerableMethods);
CxList vulnerableTrustManagerCreation = typeReferences.FindAllReferences(vulnerableTrsutManagerImpl)
	.GetAncOfType(typeof(ObjectCreateExpr));

CxList flowSinks = All.NewCxList();
flowSinks.Add(vulnerableTrustManagerCreation);

flowSinks.Add(vulnerableTrustManagerCreation.GetAncOfType(typeof(AssignExpr)).GetAssignee());
flowSinks.Add(vulnerableTrustManagerCreation.GetAncOfType(typeof(Declarator)));

CxList sslContextsInits = methods.FindByMemberAccess("SSLContext.init");

result = flowSinks.DataInfluencingOn(sslContextsInits);
//Add SSLContext.init calls with a null second parameter.
result.Add(sslContextsInits.FindByParameters(nullLiterals.GetParameters(sslContextsInits, 1)));

//HostnameVerifier sinks
CxList hostNameVerifierImpl = classDeclarations.InheritsFrom("HostnameVerifier");

CxList returnTrueStatements = trueAbsValue.GetFathers() * returnStmts;
CxList hostNameVerifierMethods = methodDecls.FindByShortName("verify");
CxList vulnerableHostVerMethods = returnTrueStatements.GetAncOfType(typeof(MethodDecl)) * hostNameVerifierMethods;
//Ensure verification methods returns true and nothing else.
foreach(CxList vulnerableMethod in vulnerableHostVerMethods){
	if(returnStmts.GetByAncs(vulnerableMethod).Count > 1)
		vulnerableHostVerMethods -= vulnerableMethod;
}
CxList vulnerableHostnameVerImpl = hostNameVerifierImpl.GetClass(vulnerableHostVerMethods);
CxList vulnerableHostnameVerCreation = typeReferences.FindAllReferences(vulnerableHostnameVerImpl)
	.GetAncOfType(typeof(ObjectCreateExpr));

flowSinks = All.NewCxList();
flowSinks.Add(vulnerableHostnameVerCreation);
flowSinks.Add(vulnerableHostnameVerCreation.GetAncOfType(typeof(AssignExpr)).GetAssignee());
flowSinks.Add(vulnerableHostnameVerCreation.GetAncOfType(typeof(Declarator)));

CxList httpURLConnDefaultHost = methods.FindByMemberAccess("HttpsURLConnection.setDefaultHostnameVerifier");
result.Add(flowSinks.DataInfluencingOn(httpURLConnDefaultHost));
result.Add(httpURLConnDefaultHost.FindByParameters(flowSinks.GetParameters(httpURLConnDefaultHost, 0)));