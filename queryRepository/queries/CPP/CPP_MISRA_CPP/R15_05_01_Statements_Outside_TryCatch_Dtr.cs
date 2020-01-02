/* MISRA CPP RULE 15-5-1
 ------------------------------
 This query checks the appearance of elements outside of try-catch block inside the destructor
 
 The Example below shows code with vulnerability: 
     
      class A
      {
          ~A()
          {
              int a;     //non-compliant-outside of try-catch block
              try
              {            
              }   
              catch(int a)
              {
	                 try{
	                 }
                   catch(...)
                   {
	                 }
              }
              int b;     //non-compliant-outside of try-catch block
          }
     }
*/


//finds all destructors
CxList destructor  = All.FindByType(typeof(DestructorDecl));
CxList tryCatchFinally = All.FindByType(typeof(TryCatchFinallyStmt));
CxList stmt= All.FindByType(typeof(Statement));

CxList tryCatch = tryCatchFinally.GetByAncs(destructor);
CxList dtrWithTryCatch=tryCatch.GetFathers().GetFathers().FindByType(typeof(DestructorDecl));
CxList tc = tryCatch.GetAncOfType(typeof(TryCatchFinallyStmt));
CxList foundResults= All.GetByAncs(stmt.GetByAncs(destructor)) * stmt.GetByAncs(destructor) - All.GetByAncs(stmt.GetByAncs(tc));
result= foundResults.GetAncOfType(typeof(DestructorDecl));