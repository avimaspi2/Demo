CxList possible_db = Find_DB_Heuristic();

if (possible_db.Count > 0)
{
	CxList tables = All.FindByName("*orders*", false) + 
		All.FindByName("*credit*", false) +
		All.FindByName("*invoice*", false) +
		All.FindByName("*booking*", false) +
		All.FindByName("*bill*", false) +
		All.FindByName("*payment*", false) +
		All.FindByName("*account*", false) +
		All.FindByName("*cash*", false) + 
		All.FindByName("*customer*", false);

	CxList inputs = Find_Interactive_Inputs();

	CxList user = All.FindByName("*user*", false) + 
		All.FindByName("*cust*", false) +
		All.FindByName("*member*", false);

	possible_db = possible_db.DataInfluencedBy(tables);
	possible_db -= possible_db.DataInfluencedBy(user);
	result = inputs.DataInfluencingOn(possible_db);
	
	if (result.Count > 0)
	{
		CxList db = Find_DB();
		db = db.DataInfluencedBy(tables);
		db -= db.DataInfluencedBy(user);
		result -= inputs.DataInfluencingOn(db);
	}
}