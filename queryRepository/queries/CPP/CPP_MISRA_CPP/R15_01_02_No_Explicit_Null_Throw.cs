/* MISRA CPP RULE 15-1-2
 ------------------------------
 This query finds all explicit NULL throws.
 checks in throw(NULL) exists.


 The following example shows what main code should look like: 
     int foo()
     {
         throw(NULL);                           //non-compliant
         try
         {
            throw(NULL)                         //non-compliant
         }
         catch
         {
             ...  
         }
         char * a=NULL;
         throw a;                               //compliant
         throw(static_cast <char *>(NULL));     //compliant 
    }

*/

//finds all throw statements
CxList thr = All.FindByType(typeof(ThrowStmt));
//finds their params
CxList refer=All.GetByAncs(thr).FindByType(typeof(UnknownReference));
//checks if there is NULL in the params
CxList findNull= refer.FindByName("null");
//checks if NULL is explicit
CxList findParentOfNull = findNull.GetFathers();
result = findParentOfNull.FindByType(typeof(ThrowStmt));