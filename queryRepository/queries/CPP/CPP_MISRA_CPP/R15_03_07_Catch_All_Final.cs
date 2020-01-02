/* MISRA CPP RULE 15-3-7   
 ------------------------------
 This query checks if catch-all appears last in try-catch statement in case of multiple catch handlers
 

 The Example below shows code with vulnerability:  
 
       void f1()
       {
             try
             {
                  ...
             }
             catch(...)
             {
                   //handle all exception types
             }          
             catch(int i)       //non-compliant-handler will never be called
             {
             }   
       }

*/

//finds all try-catch statements 
CxList trycatch = All.FindByType(typeof(TryCatchFinallyStmt));

CxList lastCatches = All.NewCxList();

//find the last catch in a try-catch scope
foreach(CxList temp in trycatch){
	TryCatchFinallyStmt t = temp.data.GetByIndex(0) as TryCatchFinallyStmt;
	if(t!=null && t.CatchClauses!=null && t.CatchClauses.Count > 1)
	{
		
		Catch c = t.CatchClauses[t.CatchClauses.Count - 1];
		if(c != null)
		{
			lastCatches.Add(All.FindById(c.NodeId));
		}
	}
}
CxList notOkCatch = All.NewCxList();
//checks if the catch is catch all and removes all the catch- all statements from the result.
CxList fatherOfCatch = All.NewCxList();
//gets the father of the catch in order to find out if it has parameters or not:
foreach(CxList cur in lastCatches)
{	
	
	CxList references = All.FindByFathers(cur).FindByType(typeof(TypeRef));
	if(references.Count>0)
		  notOkCatch.Add(cur);
		
}

result= notOkCatch;