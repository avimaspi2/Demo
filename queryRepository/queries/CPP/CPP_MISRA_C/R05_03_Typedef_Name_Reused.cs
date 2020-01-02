/*
MISRA C RULE 5-3
------------------------------
This query searches for typedef names that are reused anywhere in the code

	The Example below shows code with vulnerability: 

typedef char char_t;
int char_t;

*/

CxList identifiers = Find_Identifiers();

// remove extern delcarations since by definition they do not "declare" an identifier, just repeat it
identifiers -= identifiers.GetByAncs(All.FindByFieldAttributes(Dom.Modifiers.Extern));
identifiers -= identifiers.FindByFileName(@"*stdint.h");
CxList typedefNames = Find_TypeAliasDecl() * identifiers;

SortedList identNames = new SortedList(identifiers.Count);
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

result = identifiers.FindByShortName(typedefNames.FindByShortName(duplicateNames));