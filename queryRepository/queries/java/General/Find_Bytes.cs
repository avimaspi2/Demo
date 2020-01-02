// Finds all possible uses of class Byte
result = All.FindByType("byte", false);
result.Add(All.FindByMemberAccess("byte.*", false));
result.Add(All.FindByMemberAccess("ResultSet.getByte"));
result.Add(All.FindByMemberAccess("ResultSet.getBytes"));
result.Add(All.FindByMemberAccess("ResultSet.next"));