// Query TOCTOU
// ============

// The query deal with race condition
// Time of Check Time Of Use
// Code example is following:

//      if( f.canWrite() == false ) {
// 		try	{
			// BUG, The state of the file may change in the meantime
//			Thread.sleep(1000);
//		}
//		catch 	{}    
	// Create the file if it didn't exist before the sleep function
//		FileWriter fw = new FileWriter(f);
//		fw.close();
//


//Look for all f.canWrite() methods
CxList canWriteMethods = All.FindByMemberAccess("File.canWrite");

// Choose all if statement that includes canWrite condition
CxList ifStmt = canWriteMethods.GetAncOfType(typeof(IfStmt));

// choose all sleep methods
CxList sleep = Find_Methods().FindByShortName("sleep");
sleep.Add(Find_Read_NonDB());

// choose id statements that includes sleep and .canWrite
ifStmt = ifStmt * sleep.GetAncOfType(typeof(IfStmt));

// if into this (If Statement) block application performance FileWriter
// This statement is heuristic vulnerability of TOCTOU
result = All.FindByType(typeof(ObjectCreateExpr)).FindByName("*FileWriter").GetByAncs(ifStmt);