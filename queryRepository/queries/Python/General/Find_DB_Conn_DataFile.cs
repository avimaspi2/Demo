string[] modules = new string[] {
	"data"
	};

string[] connStrings = new string[] {
	"DefaultData"
	};

CxList imports = All.NewCxList();

if (param.Length == 1)
{
	try
	{
		imports = param[0] as CxList;
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
} else {
	imports = Find_Imports();
}

result = Find_DB_Imports(modules, connStrings, imports, null);