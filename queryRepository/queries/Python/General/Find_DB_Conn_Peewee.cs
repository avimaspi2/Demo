string[] modules = new string[] {
	"peewee"
	};

string[] connStrings = new string[] {
	"SqliteDatabase",
	"MySQLDatabase",
	"PostgresqlDatabase",
	"Database"
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

result = Find_DB_Imports(modules, connStrings, imports, "Model");