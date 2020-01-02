/// <summary>
/// Return All in case django exists and empty list in case not.
/// </summary>
CxList imports = Find_Imports();
result = All.NewCxList();

foreach (CxList import in imports)
{
	try
	{
		string importName = import.TryGetCSharpGraph<Import>().Namespace;
		if (!String.IsNullOrEmpty(importName) && importName.StartsWith("django"))
		{
			result = imports;
			break;
		}	
	}
	catch {
		cxLog.WriteDebugMessage("Failed to extract import name");
	}
}