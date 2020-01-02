CxList obj_new = Find_ObjectCreations();
CxList array_obj = Find_TypeRef().FindByType("Array");
CxList ur = Find_UnknownReference();

CxList m_a = Find_MemberAccesses();
CxList arrays = array_obj.FindByFathers(obj_new);
CxList left_side = Find_Assign_Lefts();

CxList uknow_arrays = left_side.FindByFathers(arrays.GetAncOfType(typeof(AssignExpr)));

CxList array_member_access = ur.FindAllReferences(uknow_arrays).GetMembersOfTarget();

array_member_access = array_member_access.FindByType(typeof(MemberAccess));
CxList filtered_array_member_access = All.NewCxList();

try{
	
	foreach(CxList array in array_member_access)
	{

		CSharpGraph cs = array.GetFirstGraph();
		MemberAccess ma = cs as MemberAccess;
		string member_access_name = ma.MemberName;
	
		int fakenumber; 
		bool canParse = int.TryParse(member_access_name, out fakenumber);
		if(canParse)
		{
			filtered_array_member_access.Add(array);
		}
	
	}
}catch (Exception ex)
{
	cxLog.WriteDebugMessage(ex);
}
result = m_a.FindByShortName("index*");
result.Add(filtered_array_member_access);