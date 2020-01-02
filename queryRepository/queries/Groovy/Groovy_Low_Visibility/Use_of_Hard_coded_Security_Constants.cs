// Find all vulnerable commands - currently only buffRead
CxList buffRead = All.FindByMemberAccess("BufferedReader.read*");
// The second parameter is the vulnerable one
CxList buffReadParam2 = All.GetParameters(buffRead, 2);
// Al integeres
CxList numbers = All.FindByType(typeof(IntegerLiteral));

// All binary expressions
CxList bin = All.FindByType(typeof(BinaryExpr));
// ... +,-,*,/ do not have short names, but ==, &&, != etc. do have, which makes life a little simpler
bin = bin.FindByShortName("");

// Remove final and binary expreeion (only direct assignment of a number is relevant)
CxList sanitize = 
	All.FindByType(typeof(Declarator)).FindByRegex(@"\Wfinal\W") + 
	Find_Dead_Code_Contents() +
	bin;


/// Every problematic parameter influenced (directly) by an ardcoded integer
result = buffReadParam2 * numbers + 
	buffReadParam2.InfluencedByAndNotSanitized(numbers, sanitize);