CxList methods = Find_Methods();

// 1 - Direct DB function names
CxList directDbMethods =methods.FindByShortNames(new List<string> {"sqlite_exec", "sqlite_query", "sqlite_array_query",
	"sqlite_single_query","sqlite_unbuffered_query"});
/*
CxList sqlite = methods.FindByShortName("sqlite_*");
CxList directDbMethods =
	sqlite.FindByShortName("sqlite_exec") +
	sqlite.FindByShortName("sqlite_query") +
	sqlite.FindByShortName("sqlite_array_query") +
	sqlite.FindByShortName("sqlite_single_query") +
	sqlite.FindByShortName("sqlite_unbuffered_query");
*/
result.Add(directDbMethods);

// 2.1 - Implicit DB function names influenced by SQLite3
CxList SQLite = All.FindByShortName("SQLite3");

CxList QueryMethods = methods.FindByShortName("query");
CxList PrepareMethods = methods.FindByShortName("prepare");
CxList ExecMethods = methods.FindByShortName("exec");
CxList SingleMethods = methods.FindByShortName("querySingle");



result.Add((QueryMethods+PrepareMethods+ExecMethods+SingleMethods).DataInfluencedBy(SQLite));

// 2.2 - Implicit DB function names influenced by SQLite3
CxList ExecuteMethods = methods.FindByShortName("execute");
CxList ParamMethods = methods.FindByShortName("bindParam");
CxList ValueMethods = methods.FindByShortName("bindValue");

CxList SQLite3Execs = ExecuteMethods + ParamMethods + ValueMethods;
CxList SQLite3ExecuteInfluences = SQLite3Execs.DataInfluencedBy(PrepareMethods);

result.Add(SQLite3ExecuteInfluences);

// 2.3 - Implicit DB function names influenced by SQLiteDatabase (sqlite2)
CxList SQLite2 = All.FindByType(typeof(ObjectCreateExpr))
                    .FindByShortName("SQLiteDatabase");

CxList Query2Methods = methods.FindByShortName("query");
CxList Unbuffered2Methods = methods.FindByShortName("unbufferedQuery");
CxList Single2Methods = methods.FindByShortName("singleQuery");
CxList Array2Methods = methods.FindByShortName("arrayQuery");

CxList SQLite2Exec = Query2Methods + Unbuffered2Methods 
						+ Single2Methods + Array2Methods;

CxList SQLite2ExecInfluences = SQLite2Exec.DataInfluencedBy(SQLite2);

result.Add(SQLite2ExecInfluences);