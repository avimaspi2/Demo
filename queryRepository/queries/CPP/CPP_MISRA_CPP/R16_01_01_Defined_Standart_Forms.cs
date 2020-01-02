/* MISRA CPP RULE 16-1-1
 ------------------------------
 This query finds all "defined" that appear in a non standart form.
 
 The only two permissible forms for the "defined" preprocessor operator are:
      1) defined(identifier)
      2) defined identifier   

 The Example below shows code with vulnerability: 

      #if defined (x>y) //non-compliant
      #if defined x+y   //non-compliant

 *Special case*
 Generation of the token defined during expansion of #if  and #elif can lead to an undefined behaviour.

      #define DEFINED defined
      #if DEFINED (X)   //non-compliant - undefined behaviour
      #if DEFINED Z     //non-compliant - undefined behaviour     
*/


// makes sure that the defined is in one of the two standard forms:
CxList nonStandart = All.FindByRegex(@"\s*#\s*(if|elif)\s*[^\n\r]*defined((?=\s+\w+[^\w\n\|\|&&]+\w)|(?=(\s*[(]\s*\w+[^\w\n\s]+\w*\s*[)])))",
	All.NewCxList());

//deals with the special case:
CxList specialCase = All.FindByRegex(@"#\s*define\s+(\w+)\s+defined\s+.*\n*.*\s*#(?=\s*if|elif\s+\1+\s+\w+|[(]\w+[)]\s+)", 
	All.NewCxList());

result = nonStandart + specialCase;