/*
	This query finds all outputs that came from stored sources 
	that might expose a threat when not correctly sanitized 
*/
CxList fromDB = All.NewCxList();
CxList fromFiles = All.NewCxList();
CxList stored = All.NewCxList();
CxList outputs = All.NewCxList();
CxList sanitize = All.NewCxList();

fromDB.Add(Find_DB());

fromFiles = Find_Read();

stored.Add(fromDB);
stored.Add(fromFiles);
outputs = Find_XSS_Outputs();
sanitize = Find_XSS_Sanitize();

// First results added
result = All.FindXSS(stored, outputs, sanitize);

// Next there is a need to join broken paths.
// FIRST PATH of the missing path:
CxList open = All.NewCxList();
CxList read = All.NewCxList();
open.Add(All.FindByMemberAccess("\"os\".*").FindByShortNames(new List<string>{"Open", "OpenFile"}));
read.Add(fromFiles.FindByShortName("ReadAt"));
read.Add(fromFiles.FindByShortName("Read"));
CxList firstPath = read.InfluencedBy(open); // no need to check for sanitizers here  
CxList arguments = All.GetParameters(read, 0);

// Iterate through previous nodes and link them to the SECOND PATH
CxList missedResults = All.NewCxList();
foreach ( CxList cx in firstPath.GetCxListByPath() )
{
	CxList arg = arguments.GetParameters(cx.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly), 0);
	
	// only make the concatenation if the second path is not sanitized 
	CxList secondPath = arg.InfluencingOnAndNotSanitized(outputs,sanitize);
	if ( secondPath.Count != 0 ) 
	{
		missedResults.Add(cx.ConcatenatePath(secondPath, false));
	}
}

result.Add(missedResults);