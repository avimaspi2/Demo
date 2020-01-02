/* MISRA CPP RULE 15-1-3
 ------------------------------
 This query finds all empty throw statements that are outside of catch statement

 The following example shows what main code should look like: 
     
     void f1()     
     {
          throw;           //non-compliant
          try
          {  
              throw 1;
              throw;       //non-compliant
          }      
          catch(int i)
          {
              throw;       //compliant
          }
          throw;           //non-compliant    
     }
*/


//finds all throw statements
CxList throws = All.FindByType(typeof(ThrowStmt));	
//finds their parameters
CxList throwWithParams = All.FindByFathers(throws);
//makes sure that the throw is empty
CxList emptyThrowParam=throwWithParams.FindByName("CX_RETHROW");
CxList emptyThrow = emptyThrowParam.GetAncOfType(typeof(ThrowStmt));
//is the empty throw inside of catch
CxList emptyThrowsInCatch = All.GetByAncs(emptyThrow.GetAncOfType(typeof(Catch)));
CxList illegalThrow=emptyThrow.FindByType(typeof(ThrowStmt));
result = emptyThrow - emptyThrowsInCatch;