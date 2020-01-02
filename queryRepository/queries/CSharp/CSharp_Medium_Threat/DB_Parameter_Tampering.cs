CxList tables = All.FindByShortNames(new List<string> {
		"*orders*",
		"*credit*",
		"*invoice*",
		"*booking*",
		"*bill*",
		"*payment*",
		"*account*",
		"*cash*",
		"*customer*"}, false);

CxList inputs = Find_Interactive_Inputs();
CxList db = Find_DB_In();

CxList user = All.FindByShortNames(new List<string> {
		"*user*",
		"*cust*",
		"*member*"}, false);

db = db.DataInfluencedBy(tables);
db -= db.DataInfluencedBy(user);
result = inputs.DataInfluencingOn(db);