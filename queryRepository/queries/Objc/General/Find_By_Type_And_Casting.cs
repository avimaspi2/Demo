// This query is used to look for a specific type in the following cases:
// 1. direct type: let u : NSURL 
// 2. type through casting: let u = x as String
// 
// Input for the query: the list where to search and the type to look for
if (param.Length == 2)
{
	CxList base_list = param[0] as CxList;
	string type = param[1] as string;
	
	result = base_list.FindByType(type);
	result.Add(Find_Castings(base_list, type));
}