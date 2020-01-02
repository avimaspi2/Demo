/*
 MISRA CPP RULE 0-1-10
 ------------------------------
 This query searches for unused functions, unused prototype functions are tolerated.
 
 The Example below shows code with vulnerability: 

		void f1() { }
		void f2() { } 	//Non-compliant
		void f3();		//Compliant prototype
		int main() 
		{
			f1();
			return 0;
		}
*/

CxList protoMethods = All.FindByType(typeof(MethodDecl));
//Remove main and includes.
protoMethods -= protoMethods.FindByShortName("main") + protoMethods.FindByShortName("INCLUDEREPLACE");
//Split prototype methods and regular ones.
CxList methods = All.FindByFathers(protoMethods).FindByType(typeof(StatementCollection)).GetFathers();
protoMethods -= methods;
CxList methodInvokes = All.FindByType(typeof(MethodInvokeExpr));
//Remove regular methods with invokes under their definition.
CxList unused = methods - methods.FindDefinition(methodInvokes.FindByShortName(methods));
//Remove methods that were called by using-directive
CxList imports = All.FindByType(typeof(Import));
CxList classes = All.FindByType(typeof(ClassDecl));
CxList namespaces = All.FindByType(typeof(NamespaceDecl));
foreach(CxList cur in imports) {
	string[] use = ((Import) cur.data.GetByIndex(0)).Namespace.Split('.');
	if (use.Length != 2) {
		continue;
	}
	CxList curMethods = unused.FindByShortName(use[1]);
	unused -= curMethods.GetByClass(classes.FindByShortName(use[0]));
	unused -= curMethods.GetByAncs(namespaces.FindByShortName(use[0]));
}

//Keep prototype methods with invokes under their definition.
CxList protoUsed = protoMethods.FindDefinition(methodInvokes.FindByShortName(protoMethods));
CxList allParams = All.FindByType(typeof(ParamDecl)).GetParameters(protoUsed + unused);
CxList typerefs = All.FindByType(typeof(TypeRef)).GetByAncs(allParams.GetParameters(protoUsed + unused));
string oldFile = "";
CxList others = All.NewCxList();

//Check if prototype method was definition - and not method itself
foreach (CxList curr in protoUsed) {
	CSharpGraph currGraph = curr.data.GetByIndex(0) as CSharpGraph;
	string filename = currGraph.LinePragma.FileName;
	if(!filename.Equals(oldFile)) {
		others = unused.FindByFileName(filename);	
		oldFile = filename;
	}
	others = others.FindByShortName(curr);
	CxList currParams = allParams.GetParameters(curr);
	currParams = typerefs.GetByAncs(currParams);
	bool isOverride = true;
	foreach(CxList other in others) {
		CxList otherParams = allParams.GetParameters(other);
		otherParams = typerefs.GetByAncs(otherParams);
		//Check if otherMethod overrides currMethod.
		if(currParams.Count == otherParams.Count) {
			for(int i = 0; i < currParams.Count; i++) {
				string cName = ((TypeRef) currParams.data.GetByIndex(i)).TypeName;
				string oName = ((TypeRef) otherParams.data.GetByIndex(i)).TypeName;
				if(!cName.Equals(oName)) {
					isOverride = false;	
					break;
				}
			}//end for
		}//end if
		else {
			isOverride = false;
		}
		if (isOverride) {
			unused -= other;
			break;
		}
	}
}
result = unused;