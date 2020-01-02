// Find all ESAPI classes and their references
CxList ESAPI = All.FindByMemberAccess("ESAPI.*");
ESAPI.Add(All.FindAllReferences(ESAPI.GetAncOfType(typeof(Declarator))));

// Find all ESAPI imports
CxList esapiImports = All.FindByType(typeof(Import)).FindByName("*esapi.*", false);

// Find all the declarators in tiles that contain ESAPI imports
CxList declarators = All.NewCxList();
CxList dec1 = All.FindByType(typeof(Declarator));
foreach (CxList import in esapiImports)
{
	Import importGraph = import.data.GetByIndex(0) as Import;
	// Use only declarators in the file that contains this import
	declarators.Add(dec1.FindByFileName(importGraph.LinePragma.FileName));
}
CxList potentialESAPI = All.FindAllReferences(declarators);


// Return the ESAPI members
result = (ESAPI + potentialESAPI).GetMembersOfTarget();