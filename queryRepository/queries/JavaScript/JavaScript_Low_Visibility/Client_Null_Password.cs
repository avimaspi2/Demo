///////////////////////////////////////////////////////////////////////
// Client_Null_Password
// The vulnerability here is when a password is retrieved from a DB
//   and the retrieve fails, the password is null. Before comparing 
//   with user input we must make sure the password is not null.
// Source: passwords that were retrieved from DB
// Sink: Comparison of user input with the password
// Sanitizer: Comparison of retrieved password with null
///////////////////////////////////////////////////////////////////////

CxList passwords = All_Passwords();
CxList db = Find_DB_Out();
CxList inputs = Find_Inputs();

// Find all password that were retrieved from DB
CxList passwdFromDb = passwords.InfluencedBy(db).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

// Find inary expressions inside if
CxList binary = Find_Binarys();
CxList compare = binary.FindByShortNames(new List<string>{"==", "===", "!=", "!=="});
CxList ifstmt = Find_Ifs();
CxList binaryInIf = compare.FindByFathers(ifstmt);

// Passwords in if
CxList passwdInIf = passwdFromDb.GetByAncs(binaryInIf);

// Initialize sink
CxList sink = All.NewCxList();
CxList nullLiterals = Find_NullLiteral();
CxList sanitize = All.NewCxList();
CxList SuspectedSink = All.NewCxList();

foreach(CxList passwd in passwdInIf)
{
	// find comparison
	CxList comp = passwd.GetFathers();
	BinaryExpr bin = comp.TryGetCSharpGraph<BinaryExpr>();
	if(bin == null)
	{
		continue;
	}
	CxList nodesLeftRight = All.FindById(bin.Left.NodeId);
	nodesLeftRight.Add(All.FindById(bin.Right.NodeId));
	
	if((nodesLeftRight * nullLiterals).Count > 0)
	{
		// Sanitizer - the password is checked for null
		sanitize.Add(passwords.FindAllReferences(passwd));
	}
	else
	{
		// Potential sink - the password is compared without checking for null
		SuspectedSink.Add(nodesLeftRight);
	}
}

// If the suspect is influenced by input, then it is a sink 
SuspectedSink = SuspectedSink.InfluencedBy(inputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
// Add the parent (the comparison) to the sinks
sink.Add(SuspectedSink.GetFathers());

result = passwdFromDb.InfluencingOnAndNotSanitized(sink, sanitize);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);