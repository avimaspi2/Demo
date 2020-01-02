if (param.Length == 3)
{
	CxList db1 = param[0] as CxList;
	CxList origDB = param[1] as CxList;
	bool checkForNull = (bool) param[2];

	if ((db1 * origDB).Count == 0)
	{
		CxList methods = db1;
		for(int i = 0; i < 5; i++)
		{
			methods.Add((Find_Methods() + origDB).GetByAncs(All.FindDefinition(methods)));
		}
		db1 = methods * origDB;
	}
	if (checkForNull)
	{
		CxList nul = All.FindByShortName("null"); // all null's
		CxList eqNull = nul.GetAncOfType(typeof(BinaryExpr)).FindByShortName("=="); // == null
		CxList nullIf = eqNull.GetAncOfType(typeof(IfStmt)); // if statements with null test
		CxList allNullIfContent = All.GetByAncs(nullIf); // keep all the content of these if's for the loop
		
		// Create a list of null if's db
		CxList db2 = db1.GetByAncs(nullIf);
		foreach (CxList dbInstance in db2)
		{ // for each potentially problematic db instance in the result
			// Find null if of this instance
			CxList relevantNullIf = dbInstance.GetAncOfType(typeof(IfStmt)) * nullIf;
			CxList nullIfContents = allNullIfContent.GetByAncs(relevantNullIf);
			
			// Find the parameters and their references
			CxList dbParams = nullIfContents.GetByAncs(nullIfContents.GetParameters(dbInstance));
			CxList dbParamsRef = nullIfContents.FindAllReferences(dbParams);

			// Find db params in the condition in the "if" and remove the relevant result
			CxList dbParamsInNullCheck = dbParamsRef.GetByAncs(eqNull);
			if (dbParamsInNullCheck.Count > 0)
			{
				db1 -= dbInstance;
			}
		}
	}
	result = db1;
}