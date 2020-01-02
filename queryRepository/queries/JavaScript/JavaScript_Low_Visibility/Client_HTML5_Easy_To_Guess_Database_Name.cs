/*	Client_HTML5_Easy_To_Guess_Database_Name
	Looks for commonly used client-side database names.
	Supports both Web Database and IndexedDB databases. */

/* 1. Build a list of commonly used database names. */
List<string> dbNames = new List<string> ();
{
	string[] dbPrefixes = new string[] {"my", "test", "example", 
		"temp", "tmp", "offline", "client", "browser", "local"};
	string[] dbFactors = new string[] {"", "_"};
	string[] dbSuffixes = new string[] {"db", "database"};
	
	foreach (string p in dbPrefixes)
		foreach (string s in dbSuffixes)
			foreach (string f in dbFactors)
				dbNames.Add(p + f + s);
}

/* 2. Identify all the relevant DB methods and DB names in the source-code. */
CxList methods = Find_Methods();
CxList variables = Find_UnknownReference();
CxList window = variables.FindByShortName("window");
CxList strings = Find_String_Literal();



CxList membersVariables = window.GetMembersOfTarget();
membersVariables.Add(variables);

CxList indexedDBVendors = membersVariables.FindByShortNames(
	new List<string> {
		"indexedDB",
		"webkitIndexedDB",
		"msIndexedDB",
		"mozIndexedDB",
		"moz_indexedDB"});

CxList indexedDBOpen = methods.FindByShortName("open").InfluencedBy(indexedDBVendors);

CxList relevantStrings = strings.FindByShortNames(dbNames);

CxList relevantMethods = methods.FindByShortName("openDatabase");
relevantMethods.Add(indexedDBOpen);

result = relevantMethods.InfluencedBy(relevantStrings);