List<string> names = new List<string>{
		"*orders*","*credit*","*invoice*","*booking*","*bill*","*payment*","*account*","*cash*","*customer*" };

CxList tables = All.FindByShortNames(names, false);

CxList inputs = Find_Inputs();
CxList db = Find_DB();

List<string> usersNames = new List<string>{"*user*","*cust*","*member*" };

CxList user = All.FindByShortNames(usersNames, false);

db = db.DataInfluencedBy(tables);
db = db - db.DataInfluencedBy(user);
result = inputs.DataInfluencingOn(db);