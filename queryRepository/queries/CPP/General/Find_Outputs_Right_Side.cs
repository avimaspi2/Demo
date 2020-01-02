CxList cout = All.FindByShortNames(
	new List<string>	{	"cout", 
							"cerr", 
							"clog", 
							"wcout", 
							"wcerr", 
							"wclog", 
							"ostream", 
							"iostream"});

result = All.GetByAncs(cout.GetAssigner());
result -= result.FindByType(typeof(BinaryExpr)); // unnecessary nodes to consider