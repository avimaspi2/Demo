/*
MISRA CPP RULE 2-10-3
------------------------------
This query searches for typedef names that are reused anywhere in the code
Note: same typedef names in different namespaces is compliant
	
	The Example below shows code with vulnerability: 

typedef char char_t;
int char_t;

*/

CxList identifiers = Find_Identifiers();

// remove extern delcarations since by definition they do not "declare" an identifier, just repeat it
identifiers -= identifiers.GetByAncs(All.FindByFieldAttributes(Dom.Modifiers.Extern));
identifiers -= identifiers.FindByFileName(@"*stdint.h");

CxList typedefNames = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetFathers() * identifiers;

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

CxList duplicateTypedefs = identifiers.FindByShortName(typedefNames.FindByShortName(duplicateNames));

//return only duplicate typdefs which are under the same namespace
CxList typedefNamespaces = duplicateTypedefs.GetAncOfType(typeof(NamespaceDecl));

foreach (CxList curNamespace in typedefNamespaces)
{
	CxList typedefsInCurNamespace =  duplicateTypedefs.GetByAncs(curNamespace);
	if (typedefsInCurNamespace.Count > 1)
	{
		result.Add(typedefsInCurNamespace);
	}
}