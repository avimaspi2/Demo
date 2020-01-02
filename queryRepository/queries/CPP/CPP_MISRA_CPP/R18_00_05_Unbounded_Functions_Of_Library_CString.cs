/*
 MISRA CPP RULE 18-0-5
 ------------------
  The unbounded functions of library <cstring> shall not been used.

  The Example below shows code with vulnerability: 

  			#include <cstring>          
 	 			void fn (const char * pChar){
 	 				char array [10];
 					strcpy (array, pChar); //Non-compliant
				}

*/

CxList methods = Find_Methods();
result = methods.FindByShortNames(new List<string>(){
		"strcpy","strcmp","strcat","strchr","strspn","strcspn",
		"strpbrk","strrchr","strstr","strtok","strlen"});