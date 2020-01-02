// Find all vulnerable commands - currently only buffRead
CxList buffRead = All.FindByMemberAccess("BufferedReader.read*");
// The second parameter is the vulnerable one
CxList buffReadParam2 = All.GetParameters(buffRead, 2);
// Al integeres
CxList numbers = Find_IntegerLiterals();

// All binary expressions
CxList bin = Find_BinaryExpr();
// ... +,-,*,/ do not have short names, but ==, &&, != etc. do have, which makes life a little simpler
bin = bin.FindByShortName("");

// Remove final and binary expreeion (only direct assignment of a number is relevant)
CxList sanitize = Find_Declarators().FindByRegex(@"\Wfinal\W");
sanitize.Add(Find_Dead_Code_Contents());
sanitize.Add(bin);


/// Every problematic parameter influenced (directly) by an ardcoded integer
result = buffReadParam2 * numbers;
result.Add(buffReadParam2.InfluencedByAndNotSanitized(numbers, sanitize));