CxList possibleDb = Find_DB_Heuristic();

if (possibleDb.Count > 0)
{
	CxList input = Find_Interactive_Inputs();

	CxList strings = Find_Strings();
	CxList Select = strings.FindByName("*select*", false);
	CxList Where = strings.FindByName("*where*", false);
	CxList And = strings.FindByName("*And *", false);
	And.Add(strings.FindByName("* And*", false));

	possibleDb = possibleDb.DataInfluencedBy(Select).DataInfluencedBy(Where);
	possibleDb -= possibleDb.DataInfluencedBy(And);

	result = possibleDb.DataInfluencedBy(input);

	if (result.Count > 0)
	{
		CxList db = Find_DB_For_Parameter_Tampering();
		
		result = Filter_Heuristic_Results(result, input, db);		

	}
}