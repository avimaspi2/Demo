result = 
	All.FindByMemberAccess("SqlCommand.Parameters") +
	All.FindByMemberAccess("OracleCommand.Parameters") +
	All.FindByMemberAccess("OdbcCommand.Parameters") +
	All.FindByMemberAccess("OleDbCommand.Parameters") +
	
	All.FindByMemberAccess("SqlCommand.Parameters_*") +
	All.FindByMemberAccess("OracleCommand.Parameters_*") +
	All.FindByMemberAccess("OdbcCommand.Parameters_*") +
	All.FindByMemberAccess("OleDbCommand.Parameters_*") +
	
	All.FindByName("*.SelectCommand.Parameters") + 
	All.FindByName("*.UpdateCommand.Parameters") + 
	All.FindByName("*.InsertCommand.Parameters") +
	All.FindByName("*.DeleteCommand.Parameters") +
		
	All.FindByName("*.SelectCommand.Parameters_*") + 
	All.FindByName("*.UpdateCommand.Parameters_*") + 
	All.FindByName("*.InsertCommand.Parameters_*") +
	All.FindByName("*.DeleteCommand.Parameters_*") +
	
	// EntLib
	All.FindByMemberAccess("Database.AddInParameter") +
	All.FindByMemberAccess("Database.AddOutParameter") +
	All.FindByMemberAccess("Database.AddParameter") +
	
	All.FindByMemberAccess("OracleDatabase.AddInParameter") +
	All.FindByMemberAccess("OracleDatabase.AddOutParameter") +
	All.FindByMemberAccess("OracleDatabase.AddParameter") +

	All.FindByMemberAccess("SqlDatabase.AddInParameter") +
	All.FindByMemberAccess("SqlDatabase.AddOutParameter") +
	All.FindByMemberAccess("SqlDatabase.AddParameter") +

	All.FindByMemberAccess("GenericDatabase.AddInParameter") +
	All.FindByMemberAccess("GenericDatabase.AddOutParameter") +
	All.FindByMemberAccess("GenericDatabase.AddParameter");	

result.Add(result.GetAncOfType(typeof(MethodInvokeExpr)));