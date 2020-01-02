/*
MISRA C RULE 5-2
------------------------------
This query searches for identifiers in an inner scope with same name as an identifier in outer scope, thus hiding it

	The Example below shows code with vulnerability: 

double before;   
int boo ()
{
	int before = 25;
}

*/


CxList identifiers = Find_Identifiers();

// remove extern delcarations since by definition they do not "declare" an identifier, just repeat it
identifiers -= identifiers.GetByAncs(All.FindByFieldAttributes(Dom.Modifiers.Extern));
// remove gotos
CxList gotos = All.FindByFathers(Find_All_Declarators());
gotos = gotos.FindByName("goto").GetFathers();
identifiers -= gotos;

// remove method declaration if we have the definition
CxList methodDeclAndDefs = identifiers.FindByType(typeof(MethodDecl));
CxList methodDefs = methodDeclAndDefs * All.FindByType(typeof(StatementCollection)).GetFathers();
CxList methodDecls = methodDeclAndDefs - methodDefs;
CxList doubleMethods = identifiers.GetByAncs(methodDecls.FindByShortName(methodDefs));
identifiers -= doubleMethods;


SortedList identNames = new SortedList();
CxList duplicateNames = All.NewCxList();
foreach(CxList curIdentifier in identifiers)
{
	string curName = (curIdentifier.data.GetByIndex(0) as CSharpGraph).ShortName;
	if (!identNames.Contains(curName)){
		identNames.Add(curName, null);
	}
	else{
		duplicateNames.Add(curIdentifier);
	}
}
identifiers.Add(doubleMethods);
identifiers = identifiers.FindByShortName(duplicateNames);

// for each identifier, count appearences
foreach(CxList curIdentifier in identifiers)
{	
//	if(curIdentifier != All.FindById(70333)) continue;
	CxList IdentAppearances = identifiers.FindByShortName(curIdentifier) - curIdentifier;

	if(IdentAppearances.Count > 0)
	{
		// an object is in larger or equal scope of another object if and only if its direct scope contains the other object
		CxList curScope = curIdentifier.GetAncOfType(typeof(StatementCollection));
		if (curScope.Count == 0){
			curScope = curIdentifier.GetAncOfType(typeof(MemberDeclCollection));
		}
		if (curScope.Count == 0){
			curScope = curIdentifier.GetAncOfType(typeof(MethodDecl));
		}
		if (curScope.Count == 0){
			curScope = curIdentifier.GetAncOfType(typeof(NamespaceDecl));
		}
		
		// find same name identifiers that have inner/equal scope
		CxList identAppearancesInCurScope = IdentAppearances.GetByAncs(curScope);
		
		// only add those in strictly inner scope
		foreach(CxList cur in identAppearancesInCurScope){
			CxList curTestScope = cur.GetAncOfType(typeof(MemberDeclCollection));
			
			// struct members are an exception to the rule
			if (curTestScope.Count > 0){
				continue;
			}
			
			curTestScope = cur.GetAncOfType(typeof(StatementCollection));
			if (curScope.Count == 0){
				curScope = curIdentifier.GetAncOfType(typeof(MethodDecl));
			}
			if (curScope.Count == 0){
				curTestScope = cur.GetAncOfType(typeof(NamespaceDecl));
			}

			if (curScope != curTestScope)
				result.Add(curIdentifier + cur);
		}
	}
}