/*
 MISRA CPP RULE 7-3-1
 ------------------------------
 Only definitions in global scope are main function, namespaces and extern "C" commands.

 The Example below shows code with vulnerability: 

	//file.cpp
	int func() { };		 	//Non-compliant
	namespace NS {			//Compliant
		int newFunc() { };	//Compliant
	}

*/

//Find typedefs that create compliance with Rule 3-9-2.
// start with all type objects
CxList basicTypes = All.FindByType(typeof(TypeRef));

// we only care about basic types
basicTypes = basicTypes.FindByName("char") +
	basicTypes.FindByName("short") +
	basicTypes.FindByName("int") +
	basicTypes.FindByName("long") +
	basicTypes.FindByName("float") +
	basicTypes.FindByName("double");

// remove redundent instances
basicTypes -= basicTypes.FindByFathers(All.FindByType(typeof(ObjectCreateExpr)));

// Find typedef'd types, save relevant ones in typedefs.
CxList typedefDecl = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetAncOfType(typeof(FieldDecl));
CxList typedefs = All.NewCxList();
foreach (CxList typedefD in typedefDecl) {
	CxList cut = All.FindByFathers(typedefD) * basicTypes;
	if(cut.Count > 0) {
		typedefs.Add(typedefD);
	}
}

//Build potential objects that may be under global namespace.
CxList methods = All.FindByType(typeof(MethodDecl));
CxList classes = All.FindByType(typeof(ClassDecl));
classes -= classes.FindByShortName("checkmarx_default_classname*");
CxList potent = All.FindByType(typeof(FieldDecl)) +
	All.FindByType(typeof(StructDecl)) +
	All.FindByType(typeof(EnumDecl)) +
	methods - methods.FindByShortName("main") - methods.FindByShortName("INCLUDEREPLACE")
	- typedefs;
potent -= potent.FindByShortName("checkmarx_default_*");

foreach(CxList obj in potent) {
	if (obj.FindByPosition(0).Count > 0) {
		continue;
	}
	// if obj is not in a class, still relevant.
	CxList scope = obj.GetAncOfType(typeof(ClassDecl));
	if (scope.FindByShortName("checkmarx_default_classname*").Count == 0) {
		continue;
	}
		
	//Check if obj is in a defined namespace.
	scope = obj.GetAncOfType(typeof(NamespaceDecl));
	
	if (scope.FindByShortName("Namespace*").Count >= 1) {
		result.Add(obj);
	}
}

//Take care of classes.
foreach(CxList obj in classes) {
	//Check if in defined namespace
	CxList scope = obj.GetAncOfType(typeof(NamespaceDecl));
	if (scope.FindByShortName("Namespace*").Count == 0){
		continue;
	}
	//Check if in a method
	scope = obj.GetAncOfType(typeof(MethodDecl));
	if (scope.Count > 0) {
		continue;
	}
	//If not, check if nested.
	string fileName = ((CSharpGraph) obj.data.GetByIndex(0)).LinePragma.FileName;
	CxList checkClass = classes - obj;
	checkClass = checkClass.FindByFileName(fileName);
	bool isNested = false;
	foreach (CxList checker in checkClass) {
		if (obj.GetByAncs(checker).Count > 0) {
			isNested = true;
			break;
		}
	}
	if(!isNested) {
		result.Add(obj);
	}
}