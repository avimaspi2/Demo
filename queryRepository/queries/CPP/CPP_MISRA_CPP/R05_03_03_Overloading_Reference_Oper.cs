/* MISRA CPP RULE 5-3-3
 ------------------------------
 This query finds all reference operator overloading.

 The following example shows what main code should look like: 
     
     class A
     {
         A operator&()              //non-compliant
         {
              implementation 
         }
     } 
     
*/

result = All.FindByRegex(@"operator\n*\s*&", All.NewCxList());