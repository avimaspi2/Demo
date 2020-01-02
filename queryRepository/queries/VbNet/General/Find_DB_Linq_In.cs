//Find DB Linq Inputs
CxList linq = Find_DB_Linq_aux();

result = linq.FindByShortNames(new List<string> {
		"DeleteOnSubmit",
		"InsertOnSubmit",
		"UpdateOnSubmit",
		"SubmitChanges",
		"OnSubmit"}, false);