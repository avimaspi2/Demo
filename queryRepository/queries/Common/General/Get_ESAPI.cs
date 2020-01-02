// Find all ESAPI classes and their references
CxList ESAPI = All.FindByMemberAccess("ESAPI.*");
ESAPI.Add(All.FindAllReferences(ESAPI.GetAncOfType(typeof(Declarator))));

// Find all ESAPI imports
CxList esapiImports = Find_Import().FindByName("*esapi.*", false);

// Find all the declarators in tiles that contain ESAPI imports
CxList declarators = All.NewCxList();
CxList dec1 = Find_Declarators();
foreach (CxList import in esapiImports)
{
	Import importGraph = import.TryGetCSharpGraph<Import>();
	// Use only declarators in the file that contains this import
	declarators.Add(dec1.FindByFileName(importGraph.LinePragma.FileName));
}
CxList potentialESAPI = All.FindAllReferences(declarators);

CxList allEsapi = All.NewCxList();
allEsapi.Add(ESAPI);
allEsapi.Add(potentialESAPI);

// Return the ESAPI members
result = allEsapi.GetMembersOfTarget();