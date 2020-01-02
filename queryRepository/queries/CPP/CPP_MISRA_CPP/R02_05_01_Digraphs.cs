/* MISRA CPP RULE 2-5-1
 ------------------------------
 This query finds all digraph statements
 
 Digraphs are <% %>, <: :>, %: , %:%:

 The Example below shows code with vulnerability:  

   void f(A < int > *a<:10:>)        //non-compliant
   {
       <% a<:0:>->f2 < 20 > (); %>   //non-compliant
       int k;
       if(k)
	     <% k = 6;%>                 //non-compliant
	     f(a);
   }
*/
 
result = All.FindByRegex(@"(<%)|(%>)|(<:)|(:>)|(%:)", All.NewCxList());