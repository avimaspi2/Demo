result = All.FindByMemberAccess("SqlCommand.Parameters");
result.Add(All.FindByMemberAccess("OracleCommand.Parameters"));
result.Add(All.FindByMemberAccess("OdbcCommand.Parameters"));
result.Add(All.FindByMemberAccess("OleDbCommand.Parameters"));
	
result.Add(All.FindByMemberAccess("SqlCommand.Parameters_*"));
result.Add(All.FindByMemberAccess("OracleCommand.Parameters_*"));
result.Add(All.FindByMemberAccess("OdbcCommand.Parameters_*"));
result.Add(All.FindByMemberAccess("OleDbCommand.Parameters_*"));
	
result.Add(All.FindByName("*.SelectCommand.Parameters")); 
result.Add(All.FindByName("*.UpdateCommand.Parameters")); 
result.Add(All.FindByName("*.InsertCommand.Parameters"));
result.Add(All.FindByName("*.DeleteCommand.Parameters"));
		
result.Add(All.FindByName("*.SelectCommand.Parameters_*")); 
result.Add(All.FindByName("*.UpdateCommand.Parameters_*")); 
result.Add(All.FindByName("*.InsertCommand.Parameters_*"));
result.Add(All.FindByName("*.DeleteCommand.Parameters_*"));
	
	// EntLib
result.Add(All.FindByMemberAccess("Database.AddInParameter"));
result.Add(All.FindByMemberAccess("Database.AddOutParameter"));
result.Add(All.FindByMemberAccess("Database.AddParameter"));
	
result.Add(All.FindByMemberAccess("OracleDatabase.AddInParameter"));
result.Add(All.FindByMemberAccess("OracleDatabase.AddOutParameter"));
result.Add(All.FindByMemberAccess("OracleDatabase.AddParameter"));

result.Add(All.FindByMemberAccess("SqlDatabase.AddInParameter"));
result.Add(All.FindByMemberAccess("SqlDatabase.AddOutParameter"));
result.Add(All.FindByMemberAccess("SqlDatabase.AddParameter"));

result.Add(All.FindByMemberAccess("GenericDatabase.AddInParameter"));
result.Add(All.FindByMemberAccess("GenericDatabase.AddOutParameter"));
result.Add(All.FindByMemberAccess("GenericDatabase.AddParameter"));

result.Add(result.GetAncOfType(typeof(MethodInvokeExpr)));