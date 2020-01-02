/* 
 MISRA CPP RULE 7-3-5
 ------------------------------
 This query searches for declarations of methods after using-declarations usage.
 
 The Example below shows code with vulnerability: 
 
    namespace NS{
    	int f(int);
	}
	using NS::f;
	namespace NS{
	    int f(long);	//Non-compliant
	}

*/

CxList imps = All.FindByType(typeof(Import));
imps -= imps.FindByRegex("using namespace ",false,false,false);
CxList methods = All.FindByType(typeof(MethodDecl));
CxList namespaces = All.FindByType(typeof(NamespaceDecl));

foreach(CxList imp in imps) {
	LinePragma impLp = ((CSharpGraph) imp.data.GetByIndex(0)).LinePragma;
	
	string ns = ((Import) imp.data.GetByIndex(0)).Namespace;
	string[] nsSplit = ns.Split('.');
	if( nsSplit.Length == 1 ){
		continue;
	}
	string methodName = nsSplit[nsSplit.Length - 1];
	string impNs = nsSplit[nsSplit.Length - 2];
	CxList currMethods = methods.FindByFileName(impLp.FileName).FindByShortName(methodName);
	CxList currNS = namespaces.FindByFileName(impLp.FileName).FindByShortName(impNs);
	for(int i = 0;i < currMethods.Count;i++) {
		CSharpGraph method = ((CSharpGraph) currMethods.data.GetByIndex(i));
		if(method.LinePragma.Line > impLp.Line && //method declaration has to be after using line
		currNS.FindByShortName(method.Namespace.Name).Count == 1) {//method namespace has to be same as in using
			result.Add(methods.FindById(method.NodeId));
		}
	}
}