/* MISRA CPP RULE 16-0-5
 ------------------------------
 This query checks if there are arguments to a function-like macro don't contain tokens that look 
 like preprocessing directives.
 
 The Example below shows code with vulnerability: 
      
      #define M(A) printf(#A) //non-compliant  
 
      #define foo(X) #X+X     //non-compliant

      #define bar(Z) (#Z)     //non-compliant
*/


// makes sure that the the arguments to a function-like macro don't contain tokens that look like preprocessing directives.
result = All.FindByRegex(@"#define\s+\w+\s*[(]((?<par>\w+),?)+[)]\s*(?=[(].*#\k<par>[^\w].*[)]|([^(]*#\k<par>[^\w].*)|\w+[(][^""']*#\k<par>[^\w].*[)])",
	All.NewCxList());