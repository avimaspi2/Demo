CxList tables = All.FindByShortName("*orders*", false); 
tables.Add(All.FindByShortName("*credit*", false));
tables.Add(All.FindByShortName("*invoice*", false));
tables.Add(All.FindByShortName("*booking*", false));
tables.Add(All.FindByShortName("*bill*", false));
tables.Add(All.FindByShortName("*payment*", false));
tables.Add(All.FindByShortName("*account*", false));
tables.Add(All.FindByShortName("*cash*", false)); 
tables.Add(All.FindByShortName("*customer*", false));

CxList inputs = Find_Interactive_Inputs();
CxList db = Find_DB();

CxList user = All.FindByName("*user*", false) + 
		  All.FindByName("*cust*", false) +
		  All.FindByName("*member*", false);

db = db.DataInfluencedBy(tables);
db = db - db.DataInfluencedBy(user);
result = inputs.DataInfluencingOn(db);