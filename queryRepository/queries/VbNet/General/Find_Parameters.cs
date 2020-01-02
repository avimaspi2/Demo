CxList appendParameter = All.FindByMemberAccess("Parameters.Append", false);
appendParameter.Add(All.FindByMemberAccess("Parameters.Add", false));
appendParameter.Add(All.FindByMemberAccess("Parameters.AddWithValue", false));

CxList parameters_ = All.FindByShortName("Parameters_*", false).GetMembersOfTarget();

result = All.GetParameters(appendParameter);
result.Add(parameters_.FindByShortName("Value", false));
result.Add(All.FindByType("SqlParameter", false));
result.Add(All.FindByType("OracleParameter", false));