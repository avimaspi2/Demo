result = Find_Methods().FindByShortName("SQLConnect") + 
	Find_Methods().FindByMemberAccess("Connection.open") +
	Find_ObjectCreations().FindByShortName("*Connection") +
	All.FindByMemberAccess("Connection.ConnectionString") +
	Find_Methods().FindByMemberAccess("MySQL_Driver.connect");