CxList tables = All.FindByShortNames(new List<String>()
	{"*orders*", "*credit*", "*invoice*", "*booking*", "*bill*", "*payment*", "*account*", "*cash*", "*customer*"}, false);

CxList inputs = Find_Interactive_Inputs();
CxList db = Find_DB_In();

CxList user = All.FindByShortNames(new List<String>(){"*user*", "*cust*", "*member*"}, false);

db = db.DataInfluencedBy(tables);
db -= db.DataInfluencedBy(user);
CxList sanitize = Find_Parameter_Tampering_Sanitize();

result = inputs.InfluencingOnAndNotSanitized(db, sanitize);