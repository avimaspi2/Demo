if (param.Length == 2)
{
	try
	{
		CxList param1 = param[0] as CxList;
		string[] dbCommands = param[1] as string[];	
		
		CxList db = param1.GetTargetOfMembers();

		//Block below looks for direct DB acccess by X variable
		//for example:
		// x.OnSubmit();
		CxList dbRef = All.FindAllReferences(db);
		CxList dbTarget = dbRef.GetMembersOfTarget();
		CxList directDBAccess = All.NewCxList();
		foreach (string str in dbCommands)
		{
			directDBAccess.Add(dbTarget.FindByShortName(str, false));
		}

		//If X is returned value, it may be used after it
		//we are looking who call the function that returns X
		//Example z = foo()
		// foo(){ Dim X = db(product); Return X}

		CxList returnStmt = dbRef.GetAncOfType(typeof(ReturnStmt));// looks Anc that returns X
		CxList returnStmtAncMethod = returnStmt.GetAncOfType(typeof(MethodDecl)); // Tooks just functions it self: foo()
		CxList refDBAccessMethods = All.FindAllReferences(returnStmtAncMethod) - returnStmtAncMethod; //looks for z = foo()
		//Build the methdos that applied 
		// For example: 	Accessor.GetTopFiveOrdersById(OrderPosition).InsertOnSubmit(cust)
		CxList theTarget = refDBAccessMethods.GetMembersOfTarget();
		CxList DBAccessRightSide = All.NewCxList(); 
		foreach (string str in dbCommands)
		{
			DBAccessRightSide.Add(theTarget.FindByShortName(str, false));
		}

		// The block below looks for "second oreder"
		// For example
		// 1) 	Dim xxx
		//		xxx = GetOrderById(1)
		//		xxx.DeleteOnSubmit(cust)
		//
		// 2)  dataGridView1.DataSource = Accessor.GetTopFiveOrdersById(OrderPosition)	
		//	   dataGridView1.DataSource.InsertOnSubmit(cust)

		CxList secondOrderLinq = All.GetByAncs(refDBAccessMethods.GetAncOfType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Left);
		CxList secondOrderLinqRef = All.FindAllReferences(secondOrderLinq);
		CxList secondOrderLinqTarget = secondOrderLinqRef.GetMembersOfTarget();

		CxList secondOrderDBAccess = All.NewCxList();
		foreach (string str in dbCommands)
		{
			secondOrderDBAccess.Add(secondOrderLinqTarget.FindByShortName(str, false));
		}
	
		result.Add(secondOrderDBAccess);
		result.Add(DBAccessRightSide);
		result.Add(directDBAccess);
	}
	catch (Exception ex)
	{
		
	}
}