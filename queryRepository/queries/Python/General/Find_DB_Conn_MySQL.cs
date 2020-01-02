string[] modules = new string[] {
	"MySQLdb",
	"mysql",
	"oursql",
	"pymysql",
	"cymysql"
	};

string[] connStrings = new string[] {
	"connect",
	"connector.connect"
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