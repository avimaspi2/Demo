/* MISRA CPP RULE 15-3-2
 ------------------------------
 This query finds all statements that are outside of try - catch all general statement in main

 The following example shows what main code should look like: 
     int main()
     {
          try
          {
           program code
          }
          catch(...)        
          {
            handle unexpected Exceptions
          }
          return 0;
     }


  The Example below shows code with vulnerability:
     int main()
     {
      ofstream myfile;                        //non-compliant
      myfile.open("example.txt");             //non-compliant -might throw an Exception
      myfile << "Writing this to a file.\n";  //non-compliant -might throw an Exception
      myfile.close();                         //non-compliant -might throw an Exception
        
          try
          {
           program code
          }
          catch(...)        
          {
            handle unexpected Exceptions
          }
          try{                      the following code will throw an exception that won't be handled by catch
               ofstream myfile;                        
               myfile.open("example.txt");            
               myfile << "Writing this to a file.\n"; 
               myfile.close();                         
          }catch(char a)
          {
          }   
          return 0;
     }

*/


//find all main methods
CxList vis = All.FindByShortName("main").FindByType(typeof(MethodDecl));
CxList allTryCatch = All.FindByType(typeof(TryCatchFinallyStmt)).GetByAncs(vis);
//get all statements
CxList stmt = All.FindByType(typeof(Statement));
CxList allRetStmt = stmt.FindByType(typeof(ReturnStmt));
CxList typeRef = All.FindByType(typeof(TypeRef));

foreach(CxList mainMeth in vis)
{
	//get all try-catch statemets in main method
	CxList tryCatch = allTryCatch.GetByAncs(mainMeth);
	//leave only external try- catch and remove all nested ones
	CxList lastCatches = All.NewCxList();

	foreach(CxList cur in tryCatch)
	{
		CxList parent = cur.GetFathers().GetFathers();
		if(parent != mainMeth)
		{
			tryCatch -= cur;
		}
	}
	//take care the format is according to MISRA 15-3-7 
	foreach(CxList temp in tryCatch){
	
		//find the last catch statement
		TryCatchFinallyStmt t = temp.data.GetByIndex(0) as TryCatchFinallyStmt;
		if(t != null && t.CatchClauses != null && t.CatchClauses.Count > 0)
		{
			Catch c = t.CatchClauses[t.CatchClauses.Count - 1];
			if(c != null)
			{
				lastCatches.Add(All.FindById(c.NodeId));
			}
		}
	
	}
	CxList notOkCatch = All.NewCxList();

	CxList catchesTypeRef = typeRef.FindByFathers(lastCatches);
	foreach(CxList cur in lastCatches)
	{
		CxList references = catchesTypeRef.FindByFathers(cur);
		if(references.Count > 0){
			notOkCatch.Add(cur);
		}
	}
	CxList stmtInMainMeth = stmt.GetByAncs(mainMeth);
	CxList retstmt = (allRetStmt * stmtInMainMeth).GetByAncs(mainMeth);
	CxList foundResults = stmtInMainMeth - stmtInMainMeth.GetByAncs(tryCatch) + notOkCatch.GetFathers() - retstmt;
	//result.Add(foundResults.GetAncOfType(typeof(MethodDecl)).FindByShortName("main"));
	result.Add(vis.GetMethod(foundResults));
}