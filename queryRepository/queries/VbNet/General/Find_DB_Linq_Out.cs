//Find Linq Outputs
CxList linq = Find_DB_Linq_aux();

result = linq.FindByShortName("Select", false);
result.Add(linq.FindByShortName("SelectMany", false));