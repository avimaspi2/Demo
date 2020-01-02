CxList methods = Find_Methods();

// 1 - Direct DB function names

CxList fetch = methods.FindByShortName("fetch*");

CxList directDbMethods = methods.FindByShortNames(new List<string>
	{"sqlite_fetch_all", "sqlite_fetch_array", 
		"sqlite_fetch_object", "sqlite_fetch_single", "sqlite_fetch_string","sqlite_field_name"});
/*
CxList sqlite_fetch = methods.FindByShortName("sqlite_fetch_*");
CxList directDbMethods =
	sqlite_fetch.FindByShortName("sqlite_fetch_all") +
	sqlite_fetch.FindByShortName("sqlite_fetch_array") +
	sqlite_fetch.FindByShortName("sqlite_fetch_object") +
	sqlite_fetch.FindByShortName("sqlite_fetch_single") +
	sqlite_fetch.FindByShortName("sqlite_fetch_string") +
	sqlite_fetch.FindByShortName("sqlite_field_name");
*/	
result.Add(directDbMethods);

// 2.1 - Implicit DB function names influenced by SQLite Unbuffered
CxList SQLite = All.FindByShortName("SQLiteDatabase");
CxList SQLiteU = methods.FindByShortNames(new List<String>(){ "unbufferedQuery", "sqlite_unbuffered_query" });

CxList AllMethods = fetch.FindByShortName("fetchAll");
CxList FetchMethods = fetch.FindByShortName("fetch");
CxList ObjectMethods = fetch.FindByShortName("fetchObject");
CxList SingleMethods = fetch.FindByShortName("fetchSingle");

CxList SQLite2FetchMethods = AllMethods + FetchMethods + ObjectMethods + SingleMethods; 
CxList SQLite2FetchInfluenced = SQLite2FetchMethods.DataInfluencedBy(SQLite + SQLiteU);
result.Add(SQLite2FetchInfluenced);

// 2.2 - Implicit DB function names influenced by SQLite Result
CxList SQLiteR = methods.FindByShortName("query").DataInfluencedBy(SQLite) +
	methods.FindByShortName("sqlite_query");

// 2.3 - Implicit DB function names influenced by SQLite3
CxList SQLite3 = All.FindByShortName("SQLite3");

CxList Array3Methods = fetch.FindByShortName("fetchArray");
CxList SQLite3FetchInfluenced = Array3Methods.DataInfluencedBy(SQLite3);
result.Add(SQLite3FetchInfluenced);