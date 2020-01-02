/* MISRA CPP RULE 16-0-4
 ------------------------------
 This query checks if there are function-like macros defined in the code.
 
 The Example below shows code with vulnerability: 

      #define  FUNC_MACRO(X) X*X  non-compliant
           
*/


// finds all function- like macros that are not string and commented out by double slash 
result = All.FindByRegex(@"#define\s+\w+\s*[(](\w*\,?)*[)]", All.NewCxList());