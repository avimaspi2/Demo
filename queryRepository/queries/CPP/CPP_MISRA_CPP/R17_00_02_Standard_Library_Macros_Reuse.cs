/*
 MISRA CPP RULE 17-0-2
 ------------------
  The names of standard library macros and objects shall not be reused

  The Example below shows code with vulnerability: 

  			#define NULL 2 //Non-compliant


*/

//Macros from http://www.cplusplus.com/reference/clibrary/cstdio :

result = All.FindByRegex(@"#\s*define\s+NULL\s+", null)+ 
	All.FindByRegex(@"#\s*define\s+EOF\s+", null) +
	All.FindByRegex(@"#\s*define\s+FILENAME_MAX\s+", null) +
	All.FindByRegex(@"#\s*define\s+TMP_MAX\s+", null);