string[] modules = new string[] {
	"psycopg2",	
	"pg",
	"pgdb",
	"PgSQL",
	"pyPgSQL",
	//"postgresql",
	"postgresql.driver",
	"pg8000"
	};

string[] connStrings = new string[] {
	"connect",
	//"open",
	"DB",
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

result = Find_DB_Imports(modules, connStrings, imports, "unittest.TestCase");
result.Add(Find_Methods().FindByMemberAccess("postgresql.open"));