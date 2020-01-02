result = All.FindByMemberAccess("ResultSet.*") - All.FindByMemberAccess("ResultSet.getClass*");
CxList callableStatement = All.FindByType("CallableStatement");
result.Add(callableStatement.GetMembersOfTarget());
result -= result.FindByShortName("getClass");