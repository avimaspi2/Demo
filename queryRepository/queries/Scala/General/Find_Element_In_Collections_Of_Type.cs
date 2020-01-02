//This query searches for methods that return elements from the collections with the given type
if(param.Length > 0)
{
	
	CxList collections = Find_Collections_Of_Type(param);
	List < string > list = new List<string>(){"element","elementAt","get","peek","poll","remove", "set", 
			"getOrElse", "retain", "head", "last", "apply"};
	result = collections.GetMembersOfTarget().FindByShortNames(list);
}