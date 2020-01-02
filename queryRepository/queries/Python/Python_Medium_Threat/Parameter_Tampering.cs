CxList input = Find_Interactive_Inputs();
CxList db = Find_DB_In();
CxList strings = Find_Strings();


List<string> tablesNames = new List<string>{
		"*orders*","*credit*","*invoice*","*booking*","*bill*",
		"*payment*","*account*", "*cash*","*customer*" };

CxList tables = All.FindByShortNames(tablesNames, false);

List<string> userNames = new List<string>{
		"*user*","*cust*","*member*" };

CxList user = All.FindByShortNames(userNames, false);

CxList db2 = db.DataInfluencedBy(tables);
db2 = db2 - db2.DataInfluencedBy(user);

CxList Select = strings.FindByShortName("*select*", false);
CxList Where = strings.FindByShortName("*where*", false);

List<string> andNames = new List<string>{
		"*And *","* And*" };

CxList And = strings.FindByShortNames(andNames, false); 

CxList db3 = db.DataInfluencedBy(Select).DataInfluencedBy(Where);
db3 -= db3.DataInfluencedBy(And);
CxList sanitize = Find_Parameter_Tampering_Sanitize();

CxList dbAll = All.NewCxList();
dbAll.Add(db2);
dbAll.Add(db3);

result = dbAll.InfluencedByAndNotSanitized(input, sanitize);