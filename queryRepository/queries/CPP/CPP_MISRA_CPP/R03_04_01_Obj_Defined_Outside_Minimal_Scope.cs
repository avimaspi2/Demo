/*
 MISRA CPP RULE 3-4-1
 ------------------------------
 This query searches for declarations of identifiers outside their minimal scope.

 The Example below shows code with vulnerability:

	int foo(int k) 
	{
		int j = k++;	//Non-compliant
		{
			int i = j;	//Compliant
		}
	}

*/

CxList decls = Find_All_Declarators();
CxList unknownRefs = All.FindByType(typeof(UnknownReference)).FindAllReferences(decls);
decls = All.FindDefinition(unknownRefs);

string oldfile = "";
CxList oldUnknown = All.NewCxList();

foreach (CxList decl in decls) {
	CxList scope = decl.GetAncOfType(typeof(StatementCollection));
	if (scope.Count == 0){
		scope = decl.GetAncOfType(typeof(ClassDecl));
		if (scope.Count == 0){
			scope = decl.GetAncOfType(typeof(StructDecl));
		}
	}
	string declFile = ((CSharpGraph) decl.data.GetByIndex(0)).LinePragma.FileName;
	CxList refs = All.NewCxList();
	if (!declFile.Equals(oldfile)) {
		oldUnknown = unknownRefs.FindByFileName(declFile);
	}
	refs = oldUnknown.FindAllReferences(decl) - decl;
	oldfile = declFile;
	if (refs.Count == 0) {
		continue;
	}
	int counter = 0;
	CxList firstRef = All.NewCxList();
	CxList refScope = All.NewCxList();
	while (refScope.Count == 0 && counter < refs.Count){
		firstRef = All.FindById(((CSharpGraph) refs.data.GetByIndex(counter)).NodeId);
		counter++;
		refScope = firstRef.GetAncOfType(typeof(StatementCollection));
		if (refScope.Count == 0){
			refScope = firstRef.GetAncOfType(typeof(ClassDecl));
			if (refScope.Count == 0){
				refScope = firstRef.GetAncOfType(typeof(StructDecl));
			}
		}
	}
	while (scope != refScope && scope.FindByShortName(refScope).Count == 0 && refScope.Count != 0 ) {
		CxList oldref = refScope;
		refs -= refs.GetByAncs(refScope);//Remove all refs under current refScope
		if (refs.Count == 0) {
			result.Add(decl);
			break;
		}
		//Find refScope's scope
		firstRef = refScope.GetFathers();
		refScope = firstRef.GetAncOfType(typeof(StatementCollection));
		if (refScope.Count == 0){
			refScope = firstRef.GetAncOfType(typeof(ClassDecl));
			if (refScope.Count == 0){
				refScope = firstRef.GetAncOfType(typeof(StructDecl));
			}
		}
		
	}
}