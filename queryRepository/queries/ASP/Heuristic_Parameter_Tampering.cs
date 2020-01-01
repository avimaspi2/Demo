CxList possible_db = Find_DB_Heuristic();

if (possible_db.Count > 0)
{
	CxList input = Find_Interactive_Inputs();

	CxList strings = Find_Strings();
	CxList Select = strings.FindByName("*select*", false);
	CxList Where = strings.FindByName("*where*", false);
	CxList And = strings.FindByName("*and *", false) + 
		strings.FindByName("* and*", false);

	possible_db = possible_db.DataInfluencedBy(Select).DataInfluencedBy(Where);
	possible_db -= possible_db.DataInfluencedBy(And);

	result = possible_db.DataInfluencedBy(input);

	if (result.Count > 0)
	{
		CxList db = Find_DB();
		db = db.DataInfluencedBy(Select).DataInfluencedBy(Where);
		db -= db.DataInfluencedBy(And);
		result -= db.DataInfluencedBy(input);
	}
}