/*
 MISRA CPP RULE 7-3-2
 ------------------------------
 This query searches for functions named main that are not in the global scope.
 
 The Example below shows code with vulnerability: 

      class Foo
		{
		public:
			int main() { }; //Non-compliant
		};

		int main() { }; 	//Compliant

*/

CxList mains = All.FindByType(typeof(MethodDecl)).FindByShortName("main");
foreach(CxList curr in mains) {
	CxList currClass = mains.GetAncOfType(typeof(ClassDecl));
	CxList currNS = mains.GetAncOfType(typeof(NamespaceDecl));
	System.Text.RegularExpressions.Regex nsPattern = new 
		System.Text.RegularExpressions.Regex(@"Namespace\d+");
	string NSstr = ((NamespaceDecl) currNS.data.GetByIndex(0)).Name;
	if (currClass.FindByShortName("checkmarx_default_classname*").Count == 0 ||
		!nsPattern.IsMatch(NSstr) ) {
		result.Add(curr);
	}
}