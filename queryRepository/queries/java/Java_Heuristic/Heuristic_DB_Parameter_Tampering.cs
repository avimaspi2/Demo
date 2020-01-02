CxList possibleDb = Find_DB_Heuristic();

if (possibleDb.Count > 0)
{
	CxList tables = All.FindByName("*orders*", false);
	tables.Add(All.FindByName("*credit*", false));
	tables.Add(All.FindByName("*invoice*", false));
	tables.Add(All.FindByName("*booking*", false));
	tables.Add(All.FindByName("*bill*", false));
	tables.Add(All.FindByName("*payment*", false));
	tables.Add(All.FindByName("*account*", false));
	tables.Add(All.FindByName("*cash*", false));
	tables.Add(All.FindByName("*customer*", false));

	CxList inputs = Find_Interactive_Inputs();

	CxList user = All.FindByName("*user*", false);
	user.Add(All.FindByName("*cust*", false));
	user.Add(All.FindByName("*member*", false));

	possibleDb = possibleDb.DataInfluencedBy(tables);
	possibleDb -= possibleDb.DataInfluencedBy(user);
	result = inputs.DataInfluencingOn(possibleDb);
	
	if (result.Count > 0)
	{
		CxList db = Find_DB_In();
		db = db.DataInfluencedBy(tables);
		db -= db.DataInfluencedBy(user);

		result = Filter_Heuristic_Results(result, inputs, db);

	}
}