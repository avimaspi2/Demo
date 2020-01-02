/*
MISRA C RULE 5-7
------------------------------
This query searches for identifier names that are reused anywhere in the code

	The Example below shows code with vulnerability: 

int foo();
char foo;

*/

CxList identifiers = Find_Identifiers();

// remove extern delcarations since by definition they do not "declare" an identifier, just repeat it
identifiers -= identifiers.GetByAncs(All.FindByFieldAttributes(Dom.Modifiers.Extern));

// remove method declaration if we have the definition
CxList methodDeclAndDefs = identifiers.FindByType(typeof(MethodDecl));
CxList methodDefs = methodDeclAndDefs * All.FindByType(typeof(StatementCollection)).GetFathers();
CxList methodDecls = methodDeclAndDefs - methodDefs;
CxList doubleMethods = identifiers.GetByAncs(methodDecls.FindByShortName(methodDefs));
identifiers -= doubleMethods;

SortedList identNames = new SortedList();
CxList duplicateNames = All.NewCxList();

// return all the duplicate identifier name instances
foreach(CxList curIdentifier in identifiers)
{
	string curName = (curIdentifier.data.GetByIndex(0) as CSharpGraph).ShortName;
	if (!identNames.Contains(curName)){
		identNames.Add(curName,null);
	}
	else{
		duplicateNames.Add(curIdentifier);
	}
}
identifiers.Add(doubleMethods);
result = identifiers.FindByShortName(duplicateNames);