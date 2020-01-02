CxList requests = Find_Interactive_Inputs();
CxList db = Find_DB();
CxList strings = Find_Strings();

CxList write = strings.FindByName("*update*", StringComparison.OrdinalIgnoreCase) +
	strings.FindByName("*delete*", StringComparison.OrdinalIgnoreCase) +
	strings.FindByName("*insert*", StringComparison.OrdinalIgnoreCase);

result = db.DataInfluencedBy(write).DataInfluencedBy(requests);