result = All.FindByMemberAccess("*Statement.getResultSet");
result.Add(All.FindByMemberAccess("ResultSet.get*"));
result.Add(All.FindByType("CallableStatement").GetMembersOfTarget());