CxList tables =   All.FindByName("*orders*", false) + 
			All.FindByName("*credit*", false) +
			All.FindByName("*invoice*", false) +
			All.FindByName("*booking*", false) +
			All.FindByName("*bill*", false) +
			All.FindByName("*payment*", false) +
			All.FindByName("*account*", false) +
			All.FindByName("*cash*", false) + 
			All.FindByName("*customer*", false);

CxList inputs = Find_Read()+Find_DB();
CxList db = Find_DB();

CxList user = All.FindByName("*user*", false) + 
		  All.FindByName("*cust*", false) +
		  All.FindByName("*member*", false);

db = db.DataInfluencedBy(tables);
db = db - db.DataInfluencedBy(user);
result = inputs.DataInfluencingOn(db);