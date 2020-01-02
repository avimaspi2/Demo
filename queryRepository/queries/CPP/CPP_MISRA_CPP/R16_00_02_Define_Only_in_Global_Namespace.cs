/* MISRA CPP RULE 16-0-2
 ------------------------------
 This query finds all macros that are defined not in the global namespace.
 
 The Example below shows code with vulnerability: 
      #ifndef MY_HDR   
      #define MY_HDR    //compliant
      namespace NS{
            #define foo //non-compliant
            #undef  foo //non-compliant	
     	}
*/


// finds all define and undef that are enclosed inside curly brackets

result = All.FindByRegex(@"#\s*(undef|define)\s+[^{]*}", All.NewCxList());